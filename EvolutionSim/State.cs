using System;
using System.Collections.Generic;


//these represent states
enum States
{

    roaming = 0,
    eating = 1,
    repoducing = 2,
    seekMate = 3,
    seekFood = 4,
}

//these represent the transition psths between states
enum Action
{
    notHungry = 0,
    hungryRoam = 1,
    hungryMae = 2,



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

        public override bool Equals(Object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.action == other.action;

        }

    }

    // represent a transition table as a dictonary
    Dictionary <StateTransition, States> transitions;



    //Sets each state to roaming by default
	public State()
	{
       

	}


 

}
