﻿using EvolutionSim.Pathfinding;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionSim.StateManagement
{
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

            var firstX = organism.GridPosition.X - organism.attributes.DetectionRadius;
            var firstY = organism.GridPosition.Y - organism.attributes.DetectionRadius;

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
        
        public static void Roam(Organism organism,Grid grid)
        {
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;

            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {   //decide destination

                organism.MilliSecondsSinceLastMovement = 0;
                Directions _num = (Directions)_random.Next(0, 4);
                int _destinationTileX = organism.GridPosition.X;
                int _destinationTileY = organism.GridPosition.Y;

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
                        if (_destinationTileY < Grid.VerticalCount - 1)
                        {
                            _destinationTileY += 1;
                        }
                        break;
                    case Directions.Right:
                        if (_destinationTileX < Grid.HorizontalCount - 1)
                        {
                            _destinationTileX += 1;
                        }
                        break;
                }

                grid.MoveOrganism(organism, _destinationTileX, _destinationTileY);
            }

            //if destination full decide again.
        }

        public static void MoveAlongPath(Organism organism, Grid grid, List<Tile> Path)
        {
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;

            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {
                organism.MilliSecondsSinceLastMovement = 0;

                if (Path.Any() && !Path.First().HasInhabitant())
                {
                    grid.MoveOrganism(organism, Path[0]);
                    Path.RemoveAt(0);

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
                    List<Tile> Path = PathFinding.FindShortestPath(organism.ParentTile, potentialFood, grid);
                    organism.Path = Path;
                    if(Path.Count == 0)
                    {
                        organism.DestinationTile = potentialFood;
                    }
                    else
                    {
                        organism.DestinationTile = potentialFood;
                        organism.Path.RemoveAt(organism.Path.Count - 1);
                    }

                    organism.MovingOnPath = true;
                    
                }
                else
                {
                    Roam(organism, grid);
                    
                }
                

                //if destination full decide again.
            }

            private static Tile FoodInRange(Organism organism,Grid grid)
            {
                int firstX = organism.GridPosition.X - organism.attributes.DetectionRadius;
                int firstY = organism.GridPosition.Y - organism.attributes.DetectionRadius;
                for (int i = 0; i < organism.attributes.DetectionDiameter; i++)
                {
                    for (int j = 0; j < organism.attributes.DetectionDiameter; j++)
                    {
                        if (Grid.InBounds(firstX + i, firstY + j) && grid.IsFoodAt(firstX + i, firstY + j))
                        {
                            return grid.GetTileAt(firstX + i, firstY + j);
                        }
                    }

                }
                return null;
            }
        }

        public static class EatingFood
        {

            public static void EatFood(Organism organism, Grid grid)
            {
                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;
                if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
                {
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
                    List<Tile> Path = PathFinding.FindShortestPath(organism.ParentTile, potentialMate, grid);

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
                int firstX = organism.GridPosition.X - organism.attributes.DetectionRadius;
                int firstY = organism.GridPosition.Y - organism.attributes.DetectionRadius;
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
