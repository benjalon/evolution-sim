using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.Pathfinding;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading;

namespace EvolutionSim.StateManagement
{
    enum Directions
    {
        Up,
        Left,
        Down,
        Right
    }

    public static class StateActions
    {

        delegate bool checkPathPresent(Organism org);
        static checkPathPresent hasPath = organism => organism.Path.Count > 0;


        public static List<Point> GetPointsInRange(Organism organism)
        {
            List<Point> toRet = new List<Point>();

            var firstX = organism.GridIndex.X - Organism.DETECTION_RADIUS;
            var firstY = organism.GridIndex.Y - Organism.DETECTION_RADIUS;

            for (int i = 0; i < Organism.DETECTION_DIAMETER; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    toRet.Add(new Point(firstX + i, firstY + j));
                }

            }
            return toRet;

        }

        public static void Roam(Organism organism, Grid grid, TimeManager timeManager)
        {
            // If we don't have a set destination, pick a random tile to explore
            if (timeManager.HasRoamingCooldownExpired(organism))
            {
                organism.Path = PathFinding.FindShortestPath(grid.GetTileAt(organism), grid.FindRandomNearbyEmptyTile(organism), grid);
                organism.Computing = false;
            }
            else if (organism.DestinationTile != null)
            {
                MoveAlongRoamPath(organism, grid);
            }
        }

        // Returns true if reached tile, false if not.
        private static bool MoveTowards(Organism organism, Tile destination, Grid grid)
        {
            if (organism.Rectangle.X == destination.ScreenPositionX && organism.Rectangle.Y == destination.ScreenPositionY)
            {
                grid.ReparentOrganism(organism, destination.GridIndex.X, destination.GridIndex.Y);
                return true;
            }

            var lerp = new Lerper();
            var newX = (int)lerp.Lerp(organism.Rectangle.X, destination.ScreenPositionX);
            var newY = (int)lerp.Lerp(organism.Rectangle.Y, destination.ScreenPositionY);
            organism.SetScreenPosition(newX, newY);

            return false;
        }

        public static void MoveAlongRoamPath(Organism organism, Grid grid)
        {
            var isPathBlocked = organism.Path.Count > 0 && organism.Path[0].HasInhabitant;
            if (isPathBlocked)
            {
                organism.Path.Clear(); // The path is blocked so it will need recalculating
            }
            else if (MoveTowards(organism, organism.Path[0], grid)) // Wait for the lerp
            {
                organism.Path.RemoveAt(0);
            }
        }

        public static void MoveAlongFoodPath(Organism organism, Grid grid)
        {
            var isPathBlocked = organism.Path.Count > 1 && organism.Path[0].HasInhabitant ||
                                organism.Path.Count == 1 && organism.Path[0].HasOrganismInhabitant; // If the path has only one tile, it should only contain the target food

            if (isPathBlocked)
            {
                organism.Path.Clear(); // The path is blocked so it will need recalculating
            }

            // path is not blocked so carry on as normal
            else if (organism.Path.Count > 0)
            {
                if (MoveTowards(organism, organism.Path[0], grid)) // Wait for the lerp
                {
                    organism.Path.RemoveAt(0);
                }
            } 
        }

        public static void MoveAlongMatePath(Organism organism, Grid grid)
        {
            var isPathBlocked = organism.Path.Count > 1 && organism.Path[0].HasInhabitant || 
                                organism.Path.Count == 1 && organism.Path[0].HasFoodInhabitant; // If the path has only one tile, it should only contain the target organism

            if (isPathBlocked)
            {
                organism.Path.Clear(); // The path is blocked so it will need recalculating
            }
            else if (hasPath(organism))
            {
                if (MoveTowards(organism, organism.Path[0], grid)) // Wait for the lerp
                {
                    organism.Path.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// This check will be used in the seek food to determine if the food source is valid at the searched destination
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="firstX"></param>
        /// <param name="firstY"></param>
        /// <param name="grid"></param>
        /// <param name="organism"></param>
        /// <returns></returns>
        private static bool PerformValidFoodCheck(int x, int y, int firstX, int firstY, Organism organism, Grid grid)
        {
            if (!(grid.InBounds(x, y) && grid.IsFoodAt(firstX, firstY)))
            {
                return false; // The tile is out of bounds or there's no food there
            }

            // Does the food type match the organism's diet type
            var food = grid.GetTileAt(firstX, firstY).Inhabitant as Food;
            var validFood = organism.Attributes.DietType == DietTypes.Omnivore ||
                            (organism.Attributes.DietType == DietTypes.Herbivore && food.IsHerbivoreFood) ||
                            (organism.Attributes.DietType == DietTypes.Canivore && !food.IsHerbivoreFood);

            return validFood;
        }


        public static class SeekingFood
        {
            public static void SeekFood(Organism organism, Grid grid, TimeManager timeManager)
            {
                if (organism.Computing)
                {
                    return; // If the path to food has been computed, there's no need to do it again
                }

                // If we're not moving on a path, but we're in the state seeking food, then we haven't yet found any food.
                Tile potentialFood = FoodInRange(organism, grid);

                if (potentialFood != null)
                {
                    // Path to food
                    organism.Computing = true;

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(Eek));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object state)
                    {
                        organism.Path = PathFinding.FindShortestPath(grid.GetTileAt(organism), potentialFood, grid);
                        organism.Computing = false;
                    }), null);
                }
                else
                {
                    Roam(organism, grid, timeManager);
                }

            }

            /// <summary>
            /// This method handles the spiral search method for organims when they are in searching for food
            /// </summary>
            /// <param name="organism"></param>
            /// <param name="grid"></param>
            /// <returns></returns>
            public static Tile FoodInRange(Organism organism, Grid grid)
            {
                States organismState = organism.State;
                var max_depth = Organism.DETECTION_RADIUS;
                var depth = 0;

                int firstX;
                int firstY;
                int num;
                int firstCheck;
                int i;
                int j;
                int x;
                int y;

                //while not all the surronding tiles have been searched within the specified range
                while (depth < max_depth)
                {
                    //the starting is the depth away from the origin +1 to compensate for the 0-2;
                    firstX = organism.GridIndex.X - (depth + 1);
                    firstY = organism.GridIndex.Y - (depth + 1);

                    num = 3 + (2 * depth); //number of tiles to check per depth level. (the number of extra tiles to search is calculated by doubling the current depth)
                    firstCheck = 1 - depth;
                    i = -1;
                    j = 0;

                    //first access the tiles above the organism with an offset of 1.
                    while (i < num - 1)
                    {
                        i++;
                        x = firstX + i;
                        y = firstY + j;
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), organism, grid))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    //now the tiles adjacent to the organism to the right
                    while (j < num - 1)
                    {
                        j++;
                        x = firstX + i;
                        y = firstY + j;
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), organism, grid))
                        {

                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    //now go back on oneself until we are parralel with the starting position.
                    while (i > 0)
                    {
                        i--;
                        x = firstX + i;
                        y = firstY + j;
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), organism, grid))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    //now traverse up the reamining tiles to finish back at the starting position
                    while (j > 0)
                    {
                        j--;
                        x = firstX + i;
                        y = firstY + j;
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), organism, grid))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }


                    depth++;
                }
                return null;
            }
        }

        /// <summary>
        /// This class should now be checking an organim's food preference
        /// </summary>
        public static class EatingFood
        {
            public static void EatFood(Organism organism, Grid grid, TimeManager timeManager)
            {
                var food = organism.DestinationTile.Inhabitant as Food;

                if (timeManager.HasSimulationTicked) // It's rare but two organisms can attempt to eat the same food source and the type preference is indifferent 
                {
                    food.BeEaten();
                    organism.Eat(); //organism gets fuller after eating
                }
            }

        }
        public static class SeekingMate
        {
            public static void SeekMate(Organism organism, Grid grid, TimeManager timeManager)
            {
                Tile potentialMate = MatesInRange(organism, grid);

                if (potentialMate != null)
                {
                    //ping the potential mate in the given position and get them to move into the waitingForMateState.
                    ((Organism)potentialMate.Inhabitant).WaitingForMate = true;


                    //shouldn't be calling the A* for mating probably
                    organism.Path = PathFinding.FindShortestPath(grid.GetTileAt(organism), potentialMate, grid);
                    organism.Computing = false;
                }
                //this check wont work. Organisms have no way of entering the waiting for mate state
                else if (!organism.WaitingForMate)
                {
                    Roam(organism, grid, timeManager);
                }

            }

            private static Tile MatesInRange(Organism organism, Grid grid)
            {
                int firstX = organism.GridIndex.X - Organism.DETECTION_RADIUS;
                int firstY = organism.GridIndex.Y - Organism.DETECTION_RADIUS;
                for (int i = 0; i < Organism.DETECTION_DIAMETER; i++)
                {
                    for (int j = 0; j < Organism.DETECTION_DIAMETER; j++)
                    {
                        if (grid.InBounds(firstX + i, firstY + j) && grid.IsMateAt(organism, firstX + i, firstY + j))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                }
                return null;
            }

            //this handles the logic for when an organism is waiting for a mate
            public static void WaitForMate(Organism organism, Grid grid)
            {
                //System.Console.WriteLine("Waiting!"); // TODO: Have a way of leaving this state after some time if a mate hasn't reached them
            }
        }
    }
}