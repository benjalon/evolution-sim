using EvolutionSim.Data;
using System;
using System.Collections.Generic;

namespace EvolutionSim.StateManagement
{

    //these represent the transition paths between states
    public enum Actions
    {
        NotHungry,
        HungryRoam,
        HungryMate,
        FoodDetected,
        FoodFound,
        MateFound,
        FinishedMating,
        Move,
        Waiting,
        Bang,
        LookingForPrey,
        FoundPrey,
        Hunt,
        finishedHunt,

    }



    //Class used to represent the state of an organism
    public class State
    {
        private const int HASHING_CONST_1 = 17;
        private const int HASHING_CONST_2 = 31;


        //a nested and publically avaliable class to determine behaviour of organsim when changing states
        class StateTransition
        {
            private readonly States currentState;
            private readonly Actions action;

            //StateTransition object constructor 
            public StateTransition(States currentState, Actions action)
            {
                this.currentState = currentState;
                this.action = action;

            }

            // important to have a hash code for each stateTransition as it is being used as a key in
            // the dictonary(lookup table)
            public override int GetHashCode()
            {

                return HASHING_CONST_1 + HASHING_CONST_2 * this.currentState.GetHashCode() + HASHING_CONST_2 * this.action.GetHashCode();

            }

            public override bool Equals(Object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.currentState == other.currentState && this.action == other.action;

            }

        }

        // represent a transition table as a dictonary
        private Dictionary<StateTransition, States> transitions;
        
        //Sets each state to roaming by default
        public State()
        {
            //CurrentState = PotentialStates.Roaming;

            //opens a new dictonary which holds a StateTransition object as a key with the coresponding State enum
            this.transitions = new Dictionary<StateTransition, States>
            {
                //here we add all of the possible state transitions in

                //if organism is in roam state and isn't hungry then remain in roaming
                { new StateTransition (States.Roaming, Actions.NotHungry), States.Roaming},

                //if organism is in roaming state and wants to mate then place into seekMate state
                { new StateTransition (States.Roaming, Actions.HungryMate), States.SeekMate},

                { new StateTransition (States.Roaming, Actions.HungryRoam), States.SeekFood},

                // if the organism is seeking a mate but becomes too hungry then switch to seek food
                { new StateTransition (States.SeekMate, Actions.HungryRoam), States.SeekFood},

                //once the organism has finished mating place back into the roaming state
                { new StateTransition(States.Mating, Actions.FinishedMating), States.Roaming},

                //when the organism finds food place into the eating state
                { new StateTransition(States.SeekFood, Actions.FoodDetected), States.MovingToFood},

                { new StateTransition(States.SeekFood, Actions.NotHungry), States.Roaming}, // The player has changed the organism's hunger level manually

                { new StateTransition(States.MovingToFood,Actions.FoodFound),States.Eating },

                { new StateTransition(States.MovingToFood,Actions.NotHungry),States.Roaming }, // Food has disappeared

                {new StateTransition(States.Eating,Actions.NotHungry),States.Roaming },

                { new StateTransition(States.SeekMate, Actions.MateFound), States.MovingToMate},

                //now if an organism is waiting for a mate and takes the action move to go over to partner
                {new StateTransition(States.SeekMate, Actions.Waiting), States.WaitingForMate},

                //we are finished with mating, so now go back to roaming!
                {new StateTransition(States.WaitingForMate, Actions.Move), States.Roaming},

                {new StateTransition(States.MovingToMate, Actions.Bang), States.Mating},

                { new StateTransition(States.MovingToMate,Actions.FinishedMating),States.Roaming }, // Mate has disappeared

                { new StateTransition(States.Roaming, Actions.LookingForPrey), States.FindingPrey},
                // predator is now hunting

                { new StateTransition(States.FindingPrey, Actions.FoundPrey), States.Hunting },

                { new StateTransition(States.Hunting, Actions.finishedHunt), States.Roaming},

              
            };
        }


        //return the next state deterministically
        public States GetNext(States currentState, Actions action)
        {
            StateTransition transition = new StateTransition(currentState, action);
            States nextState;

            if (!this.transitions.TryGetValue(transition, out nextState))
            {
                throw new Exception("The following is not a valid transition: " + currentState + "->" + action);
            }

            return nextState;
        }

        //handles the moving of states.
        public States MoveState(States determinedState, Actions action)
        {
            determinedState = GetNext(determinedState, action);
            return determinedState;

        }

    }
}