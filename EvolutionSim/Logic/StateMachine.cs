using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    //State Machine Class
    //this class is used for describing the conditions for which a change in state occurs and modeling organism behaviour
    public class StateMachine
    {
        //initalise lookup table
        private State _state;
        private Grid _grid;

        // Grid _simGrid;

        public StateMachine(Grid grid)
        {
          _grid = grid;
          _state = new State();


        }
     
        public void UpdateOrganismAttributes(Organism organism)
        {

            //hardcode a value that doesn't go down too fast
            if (organism._attributes._hunger > 0)
            {
                organism._attributes._hunger -= 0.0001;
            }
            else
            {
                organism._attributes._hunger = 0;
            }
 


        }

        /// <summary>
        /// This method is used for testing which state an organism is in, should be called in the update method
        /// </summary>
        /// <param name="_passedOrganism"></param>
        public void checkState(Organism _passedOrganism)
        {
            //test the organisms current attributes
            //by switching on the current state

            PotentialStates organismState = _passedOrganism.organismState;


            switch (organismState)
            {

                case PotentialStates.Roaming:

                    //if food < 50 then go to seek food, contuine to seek food until back to 80% full?
                    //diferent food sources have different values

                    if (_passedOrganism._attributes._hunger < 0.4)
                    {

                        //then move into the seek food state
                        _passedOrganism.organismState = _state.MoveState(organismState, Action.HungryRoam);



                    }

                    else if (_passedOrganism._attributes._hunger >= 0.8)
                    {
                        //go find a mate
                        _passedOrganism.organismState = _state.MoveState(organismState, Action.HungryMate);


                    }


                    break;

                //this code block handles logic when organism is the middle of eating
                case PotentialStates.Eating:

                    //if there is food in food source then contuine eating

                    //if food hungry is maxed out (100) then stop eating && move back into roaming
                    if (_passedOrganism._attributes._hunger == 1.0)
                    {

                        _passedOrganism.organismState = _state.MoveState(organismState, Action.NotHungry);

                    }

                    break;

                //organism needs way of tracking other organisms of the same species
                //
                case PotentialStates.SeekMate:

                    //if (_simGrid.TrackMate(_passedOrganism))
                    //{
                    //    _passedOrganism.organismState = _state.MoveState(organismState, Action.MateFound);

                    //}


                    break;

                case PotentialStates.SeekFood:

                    // 1) call tracking method located in grid, 
                    // 2) if tracking method return true then the organism moves over to food and transitions into the eating state:
                    //if (_simGrid.TrackFood(_passedOrganism))
                    //{
                    //    _passedOrganism.organismState = _state.MoveState(organismState, Action.FoodFound);

                    //}

                    //else
                    //{
                    //    //remain in the same state
                    //    return;
                    //}


                    break;

                //once an organism has begun mating it cannont stop or change state
                //once a certain time has elasped move back to roaming

                case PotentialStates.Mating:
                  
    

                    break;


                default:

                    break;

            }



        }

        /// <summary>
        /// This method controls how an organism goes about its buisness when in a given state
        /// </summary>
        ///         public void determineBehaviour(Organism _passedOrganism, GameTime gameTime)

        public void determineBehaviour(Organism _passedOrganism)
        {

            PotentialStates organismState = _passedOrganism.organismState;


            switch (organismState)
            {

                case PotentialStates.Roaming:
                    Logic.StateActions.Roam(_passedOrganism,_grid);

                    break;

                case PotentialStates.Eating:

                    break;
                case PotentialStates.Mating:

                    break;

                case PotentialStates.SeekFood:
                    Logic.StateActions.SeekingFood.SeekFood(_passedOrganism, _grid);


                    break;

                case PotentialStates.SeekMate:

                   // _simGrid.Move(gameTime);

                    break;

                default:

                    break;

            }




        }



    }
}
