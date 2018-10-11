using System;
using System.Threading;

//Class used to represent the state of an organism
public class State
{
    States currentAction;


    enum States{

        roaming = 0,
        eating =1,
        repoducing = 2,


    }
    
    //Sets each state to roaming by default
	public State()
	{
        this.currentAction = States.roaming;

	}



}
