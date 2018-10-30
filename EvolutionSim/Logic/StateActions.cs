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
        private static Boolean InBounds(int x, int y)
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

        public static class SeekingFood
        {
            public static void SeekFood(Organism organism, Grid grid)
            {
                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;
                if (FoodInRange(organism,grid))
                {
                    // Path to food
                }
                else
                {
                    Logic.StateActions.Roam(organism, grid);
                }
               
                

                //if destination full decide again.
            }

            private static bool FoodInRange(Organism organism,Grid grid)
            {
                int firstX = organism.GridPosition.X - (organism._attributes._DetectionRadius)/2;
                int firstY = organism.GridPosition.Y - (organism._attributes._DetectionRadius)/2;

                for (int i = 0; i < organism._attributes._DetectionRadius; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (InBounds(firstX + i,firstY+j) && grid._tiles[firstX + i][firstY+j].Inhabitant is Food)
                        {

                            return true;
                        }


                    }

                }
                return false;






                //// Check +X +Y -X -Y
                //for (int i = 1; i < 3; i++)
                //{
                //    // Break out of loop when food is found
                //    // Collision detection
                //    if (InBounds(organism.GridPosition.X + i, organism.GridPosition.Y) && grid._tiles[organism.GridPosition.X + i][organism.GridPosition.Y].Inhabitant is Food)
                //    {

                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }

                //    if (InBounds(organism.GridPosition.X - i, organism.GridPosition.Y) && grid._tiles[organism.GridPosition.X - i][organism.GridPosition.Y].Inhabitant is Food)
                //    {

                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }
                //    if (InBounds(organism.GridPosition.X, organism.GridPosition.Y + i) && grid._tiles[organism.GridPosition.X][organism.GridPosition.Y + i].Inhabitant is Food)
                //    {

                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }
                //    if (InBounds(organism.GridPosition.X, organism.GridPosition.Y - i) && grid._tiles[organism.GridPosition.X][organism.GridPosition.Y - i].Inhabitant is Food)
                //    {

                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }
                //    if (InBounds(organism.GridPosition.X - i, organism.GridPosition.Y + i) && grid._tiles[organism.GridPosition.X - i][organism.GridPosition.Y + i].Inhabitant is Food)
                //    {
                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }

                //    if (InBounds(organism.GridPosition.X + i, organism.GridPosition.Y + i) && grid._tiles[organism.GridPosition.X + i][organism.GridPosition.Y + i].Inhabitant is Food)
                //    {
                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }
                //    if (InBounds(organism.GridPosition.X - i, organism.GridPosition.Y - i) && grid._tiles[organism.GridPosition.X - i][organism.GridPosition.Y - i].Inhabitant is Food)
                //    {
                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }
                //    if (InBounds(organism.GridPosition.X + i, organism.GridPosition.Y - i) && grid._tiles[organism.GridPosition.X + i][organism.GridPosition.Y - i].Inhabitant is Food)
                //    {
                //        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                //        return true;
                //    }






                //}



            }

            


        }
       


    }
}
