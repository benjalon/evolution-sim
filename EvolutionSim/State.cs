using System;
using System.Collections.Generic;

//these represent states
public enum States
{
    roaming,
    eating,
    repoducing,
    seekMate,
    seekFood,
}

//these represent the transition psths between states
public enum Action
{
    notHungry,
    hungryRoam,
    hungryMate,
}



    //Class used to represent the state of an organism
    public class State
    {


        //a nested and publically avaliable class to determine behaviour of organsim when changing states
        class StateTransition
        {
            readonly States CurrentState;
            readonly Action action;

            public StateTransition(States PassedCurrentState, Action passedAction)
            {
                this.CurrentState = PassedCurrentState;
                this.action = passedAction;

            }

        //important to have a hash code for each stateTransition as it is being used as a key in
        //the dictonary (lookup table)
            public override int GetHashCode()
            {

                return 17 + 31 * CurrentState.GetHashCode() + 31 * Action.GetHashCode();

            }

            public override bool Equals(Object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.action == other.action;

            }

        }

        // represent a transition table as a dictonary
        Dictionary<StateTransition, States> transitions;

    //getters and setters for the differing states
        public States CurrentState
    {
        get;
        private set;
    }


        //Sets each state to roaming by default
        public State()
        {
            CurrentState = States.roaming;

            //opens a new dictonary which holds a StateTransition object as a key with the coresponding State enum
            transitions = new Dictionary<StateTransition, States>
            {





            }
    


    }

}
