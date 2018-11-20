using EvolutionSim.Pathfinding;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
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

            var firstX = organism.TileIndex.X - organism.attributes.DetectionRadius;
            var firstY = organism.TileIndex.Y - organism.attributes.DetectionRadius;

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
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME.Milliseconds;


            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {   //decide destination

                if (organism.DestinationTile is null)
                {
                    organism.MilliSecondsSinceLastMovement = 0;

                    Directions _num = (Directions)_random.Next(0, 4);
                    int _destinationTileX = organism.TileIndex.X;
                    int _destinationTileY = organism.TileIndex.Y;

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
                    organism.DestinationTile = grid.GetTileAt(_destinationTileX, _destinationTileY);

                }
                else
                {
                    if (organism.Rectangle == organism.DestinationTile.Rectangle)
                    {
                        grid.MoveOrganism(organism, organism.DestinationTile.TileIndex.X, organism.DestinationTile.TileIndex.Y);
                        organism.DestinationTile = null;
                        //organism.MilliSecondsSinceLastMovement = 0;

                    }
                    else
                    {
                        //MoveTowardsTile
                        Lerper lerp = new Lerper();


                        var newX = (int)lerp.Lerp(organism.Rectangle.X, organism.DestinationTile.Rectangle.X);
                        var newY = (int)lerp.Lerp(organism.Rectangle.Y, organism.DestinationTile.Rectangle.Y);
                        organism.SetPosition(newX, newY);


                    }

                }

                // organism.moveTowardstile

                //grid.MoveOrganism(organism, _destinationTileX, _destinationTileY);
            }


        }
        // Returns true if reached tile, false if not.
        private static bool Move(Organism organism,Rectangle DestinationRec,Point DestinationPoint,Grid grid){
            if (organism.Rectangle == DestinationRec)
            {
                grid.MoveOrganism(organism, DestinationPoint.X, DestinationPoint.Y);
                //organism.DestinationTile = null;

                return true;
            }
            else
            {
                //MoveTowardsTile
                Lerper lerp = new Lerper();


                var newX = (int)lerp.Lerp(organism.Rectangle.X, DestinationRec.X);
                var newY = (int)lerp.Lerp(organism.Rectangle.Y, DestinationRec.Y);
                organism.SetPosition(newX, newY);

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
                    if(Move(organism,Path.ElementAt(0).Rectangle,Path.ElementAt(0).TileIndex, grid))
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

            private static Tile FoodInRange(Organism organism, Grid grid)
            {

                // COMMENT THIS!!!!!!!!!!!!!!!!!!!!!!!!!!
                var max_depth = organism.attributes.DetectionRadius;
                var depth = 0;

                int firstX;
                int firstY;
                int num;
                int firstCheck;
                int i;
                int j;
                int x;
                int y;

                while (depth < max_depth)
                {
                    //the starting is the depth away from the origin +1 to compensate for the 0-2;
                    firstX = organism.TileIndex.X - (depth + 1);
                    firstY = organism.TileIndex.Y - (depth + 1);

                    num = 3 + (2 * depth); //number of tiles to check per depth level. 
                    firstCheck = 1 - depth;
                    i = -1;
                    j = 0;

                    while (i < num - 1)
                    {
                        i++;
                        x = firstX + i;
                        y = firstY + j;
                        if (Grid.InBounds(x, y) && grid.IsFoodAt(firstX + i, firstY + j))
                        {

                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }
                    
                    while (j < num - 1)
                    {
                        j++;
                        x = firstX + i;
                        y = firstY + j;
                        if (Grid.InBounds(x, y) && grid.IsFoodAt(firstX + i, firstY + j))
                        {

                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    while (i > 0)
                    {
                        i--;
                        x = firstX + i;
                        y = firstY + j;
                        if (Grid.InBounds(x, y) && grid.IsFoodAt(firstX + i, firstY + j))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    while (j > 0)
                    {
                        j--;
                        x = firstX + i;
                        y = firstY + j;
                        if (Grid.InBounds(x, y) && grid.IsFoodAt(firstX + i, firstY + j))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                    depth++;
                }
                return null;
            }
        }
    

        public static class EatingFood
        {

            public static void EatFood(Organism organism, Grid grid)
            {
                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME.Milliseconds;
                if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
                {
                    organism.MilliSecondsSinceLastMovement = 0;

                    Food food = organism.DestinationTile.Inhabitant as Food;
                    if (food != null) // It's rare but two organisms can attempt to eat the same food source
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
                int firstX = organism.TileIndex.X - organism.attributes.DetectionRadius;
                int firstY = organism.TileIndex.Y - organism.attributes.DetectionRadius;
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
