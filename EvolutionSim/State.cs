using System;
using System.Collections.Generic;

//these represent states
public enum States
{
    Roaming,
    Eating,
    SeekMate,
    SeekFood,
    Mating,

}

//these represent the transition paths between states
public enum Action
{
    NotHungry,
    HungryRoam,
    HungryMate,
    FoodFound,
    MateFound,
    FinishedMating
}



    //Class used to represent the state of an organism
    public class State
    {


        //a nested and publically avaliable class to determine behaviour of organsim when changing states
        class StateTransition
        {
            readonly States CurrentState;
            readonly Action action;

        //StateTransition object constructor 
            public StateTransition(States PassedCurrentState, Action passedAction)
            {
                this.CurrentState = PassedCurrentState;
                this.action = passedAction;

            }

        //important to have a hash code for each stateTransition as it is being used as a key in
        //the dictonary (lookup table)
            public override int GetHashCode()
            {

                return 17 + 31 * CurrentState.GetHashCode() + 31 * action.GetHashCode();

            }

            public override bool Equals(Object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.action == other.action;

            }

        }

        // represent a transition table as a dictonary
        Dictionary<StateTransition, States> transitions;

    //getters and setters for States class
        public States CurrentState
    {
        get;
        private set;
    }


        //Sets each state to roaming by default
        public State()
        {
            CurrentState = States.Roaming;

        //opens a new dictonary which holds a StateTransition object as a key with the coresponding State enum
        transitions = new Dictionary<StateTransition, States>
            {
                //here we add all of the possible state transitions in

                //if organism is in roam state and isn't hungry then remain in roaming
                { new StateTransition (States.Roaming, Action.NotHungry), States.Roaming},

                //if organism is in roaming state and wants to mate then place into seekMate state
                { new StateTransition (States.Roaming, Action.HungryMate), States.SeekMate},

                { new StateTransition (States.SeekMate, Action.MateFound), States.Mating},

                // if the organism is seeking a mate but becomes too hungry then switch to seek food
                { new StateTransition (States.SeekMate, Action.HungryRoam), States.SeekFood},

                //once the organism has finished mating place back into the roaming state
                { new StateTransition(States.Mating, Action.FinishedMating), States.Roaming},

                //when the organism finds food place into the eating state
                { new StateTransition(States.Roaming, Action.FoodFound), States.Eating},

            };
    }


    //return the next state deterministically
    public States GetNext(Action action)
    {
        StateTransition transition = new StateTransition(CurrentState, action);
        States nextState;

        if (!transitions.TryGetValue(transition, out nextState))
            throw new Exception("The following is not a valid transition: " + CurrentState + "->" + action);

        return nextState;
    }

    //handles the moving of states.
    public States MoveState (Action action)
    {
        CurrentState = GetNext(action);
        return CurrentState;

    }



}
