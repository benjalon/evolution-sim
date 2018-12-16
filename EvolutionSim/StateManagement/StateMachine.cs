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
        private readonly State state;
        private readonly Grid grid;
        private readonly TimeManager timeManager;

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
        public void UpdateOrganismAttributes(Organism organism, TimeManager timeManager)
        {
            if (!timeManager.HasSimulationTicked)
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
                organism.Attributes.Hunger -= 0.001f;
                //organism.IncreaseHealth(1); // TODO: Maybe organisms should heal up over time?
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

            States organismState = organism.State;

            this.timeManager.UpdateOrganismTimers(organism);

            switch (organismState)
            {
                #region Neutral States

                case States.Roaming: // This is for when an organism is roaming randomly with no particular goal
                    if (organism.Attributes.Hunger < 0.8) // Hungry so find some food
                    {
                        organism.State = this.state.MoveState(organismState, Actions.HungryRoam); //then move into the seek food state
                    }
                    else if (organism.Attributes.Hunger >= 0.8 && this.timeManager.HasMatingCooldownExpired(organism)) // Not hungry so find a mate
                    {
                        organism.Path.Clear(); // TODO unneeeded?
                        organism.State = this.state.MoveState(organismState, Actions.HungryMate); //go find a mate
                    }

                    break;

                #endregion

                #region Food States

                case States.SeekFood: // When an organism is running a pathfinding algorithm to find food
                    if (organism.Attributes.Hunger >= 0.8)
                    {
                        organism.Path.Clear();
                        organism.State = this.state.MoveState(organismState, Actions.NotHungry);
                    }

                    if (organism.DestinationTile != null && organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.FoodDetected); // Food found, move towards it
                    }

                    break;

                case States.MovingToFood:
                    if (organism.Path.Count == 1 && organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.FoodFound); // adjacent to food, eat it
                    }

                    if (organism.DestinationTile == null || !organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.NotHungry); // Food is gone, give up
                    }

                    break;

                case States.Eating: //this code block handles logic when organism is the middle of eating
                    if (organism.DestinationTile == null || !organism.DestinationTile.HasFoodInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.NotHungry); // Food is gone, stop eating
                    }

                    break;

                #endregion

                #region Mate States

                case States.SeekMate: // When an organism is running pathfinding algorithm to find a mate
                    if (organism.DestinationTile != null && organism.DestinationTile.HasOrganismInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.MateFound); // Mate found, move towards them
                    }
                    else if (organism.WaitingForMate)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.Waiting); // A mate has found this organism, wait for them
                    }

                    if (organism.Attributes.Hunger < 0.4)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.HungryRoam); // hungry so stop looking for mate and go back to searching for food
                    }

                    break;

                case States.MovingToMate: // When an organism is moving on a path towards a mate
                    if (organism.DestinationTile != null && organism.DestinationTile.HasOrganismInhabitant && this.grid.IsAdjacent(organism.GridIndex, organism.Path[0].GridIndex))
                    {
                        organism.State = this.state.MoveState(organismState, Actions.Bang); //the organism is adjacent to a mate, so go ahead and make love
                    }

                    if (organism.DestinationTile == null || !organism.DestinationTile.HasOrganismInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.FinishedMating); // Mate is gone, give up
                    }

                    break;

                case States.WaitingForMate: // When an organism is waiting for their mate to approach them
                    if (!organism.WaitingForMate)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.Move); //mating is over, so go back into roaming.
                    }

                    // TODO: its possible to starve to death in this state, its also possible their mate gets killed on the way and never arrives

                    break;

                case States.Mating: // When an organism is mating
                    organism.State = this.state.MoveState(organismState, Actions.FinishedMating); // Tell this organism the mating is done
                    if (organism.DestinationTile.HasOrganismInhabitant)
                    {
                        ((Organism)(organism.DestinationTile.Inhabitant)).State = this.state.MoveState(organismState, Actions.FinishedMating); // Tell the mate the mating is done
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
            States organismState = organism.State;

            switch (organismState)
            {

                case States.Roaming:
                    if (this.timeManager.HasRoamingCooldownExpired(organism))
                    {
                        StateActions.Roam(organism, this.grid, this.timeManager);
                    }

                    break;

                case States.Eating:
                    StateActions.EatingFood.EatFood(organism, this.grid, this.timeManager);
                    break;

                case States.Mating:
                    var mother = (Organism)organism.DestinationTile.Inhabitant;
                    MatingOccurred?.Invoke(this, new MatingArgs(organism, mother));
                    mother.WaitingForMate = false;
                    
                    break;

                case States.SeekFood:

                    StateActions.SeekingFood.SeekFood(organism, this.grid, this.timeManager);

                    break;

                case States.MovingToMate:
                    StateActions.MoveAlongPath(organism, this.grid);

                    break;

                //when in seaking mate scan for an organism who is also in the "SeekMate" State
                case States.SeekMate:
                    StateActions.SeekingMate.SeekMate(organism, this.grid, this.timeManager);

                    break;

                case States.WaitingForMate:

                    StateActions.SeekingMate.WaitForMate(organism, this.grid);

                    break;

                case States.MovingToFood:
                    StateActions.MoveAlongPath(organism, this.grid);
                    break;

                default:

                    break;

            }
        }
    }
}
