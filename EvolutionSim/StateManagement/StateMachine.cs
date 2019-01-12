using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.TileGrid;
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
        
        public event EventHandler MatingOccurred;

      //  delegate bool checkPathPresent(Organism org);
       // checkPathPresent hasPath = organism => organism.Path.Count == 1;
        private const double MATING_THRESHOLD = 0.8;
        private const double HUNGREY_THRESHOLD = 0.4;


        public StateMachine()
        {
            this.state = new State();
        }

        /// <summary>
        /// call statemachine methods for each tick of the timer
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="timeManager"></param>
        public void UpdateStates(Grid grid, TimeManager timeManager)
        {
            Organism organism;
            for (var i = grid.Organisms.Count - 1; i >= 0; i--)
            {
                organism = grid.Organisms[i];
                CheckState(timeManager, organism);
                DetermineBehaviour(grid, timeManager, organism);
            }
        }

        /// <summary>
        /// This method is used for testing which state an organism is in, essentially on each tick of the simulation the 
        /// state variables are checked along with the organism attributes, when the organism falls into a certain
        /// boundary or a specific condition is true we call the lookup table and facilitate the state transition 
        /// if it is legal, otherwise an exception is thrown in the 'State' or lookup class
        /// </summary>
        /// <param name="organism"></param>
        public void CheckState(TimeManager timeManager, Organism organism)
        {
            //test the organisms current attributes
            //by switching on the current state


            States organismState = organism.State;

            timeManager.UpdateOrganismTimers(organism); //this is already called in the attribute updater

            switch (organismState)
            {

                #region Neutral States

                case States.Roaming: // This is for when an organism is roaming randomly with no particular goal
                    if (organism.Hunger < HUNGREY_THRESHOLD) // Hungry so find some food
                    {
                        organism.State = this.state.MoveState(organismState, Actions.HungryRoam); //then move into the seek food state
                    }
                    else if (organism.Hunger >= MATING_THRESHOLD && timeManager.HasMatingCooldownExpired(organism)) // Not hungry so find a mate
                    {
                        organism.Path.Clear(); // Need to get rid of any exisiting path an organism may have
                        organism.State = this.state.MoveState(organismState, Actions.HungryMate); //go find a mate
                    }

                    //otherwise if the organism isn't hungry, doesn't want to mate and is of type carnivore
                    else if(organism.Attributes.DietType == DietTypes.Canivore && timeManager.HasHuntingCooldownExpired(organism))
                    {

                        organism.State = this.state.MoveState(organismState, Actions.LookingForPrey);


                    }

                    break;

                #endregion
                    


                #region Food States

                case States.SeekFood: // When an organism is running a pathfinding algorithm to find food
                    if (organism.Hunger >= MATING_THRESHOLD)
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
                    //organisms have a path count greater than 1 a lot of the time.
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

                    if (organism.Hunger < HUNGREY_THRESHOLD)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.HungryRoam); // hungry so stop looking for mate and go back to searching for food
                    }

                    break;

                case States.MovingToMate: // When an organism is moving on a path towards a mate
                    if (organism.Path.Count == 1 && organism.DestinationTile.HasOrganismInhabitant)
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
                        ((Organism)(organism.DestinationTile.Inhabitant)).State = this.state.MoveState(organismState, Actions.FinishedMating); // Tell the mate that its mating is done
                    }

                    break;

                #endregion
                
                //these states are only avaliable to carnivores, the preyFound bool will be null if organisms are omnivores or herbivores
                #region Hunt States 

                case States.FindingPrey:
                    
                    //then move into the Hunting state
                    if ((bool)organism.PreyFound && organism.DestinationTile != null)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.FoundPrey);

                    }

                    if (timeManager.HasHuntingCooldownExpired(organism)) // if the cooldown has expired then give up looking
                    {
                        organism.State = this.state.MoveState(organismState, Actions.GiveUpLooking);

                    }
                     

                    break;


                // now here's the tricky bit, need to find an organism, create a location variable which updates with deltaT
                // then calculate and traverse along the path until either:
                // 1) we catch the prey
                // 2) the chase time expires
                // chasing speed should be based on the speed of the organism
                case States.Hunting:

                    //then we've found the target or time has expired so stop hunting
                    if(organism.Path.Count == 1 && organism.DestinationTile.HasOrganismInhabitant)
                    {
                        organism.State = this.state.MoveState(organismState, Actions.CaughtPrey);

                    }

                    else if (timeManager.HasHuntingCooldownExpired(organism)) // otherwise give the hunt up if time has expired
                    {

                        organism.State = this.state.MoveState(organismState, Actions.FinishedHunt);
                    }
                  

                
                    break;

                case States.KillingPrey:

                    //organism has finished hunting
                    organism.State = this.state.MoveState(organismState, Actions.FinishedHunt);

                    //set the prey found to be false
                    organism.PreyFound = false;

                    break;

                default:
                    break;
            }

        }
             #endregion


        /// <summary>
        /// This method controls how an organism goes about its buisness when in a given state
        /// it does this through a combonation of state variables and cooldowns
        /// </summary>
        private void DetermineBehaviour(Grid grid, TimeManager timeManager, Organism organism)
        {
            States organismState = organism.State;

            switch (organismState)
            {

                case States.Roaming:
                    StateActions.Roam(organism, grid, timeManager);

                    break;

                case States.Eating:
                    StateActions.EatingFood.EatFood(organism, grid, timeManager);
                    break;

                case States.Mating:
                    var mother = (Organism)organism.DestinationTile.Inhabitant;

                    MatingOccurred?.Invoke(this, new MatingArgs(organism, mother));

                    organism.MsSinceLastMate = 0;
                    mother.MsSinceLastMate = 0;
                    mother.WaitingForMate = false;
                    
                    break;

                case States.SeekFood:

                    StateActions.SeekingFood.SeekFood(organism, grid, timeManager);

                    break;

                case States.MovingToMate:
                    StateActions.MoveAlongMatePath(organism, grid);

                    break;

                //when in seaking mate scan for an organism who is also in the "SeekMate" State
                case States.SeekMate:
                    StateActions.SeekingMate.SeekMate(organism, grid, timeManager);

                    break;

                case States.WaitingForMate:

                    StateActions.SeekingMate.WaitForMate(organism, grid);

                    break;

                case States.MovingToFood:
                    StateActions.MoveAlongFoodPath(organism, grid);
                    break;


                case States.FindingPrey:

                    StateActions.SeekingFood.SeekFood(organism, grid, timeManager);

                    //should be called once as the organism will move out of state as soon as the
                    //destination tile isn't null
                    //this will only work if this is checked once before moving onto the following state
                    //state upon the next call of checkState
                    if(organism.DestinationTile != null && organism.PreyFound == true)
                    {
                        grid.addOrganismBeingHunted((Organism)organism.DestinationTile.Inhabitant);

                    }

                    break;

                    //this is where all the pathfinding will go, 
                    //each tick of the system will re-calculate 
                    //the destination tile and the path to the destination tile
                    //with simplified path finding
                case States.Hunting: 

                

                    break;


                case States.KillingPrey: //prey has been caught, so kill it

                    var prey = (Organism)organism.DestinationTile.Inhabitant;
                    prey.killOrganism();

                    break;


                default:

                    break;

            }
        }
    }
}
