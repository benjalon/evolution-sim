using EvolutionSim.Pathfinding;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static int Distance(Point StartPosition, Point EndPosition)
        {
            return Math.Abs(StartPosition.X - EndPosition.X) + Math.Abs(EndPosition.Y - EndPosition.Y);
        }

        public static Boolean AdjacencyCheck(Point StartPosition, Point EndPosition)
        {
            double distance = Math.Floor(Math.Sqrt((StartPosition.X - EndPosition.X) * (StartPosition.X - EndPosition.X) + (StartPosition.Y - EndPosition.Y) * (StartPosition.Y - EndPosition.Y)));
            return distance == 1;
        }

        public static List<Point> GetPointsInRange(Organism organism)
        {
            List<Point> toRet = new List<Point>();

            var firstX = organism.GridIndex.X - organism.attributes.DetectionRadius;
            var firstY = organism.GridIndex.Y - organism.attributes.DetectionRadius;

            for (int i = 0; i < organism.attributes.DetectionDiameter; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    toRet.Add(new Point(firstX + i, firstY + j));

                }

            }
            return toRet;

        }

        private static Random _random = new Random();

        public static void Roam(Organism organism, Grid grid)
        {
            Boolean FoundFreeTile = false;
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME.Milliseconds;


            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {   //decide destination

                if (organism.DestinationTile is null)
                {
                    int _destinationTileX = organism.GridIndex.X;
                    int _destinationTileY = organism.GridIndex.Y;
                    organism.MilliSecondsSinceLastMovement = 0;
                    while (!FoundFreeTile)
                    {
                        Directions _num = (Directions)_random.Next(0, 4);


                        switch (_num)
                        {
                            case Directions.Up:
                                if (_destinationTileY > 0)
                                {
                                    _destinationTileY -= 1;
                                }
                                break;
                            case Directions.Left:
                                if (_destinationTileX > 0)
                                {
                                    _destinationTileX -= 1;
                                }
                                break;
                            case Directions.Down:
                                if (_destinationTileY < Grid.TileCountY - 1)
                                {
                                    _destinationTileY += 1;
                                }
                                break;
                            case Directions.Right:
                                if (_destinationTileX < Grid.TileCountX - 1)
                                {
                                    _destinationTileX += 1;
                                }
                                break;
                        }
                        if(!grid.GetTileAt(_destinationTileX, _destinationTileY).HasInhabitant())
                        {
                            organism.DestinationTile = grid.GetTileAt(_destinationTileX, _destinationTileY);
                            //grid.ReparentOrganism(organism, organism.DestinationTile.GridIndex.X, organism.DestinationTile.GridIndex.Y);
                            FoundFreeTile = true;

                        }
                    }
     
                }
                else
                {
                    if (organism.Rectangle.X == organism.DestinationTile.ScreenPositionX && organism.Rectangle.Y == organism.DestinationTile.ScreenPositionY)
                    {
                        grid.ReparentOrganism(organism, organism.DestinationTile.GridIndex.X, organism.DestinationTile.GridIndex.Y);
                        organism.DestinationTile = null;
                        //organism.MilliSecondsSinceLastMovement = 0;

                    }
                    else
                    {
                        //MoveTowardsTile
                        Lerper lerp = new Lerper();


                        var newX = (int)lerp.Lerp(organism.Rectangle.X, organism.DestinationTile.ScreenPositionX);
                        var newY = (int)lerp.Lerp(organism.Rectangle.Y, organism.DestinationTile.ScreenPositionY);
                        organism.SetScreenPosition(newX, newY);


                    }

                }

                // organism.moveTowardstile

                //grid.MoveOrganism(organism, _destinationTileX, _destinationTileY);
            }


        }
        // Returns true if reached tile, false if not.
        private static bool Move(Organism organism,Tile Destination,Grid grid){
            if (organism.Rectangle.X == Destination.ScreenPositionX && organism.Rectangle.Y == Destination.ScreenPositionY)
            {
                grid.ReparentOrganism(organism, Destination.GridIndex.X, Destination.GridIndex.Y);
                //organism.DestinationTile = null;

                return true;
            }
            else
            {
                //MoveTowardsTile
                Lerper lerp = new Lerper();


                var newX = (int)lerp.Lerp(organism.Rectangle.X, Destination.ScreenPositionX);
                var newY = (int)lerp.Lerp(organism.Rectangle.Y, Destination.ScreenPositionY);
                organism.SetScreenPosition(newX, newY);

                return false;
            }

        }



        public static void MoveAlongPath(Organism organism, Grid grid, List<Tile> Path)
        {
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME.Milliseconds;

            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {

                if (Path.Any() && !Path.First().HasInhabitant())
                {
                    if(Move(organism,Path.ElementAt(0), grid))
                    {
                        Path.RemoveAt(0);
                    }
                }
            }

            if (!Path.Any())
            {
                    organism.MovingOnPath = false;
            }


        }

        public static class SeekingFood
        {
            public static void SeekFood(Organism organism, Grid grid)
            {

                // Essentially, if food has been located, and path calculated, we move towards food
 
                // If we're not moving on a path, but we're in the state seeking food, then we haven't yet found any food.
                Tile potentialFood = FoodInRange(organism, grid);

                if (potentialFood != null)
                {
                    // Path to food
                    List<Tile> Path = PathFinding.FindShortestPath(grid.GetTileAt(organism), potentialFood, grid);
                    organism.Path = Path;
                    if(Path.Count == 0)
                    {
                        organism.DestinationTile = potentialFood;

                    }
                    else
                    {
                        organism.DestinationTile = potentialFood;
                        //organism.Path.RemoveAt(organism.Path.Count - 1);

                    }
                    organism.MovingOnPath = true;
                    
                }
                else
                {
                    Roam(organism, grid);
                    
                }
                

                //if destination full decide again.
            }

            /// <summary>
            /// This method handles the spiral search method for organims when they are in searching for food
            /// </summary>
            /// <param name="organism"></param>
            /// <param name="grid"></param>
            /// <returns></returns>
            private static Tile FoodInRange(Organism organism, Grid grid)
            {

                // Defines the range of surronding tiles the organism can search
                var max_depth = organism.attributes.DetectionRadius;
                var depth = 0;

                //bool herbivore = (food.herbivoreFriendly && food != null);

             
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
                        if (PerformValidFoodCheck(x, y, (firstX+i), (firstY+j), grid))
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
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), grid))
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
                        if (PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), grid))
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
                        if(PerformValidFoodCheck(x, y, (firstX + i), (firstY + j), grid))
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
        /// This check will be used in the seek food to determine if the food source is valid at the searched destination
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="firstX"></param>
        /// <param name="firstY"></param>
        /// <param name="grid"></param>
        /// <param name="organism"></param>
        /// <returns></returns>
        private static bool PerformValidFoodCheck(int x, int y, int firstX, int firstY, Grid grid)
        {
           
            return (Grid.InBounds(x, y) && grid.IsFoodAt(firstX, firstY));

        }


        /// <summary>
        /// This class should now be checking an organim's food preference
        /// </summary>
        public static class EatingFood
        {

            public static void EatFood(Organism organism, Grid grid)
            {
                bool validFood;

                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME.Milliseconds;
                if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
                {
                    organism.MilliSecondsSinceLastMovement = 0;

                   Food food = organism.DestinationTile.Inhabitant as Food;
                    //this combines two checks

                    //this check determines if the organism can eat the current food source
                    validFood = organism.OrganismPref == Organism.FoodType.Omnivore || organism.OrganismPref == Organism.FoodType.Herbivore;
                  
                    
                    if (food != null && validFood && food.HerbivoreFriendly) // It's rare but two organisms can attempt to eat the same food source and the type preference is indifferent 

                    {
                        food.Eat();
                        // organism._attributes._hunger += 0.3;
                    }

                    organism.DestinationTile = null;
                    organism.Path.Clear();
                }
            }
        }
        public static class SeekingMate
        {
            
            public static void SeekMate(Organism organism, Grid grid)
            {
                
                Tile potentialMate = MatesInRange(organism, grid);

                if (potentialMate != null)
                {
                    //ping the potential mate in the given position and get them to move into the waitingForMateState.
                    ((Organism)potentialMate.Inhabitant).PingMate();


                    //shouldn't be calling the A* for mating probably
                    List<Tile> Path = PathFinding.FindShortestPath(grid.GetTileAt(organism), potentialMate, grid);

                    organism.Path = Path;
                    if (Path.Count == 0)
                    {
                        organism.DestinationTile = potentialMate;
                    }
                    else
                    {
                        organism.DestinationTile = potentialMate;
                        organism.Path.RemoveAt(organism.Path.Count - 1);
                    }

                    organism.MovingOnPath = true;
                    
                }
                //this check wont work. Organisms have no way of entering the waiting for mate state
                else if (!organism.attributes.WaitingForMate)
                {
                    Roam(organism, grid);
                }
                
            }

            private static Tile MatesInRange(Organism organism, Grid grid)
            {
                int firstX = organism.GridIndex.X - organism.attributes.DetectionRadius;
                int firstY = organism.GridIndex.Y - organism.attributes.DetectionRadius;
                for (int i = 0; i < organism.attributes.DetectionDiameter; i++)
                {
                    for (int j = 0; j < organism.attributes.DetectionDiameter; j++)
                    {
                        if (Grid.InBounds(firstX + i, firstY + j) && grid.IsMateAt(organism, firstX + i, firstY + j))
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



                System.Console.WriteLine("Waiting!");






            }


        }
    }
}
