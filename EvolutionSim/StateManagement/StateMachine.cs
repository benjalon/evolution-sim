using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.Utility;
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
        private TimeManager timeManager;

        // Grid _simGrid;
        public event EventHandler MatingOccurred;

        public StateMachine(Grid grid, TimeManager timeManager)
        {
            this.grid = grid;
            this.timeManager = timeManager;

            this.state = new State();
        }

        /// <summary>
        /// as time marches on make the organism hungrey
        /// </summary>
        /// <param name="organism"></param>
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
        /// <param name="organism"></param>
        public void checkState(Organism organism)
        {
            //test the organisms current attributes
            //by switching on the current state

            PotentialStates organismState = organism.OrganismState;


            switch (organismState)
            {

                case PotentialStates.Roaming:

                    organism.MilliSecondsSinceLastMate += TimeManager.DELTA_MS;

                    // the organisms hunger is low then go and seek food.
                    if (organism.attributes.Hunger < 0.8)
                    {

                        //then move into the seek food state
                        organism.OrganismState = this.state.MoveState(organismState, Action.HungryRoam);


                    }

                    
                    else if (organism.attributes.Hunger >= 0.8 && this.timeManager.MatingCooldownExpired(organism.MilliSecondsSinceLastMate))
                    {
                        organism.MilliSecondsSinceLastMate = 0;

                        //sometimes an organism will 
                        organism.MovingOnPath = false;
                        //go find a mate
                        organism.OrganismState = this.state.MoveState(organismState, Action.HungryMate);


                    }



                    break;

                //this code block handles logic when organism is the middle of eating
                case PotentialStates.Eating:
                    if (organism.DestinationTile == null)
                    {
                        organism.OrganismState = this.state.MoveState(organismState, Action.NotHungry);

                    }
                    else if (!this.grid.IsAdjacent(organism.GridIndex, organism.DestinationTile.GridIndex))
                    {
                        // Change NotHungry?
                        organism.OrganismState = this.state.MoveState(organismState, Action.NotHungry);
                    }
                    //if there is food in food source then contuine eating

                    //if food hungry is maxed out (100) then stop eating && move back into roaming


                    break;

                //organism needs way of tracking other organisms of the same species
                //This needs revision.
                //we have an error where an organism will transition state but the "Moving Along path value is set to true"
                case PotentialStates.SeekMate:

                    if (organism.MovingOnPath)
                    {
                        organism.OrganismState = this.state.MoveState(organismState, Action.MateFound);
                    }

                    else if (organism.attributes.WaitingForMate)
                    {
                        //then the organism has been pinged by the organism and will wait to get fkt
                        organism.OrganismState = this.state.MoveState(organismState, Action.Waiting);

                    }

                    if(organism.attributes.Hunger < 0.4) // then stop searching for a mate and go back to searching for food
                    {

                        organism.OrganismState = this.state.MoveState(organismState, Action.HungryRoam);

                    }


                    break;


                //check if an organism should be moving to "Moving to Mate"
                case PotentialStates.MovingToMate:
                    if (organism.DestinationTile != null && this.grid.IsAdjacent(organism.GridIndex, organism.DestinationTile.GridIndex))
                    {
                        //the organism is adjacent to mate, so go ahead and make love
                        organism.OrganismState = this.state.MoveState(organismState, Action.Bang);


                    }



                    break;

                case PotentialStates.WaitingForMate:
                    if (!organism.attributes.WaitingForMate)
                    {

                        //mating is over, so go back into roaming.
                        organism.OrganismState = this.state.MoveState(organismState, Action.Move);


                    }


                    break;


                case PotentialStates.SeekFood:

                    if (organism.MovingOnPath)
                    {
                        organism.OrganismState = this.state.MoveState(organismState, Action.FoodDetected);
                    }



                    break;
                case PotentialStates.MovingToFood:
                    if (organism.DestinationTile != null && this.grid.IsAdjacent(organism.GridIndex, organism.DestinationTile.GridIndex))
                    {
                        organism.OrganismState = this.state.MoveState(organismState, Action.FoodFound);

                    }
                    if (!organism.MovingOnPath)
                    {
                        organism.OrganismState = this.state.MoveState(organismState, Action.FoodFound);

                    }


                    break;

                //once an organism has begun mating it cannont stop or change state
                //once a certain time has elasped move back to roaming

                case PotentialStates.Mating:

                    organism.OrganismState = this.state.MoveState(organismState, Action.FinishedMating);

                    //this line throws a null pointer in the senario where an organism moves over to anther organism's destination 
                    //but it has moved away because because it didn't detect the organism coming over to mate with it
                    //I'm not 100% sure how to fix this yet.
                    if (organism.DestinationTile.HasOrganismInhabitant())
                    {
                        ((Organism)(organism.DestinationTile.Inhabitant)).OrganismState = this.state.MoveState(organismState, Action.FinishedMating);

                        //Sets the oganism back to not waiting for a mate
                        //((Organism)(organism.DestinationTile.Inhabitant)).pingFinished();
                        //MatingOccurred?.Invoke(this, new MatingArgs(organism));
                    }
                    //grid.AttemptToPositionAt(grid.AddOrganism, _passedOrganism.GridPosition.X, _passedOrganism.GridPosition.Y + 1);


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
                    StateActions.Roam(_passedOrganism, this.grid, this.timeManager);

                    break;

                case PotentialStates.Eating:
                    StateActions.EatingFood.EatFood(_passedOrganism, this.grid, this.timeManager);
                    break;

                case PotentialStates.Mating:

                    ((Organism)(_passedOrganism.DestinationTile.Inhabitant)).PingFinished();
                    MatingOccurred?.Invoke(this, new MatingArgs(_passedOrganism));

                    break;

                case PotentialStates.SeekFood:

                    StateActions.SeekingFood.SeekFood(_passedOrganism, this.grid, this.timeManager);

                    break;

                case PotentialStates.MovingToMate:
                    StateActions.MoveAlongPath(_passedOrganism, this.grid, this.timeManager, _passedOrganism.Path);

                    break;

                //when in seaking mate scan for an organism who is also in the "SeekMate" State
                case PotentialStates.SeekMate:

                    StateActions.SeekingMate.SeekMate(_passedOrganism, this.grid, this.timeManager);

                    break;


                case PotentialStates.WaitingForMate:

                    StateActions.SeekingMate.WaitForMate(_passedOrganism, this.grid);

                    break;



                case PotentialStates.MovingToFood:
                    StateActions.MoveAlongPath(_passedOrganism, this.grid, this.timeManager, _passedOrganism.Path);
                    break;

                default:

                    break;

            }




        }


    }
}
