using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using System;

namespace EvolutionSim.StateManagement
{
    //State Machine Class
    //this class is used for describing the conditions for which a change in state occurs and modeling organism behaviour
    public class StateMachine
    {
        //initalise lookup table
        private State state;
        private Grid grid;

        // Grid _simGrid;

        public StateMachine(Grid grid)
        {
            this.grid = grid;
            this.state = new State();
        }
     
        public void UpdateOrganismAttributes(Organism organism)
        {

            //hardcode a value that doesn't go down too fast
            if (organism.attributes.Hunger > 0)
            {
                organism.attributes.Hunger -= 0.0001;
            }
            else
            {
                organism.attributes.Hunger = 0;
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

            PotentialStates organismState = _passedOrganism.OrganismState;


            switch (organismState)
            {

                case PotentialStates.Roaming:

                    //if food < 50 then go to seek food, contuine to seek food until back to 80% full?
                    //diferent food sources have different values

                    if (_passedOrganism.attributes.Hunger < 0.4)
                    {

                        //then move into the seek food state
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.HungryRoam);


                    }

                    else if (_passedOrganism.attributes.Hunger >= 0.8)
                    {
                        //go find a mate
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.HungryMate);


                    }


                    break;

                //this code block handles logic when organism is the middle of eating
                case PotentialStates.Eating:
                    if(_passedOrganism.DestinationTile == null)
                    {
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.NotHungry);
                        
                    }
                    else if (!StateActions.AdjacencyCheck(_passedOrganism.GridPosition, _passedOrganism.DestinationTile.GridPosition)){
                        // Change NotHungry?
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.NotHungry);
                    }
                    //if there is food in food source then contuine eating

                    //if food hungry is maxed out (100) then stop eating && move back into roaming


                    break;

                //organism needs way of tracking other organisms of the same species
                //
                case PotentialStates.SeekMate:

                        if (_passedOrganism.MovingOnPath)
                        {
                            _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.MateFound);
                        }
                 

                    break;

                
                //check if an organism should be moving to "Moving to Mate"
                case PotentialStates.MovingToMate:
                    if (_passedOrganism.DestinationTile != null && StateActions.AdjacencyCheck(_passedOrganism.GridPosition, _passedOrganism.DestinationTile.GridPosition))
                    {
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.Bang);


                    }



                    break;
                
                case PotentialStates.WaitingForMate:



                    break;


                case PotentialStates.SeekFood:

                   if( _passedOrganism.MovingOnPath)
                    {
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.FoodDetected);
                    }



                    break;
                case PotentialStates.MovingToFood:
                    if (_passedOrganism.DestinationTile != null && StateActions.AdjacencyCheck(_passedOrganism.GridPosition, _passedOrganism.DestinationTile.GridPosition))
                    {
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.FoodFound);

                    }
                    if (!_passedOrganism.MovingOnPath)
                    {
                        _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.FoodFound);

                    }


                    break;

                //once an organism has begun mating it cannont stop or change state
                //once a certain time has elasped move back to roaming

                case PotentialStates.Mating:
                    Console.WriteLine("Success");
                    _passedOrganism.OrganismState = this.state.MoveState(organismState, Action.FinishedMating);
                    ((Organism)(_passedOrganism.DestinationTile.Inhabitant)).OrganismState = this.state.MoveState(organismState, Action.FinishedMating);

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

            PotentialStates organismState = _passedOrganism.OrganismState;


            switch (organismState)
            {

                case PotentialStates.Roaming:
                    StateActions.Roam(_passedOrganism, this.grid);

                    break;

                case PotentialStates.Eating:
                    StateActions.EatingFood.EatFood(_passedOrganism, this.grid);
                    break;
                case PotentialStates.Mating:

                    break;

                case PotentialStates.SeekFood:
                    StateActions.SeekingFood.SeekFood(_passedOrganism, this.grid);

                    break;

                case PotentialStates.MovingToMate:
                    StateActions.MoveAlongPath(_passedOrganism, this.grid, _passedOrganism.Path);

                    break;

                case PotentialStates.SeekMate:

                        StateActions.SeekingMate.SeekMate(_passedOrganism, this.grid);

                    break;

                case PotentialStates.WaitingForMate:

                    System.Console.WriteLine("Waiting!");
                    break;
                   // _simGrid.Move(gameTime);

                    
                case PotentialStates.MovingToFood:
                    StateActions.MoveAlongPath(_passedOrganism, this.grid, _passedOrganism.Path);
                    break;

                default:

                    break;

            }




        }



    }
}
