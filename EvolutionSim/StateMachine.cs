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


        //this method provides the logic for state transitions
        public void performAction(Organism _passedOrganism)
        {
            //test the organisms current attributes
            //by switching on the current state

            States organismState = (States)_passedOrganism._state.CurrentState;
            Action neededAction;
            State newState;


            switch (organismState)
            {

                case States.Roaming:

                    //if food < 50 then go to seek food, contuine to seek food until back to 80% full?
                    //diferent food sources have different values

                    if (_passedOrganism._attributes._hunger < 0.4)
                    {

                        //then move into the seek food state
                        _passedOrganism._state.MoveState(Action.HungryRoam);



                    }

                    if (_passedOrganism._attributes._hunger > 0.8)
                    {
                        //go find a mate
                        _passedOrganism._state.MoveState(Action.HungryMate);


                    }

                    else
                    {
                        Random newRand = new Random();

                        int MateOrEat = newRand.Next(0, 1);

                        if (MateOrEat == 1)
                        {
                            _passedOrganism._state.MoveState(Action.HungryMate);

                        }
                        else
                        {

                            _passedOrganism._state.MoveState(Action.HungryRoam);

                        }

                    }

                    break;

                //this code block handles logic when organism is the middle of eating
                case States.Eating:

                    //if there is food in food source then contuine eating

                    //if food hungry is maxed out (100) then stop eating && move back into roaming
                    if (_passedOrganism._attributes._hunger == 1.0)
                    {

                        _passedOrganism._state.MoveState(Action.NotHungry);

                    }

                    break;

                //organism needs way of tracking other organisms of the same species
                //
                case States.SeekMate:

                    break;

                case States.SeekFood:

                    // 1) call tracking method, 
                    // 2) if tracking method return true then the organism moves over to food and transitions into the eating state:

                    _passedOrganism._state.MoveState(Action.FoodFound);

                    break;


                case States.Mating:

                    break;


                default:

                    break;



            }



        }


        //needs some sort of behaviour method
        //need to tie in graphics to a behviour method of some sort 



    }
}
