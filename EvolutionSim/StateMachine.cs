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
        

    
    public State performAction(Organism _passedOrganism)
        {
                //test the organisms current attributes
                //by switching on the current state

                States organismState = (States)_passedOrganism._state.CurrentState;
                Action neededAction;
                State newState;

                //then we're in roaming
                if (organismState == States.Roaming)
                {
                    //hunger falls below a certain percentage or value
            




                }


                else if(organismState == States.Eating)
                {





                }

                else if (organismState == States.SeekFood)
                {





                }

                else if (organismState == States.SeekMate)
                {





                }


                else 
                {





                }











            return newState;

   
        }

  
}
