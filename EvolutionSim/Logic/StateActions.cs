using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic
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

            if (distance == 1)
            {
                return true;
            }
            return false;
        }

        public static List<Point> GetPointsInRange(Organism organism)   
        {
            List<Point> toRet = new List<Point>();

            int firstX = organism.GridPosition.X - (organism._attributes._DetectionRadius) / 2;
            int firstY = organism.GridPosition.Y - (organism._attributes._DetectionRadius) / 2;

            for (int i = 0; i < organism._attributes._DetectionRadius; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    toRet.Add(new Point(firstX + i, firstY + j));

                }

            }
            return toRet;

        }

        private static Random _random = new Random();
        public static Boolean InBounds(int x, int y)
        {
            if (y >= Grid.verticalCount || y < 0 || x >= Grid.horizontalCount || x < 0)
            {
                return false;
            }
            return true;
        }
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
                        if (_destinationTileY < Grid.verticalCount - 1)
                        {
                            _destinationTileY += 1;
                        }
                        break;
                    case Directions.Right:
                        if (_destinationTileX < Grid.horizontalCount - 1)
                        {
                            _destinationTileX += 1;
                        }
                        break;
                }


                if (!grid._tiles[_destinationTileX][_destinationTileY].HasMapItem())
                {

                    grid._tiles[organism.GridPosition.X][organism.GridPosition.Y].MoveInhabitant(grid._tiles[_destinationTileX][_destinationTileY]);

                }
            }

            //if destination full decide again.
        }

        public static void MoveAlongPath(Organism organism, Grid grid, List<Tile> Path)
        {
            organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;

            if (organism.MilliSecondsSinceLastMovement > Organism.MS_PER_DIRECTION_CHANGE)
            {
                organism.MilliSecondsSinceLastMovement = 0;

                if (Path.Any() && !Path.First().HasMapItem())
                {
                    grid._tiles[organism.GridPosition.X][organism.GridPosition.Y].MoveInhabitant(Path.First());
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
            public static Boolean SeekFood(Organism organism, Grid grid)
            {

                // Essentially, if food has been located, and path calculated, we move towards food
 
                // If we're not moving on a path, but we're in the state seeking food, then we haven't yet found any food.
                    Tile PotentialFood = FoodInRange(organism, grid);

                    if (PotentialFood != null)
                    {
                        // Path to food
                        List<Tile> Path = Logic.Pathfinding.PathFinding.FindShortestPath(organism.ParentTile, PotentialFood, grid._tiles);
                        organism._Path = Path;
                        if(Path.Count == 0)
                    {
                        organism.DestinationTile = PotentialFood;
                    }
                    else
                    {
                        organism.DestinationTile = PotentialFood;
                        organism._Path.RemoveAt(organism._Path.Count - 1);
                    }

                        organism.MovingOnPath = true;
                        return true;
                    }
                    else
                    {
                        Logic.StateActions.Roam(organism, grid);
                        return false;
                    }
                

               
               
                

                //if destination full decide again.
            }

            private static Tile FoodInRange(Organism organism,Grid grid)
            {
                int firstX = organism.GridPosition.X - (organism._attributes._DetectionRadius)/2;
                int firstY = organism.GridPosition.Y - (organism._attributes._DetectionRadius)/2;

                for (int i = 0; i < organism._attributes._DetectionRadius; i++)
                {
                    for (int j = 0; j < organism._attributes._DetectionRadius; j++)
                    {
                        if (InBounds(firstX + i,firstY+j) && grid._tiles[firstX + i][firstY+j].Inhabitant is Food)
                        {


                            return grid._tiles[firstX + i][firstY + j];

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

                    Food food = (Food)organism.DestinationTile.Inhabitant;
                    food.foodHealth = 0;
                    // organism._attributes._hunger += 0.3;
                    organism.DestinationTile = null;
                    organism._Path = new List<Tile>();
                }
          }
        }

    }
}
