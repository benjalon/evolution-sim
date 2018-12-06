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
            if (!TimeManager.HAS_SIMULATION_TICKED)
            {
                return; // Wait a bit
            }

            organism.Attributes.Age += 1;
            
            if (organism.Attributes.Age > 1000)
            {
                organism.DecreaseHealth(999);
            }
            else if (organism.Attributes.Hunger > 0)
            {
                organism.Attributes.Hunger -= 0.001;
                organism.IncreaseHealth(1);
            }
            else
            {
                organism.Attributes.Hunger = 0;
                organism.DecreaseHealth(1);
            }



        }

        /// <summary>
        /// This method is used for testing which state an organism is in, should be called in the update method
        /// </summary>
        /// <param name="organism"></param>
        public void CheckState(Organism organism)
        {
            //test the organisms current attributes
            //by switching on the current state

            PotentialStates organismState = organism.State;

            this.timeManager.UpdateOrganismTimers(organism);

            switch (organismState)
            {
                #region Neutral States

                case PotentialStates.Roaming: // This is for when an organism is roaming randomly with no particular goal
                    if (organism.Attributes.Hunger < 0.8) // Hungry so find some food
                    {
                        organism.State = this.state.MoveState(organismState, Action.HungryRoam); //then move into the seek food state
                    }
                    else if (organism.Attributes.Hunger >= 0.8) // Not hungry so find a mate
                    {
                        organism.DestinationTile = null;
                        organism.State = this.state.MoveState(organismState, Action.HungryMate); //go find a mate
                    }

                    break;

                #endregion

                #region Food States

                case PotentialStates.SeekFood: // When an organism is running a pathfinding algorithm to find food
                    if (organism.DestinationTile != null && organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Action.FoodDetected); // Food found, move towards it
                    }

                    break;

                case PotentialStates.MovingToFood:
                    if (organism.DestinationTile != null && organism.DestinationTile.HasFoodInhabitant && this.grid.IsAdjacent(organism.GridIndex, organism.DestinationTile.GridIndex))
                    {
                        organism.State = this.state.MoveState(organismState, Action.FoodFound); // adjacent to food, eat it
                    }

                    if (organism.DestinationTile == null || !organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Action.NotHungry); // Food is gone, give up
                    }

                    break;

                case PotentialStates.Eating: //this code block handles logic when organism is the middle of eating
                    if (organism.DestinationTile == null || !organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Action.NotHungry); // Food is gone, stop eating
                    }

                    break;

                #endregion

                #region Mate States

                case PotentialStates.SeekMate: // When an organism is running pathfinding algorithm to find a mate
                    if (organism.DestinationTile != null && organism.DestinationTile.HasOrganismInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Action.MateFound); // Mate found, move towards them
                    }
                    else if (organism.Attributes.WaitingForMate)
                    {
                        organism.State = this.state.MoveState(organismState, Action.Waiting); // A mate has found this organism, wait for them
                    }

                    if (organism.Attributes.Hunger < 0.4)
                    {
                        organism.State = this.state.MoveState(organismState, Action.HungryRoam); // hungry so stop looking for mate and go back to searching for food
                    }

                    break;
                    
                case PotentialStates.MovingToMate: // When an organism is moving on a path towards a mate
                    if (organism.DestinationTile != null && organism.DestinationTile.HasOrganismInhabitant && this.grid.IsAdjacent(organism.GridIndex, organism.DestinationTile.GridIndex))
                    {
                        organism.State = this.state.MoveState(organismState, Action.Bang); //the organism is adjacent to a mate, so go ahead and make love
                    }

                    if (organism.DestinationTile == null || !organism.DestinationTile.HasOrganismInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Action.FinishedMating); // Mate is gone, give up
                    }

                    break;

                case PotentialStates.WaitingForMate: // When an organism is waiting for their mate to approach them
                    if (!organism.Attributes.WaitingForMate)
                    {
                        organism.State = this.state.MoveState(organismState, Action.Move); //mating is over, so go back into roaming.
                    }

                    // TODO: its possible to starve to death in this state, its also possible their mate gets killed on the way and never arrives

                    break;

                case PotentialStates.Mating: // When an organism is mating
                    organism.State = this.state.MoveState(organismState, Action.FinishedMating); // Tell this organism the mating is done
                    if (organism.DestinationTile.HasOrganismInhabitant)
                    {
                        ((Organism)(organism.DestinationTile.Inhabitant)).State = this.state.MoveState(organismState, Action.FinishedMating); // Tell the mate the mating is done
                    }

                    break;

                #endregion

                default:
                    break;
            }

        }

        /// <summary>
        /// This method controls how an organism goes about its buisness when in a given state
        /// </summary>

        public void DetermineBehaviour(Organism organism)
        {
            PotentialStates organismState = organism.State;

            switch (organismState)
            {

                case PotentialStates.Roaming:
                    StateActions.Roam(organism, this.grid);

                    break;

                case PotentialStates.Eating:
                    StateActions.EatingFood.EatFood(organism, this.grid);
                    break;

                case PotentialStates.Mating:

                    ((Organism)(organism.DestinationTile.Inhabitant)).Attributes.WaitingForMate = false;

                    if (TimeManager.HAS_SIMULATION_TICKED)
                    {
                        MatingOccurred?.Invoke(this, new MatingArgs(organism));
                    }

                    break;

                case PotentialStates.SeekFood:

                    StateActions.SeekingFood.SeekFood(organism, this.grid);

                    break;

                case PotentialStates.MovingToMate:
                    StateActions.MoveAlongPath(organism, this.grid, organism.Path);

                    break;

                //when in seaking mate scan for an organism who is also in the "SeekMate" State
                case PotentialStates.SeekMate:

                    StateActions.SeekingMate.SeekMate(organism, this.grid);

                    break;
                    
                case PotentialStates.WaitingForMate:

                    StateActions.SeekingMate.WaitForMate(organism, this.grid);

                    break;

                case PotentialStates.MovingToFood:
                    StateActions.MoveAlongPath(organism, this.grid, organism.Path);
                    break;

                default:

                    break;

            }
        }
    }
}
