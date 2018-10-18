using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    //State Machine Class
    //this class is used for describing the conditions for which a change in state occurs
    class StateMachine
    {

        //list of constructed organisms which are currently in game
        List<Organism> liveOrganisms = new List<Organism>();
        

    
    public void determineBehaviour()
        {

            //for everything in the live organisms list
            foreach (var thing in liveOrganisms)
            {
                //test the organisms current attributes
                //by switching on the current state

                States organismState = (States)thing._state.CurrentState;

                switch (organismState)
                {
                    case organismState:
                        break;

                    case

                   

                }


            }


           


        }

  
}
