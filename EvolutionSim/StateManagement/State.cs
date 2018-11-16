using System;
using System.Collections.Generic;

namespace EvolutionSim.StateManagement
{

    //these represent states
    public enum PotentialStates
    {
        Roaming,
        Eating,
        SeekMate,
        SeekFood,
        Mating,
        MovingToFood,
        WaitingForMate,
        MovingToMate,
    }

    //these represent the transition paths between states
    public enum Action
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

    }



    //Class used to represent the state of an organism
    public class State
    {


        //a nested and publically avaliable class to determine behaviour of organsim when changing states
        class StateTransition
        {
            readonly PotentialStates CurrentState;
            readonly Action action;

            //StateTransition object constructor 
            public StateTransition(PotentialStates PassedCurrentState, Action passedAction)
            {
                this.CurrentState = PassedCurrentState;
                this.action = passedAction;

            }

            // important to have a hash code for each stateTransition as it is being used as a key in
            // the dictonary(lookup table)
            public override int GetHashCode()
            {

                return 17 + 31 * this.CurrentState.GetHashCode() + 31 * this.action.GetHashCode();

            }

            public override bool Equals(Object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.action == other.action;

            }

        }

        // represent a transition table as a dictonary
        Dictionary<StateTransition, PotentialStates> transitions;
        
        //Sets each state to roaming by default
        public State()
        {
            //CurrentState = PotentialStates.Roaming;

            //opens a new dictonary which holds a StateTransition object as a key with the coresponding State enum
            this.transitions = new Dictionary<StateTransition, PotentialStates>
            {
                //here we add all of the possible state transitions in

                //if organism is in roam state and isn't hungry then remain in roaming
                { new StateTransition (PotentialStates.Roaming, Action.NotHungry), PotentialStates.Roaming},

                //if organism is in roaming state and wants to mate then place into seekMate state
                { new StateTransition (PotentialStates.Roaming, Action.HungryMate), PotentialStates.SeekMate},

                { new StateTransition (PotentialStates.Roaming, Action.HungryRoam), PotentialStates.SeekFood},

               // { new StateTransition (PotentialStates.SeekMate, Action.MateFound), PotentialStates.Mating},

                // if the organism is seeking a mate but becomes too hungry then switch to seek food
                { new StateTransition (PotentialStates.SeekMate, Action.HungryRoam), PotentialStates.SeekFood},

                //once the organism has finished mating place back into the roaming state
                { new StateTransition(PotentialStates.Mating, Action.FinishedMating), PotentialStates.Roaming},

                //when the organism finds food place into the eating state
                { new StateTransition(PotentialStates.SeekFood, Action.FoodDetected), PotentialStates.MovingToFood},

                { new StateTransition(PotentialStates.MovingToFood,Action.FoodFound),PotentialStates.Eating },

                {new StateTransition(PotentialStates.Eating,Action.NotHungry),PotentialStates.Roaming },
                
                { new StateTransition(PotentialStates.SeekMate, Action.MateFound), PotentialStates.MovingToMate},

                //now if an organism is waiting for a mate and takes the action move to go over to partner
                {new StateTransition(PotentialStates.SeekMate, Action.Waiting), PotentialStates.WaitingForMate},

                //we are finished with mating, so now go back to roaming!
                {new StateTransition(PotentialStates.WaitingForMate, Action.Move), PotentialStates.Roaming},

                {new StateTransition(PotentialStates.MovingToMate, Action.Bang), PotentialStates.Mating}

            };
        }


        //return the next state deterministically
        public PotentialStates GetNext(PotentialStates CurrentState, Action action)
        {
            StateTransition transition = new StateTransition(CurrentState, action);
            PotentialStates nextState;

            if (!this.transitions.TryGetValue(transition, out nextState))
            {
                throw new Exception("The following is not a valid transition: " + CurrentState + "->" + action);
            }

            return nextState;
        }

        //handles the moving of states.
        public PotentialStates MoveState(PotentialStates determinedState, Action action)
        {
            determinedState = GetNext(determinedState, action);
            return determinedState;

        }


        //used to test the transition logic
        public void testTransition()
        {


        }


    }
}