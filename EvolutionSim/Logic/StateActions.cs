using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic
{
    public static class StateActions
    {
        private static Random _random = new Random();

        public static void Roam(Organism organism, Grid grid)
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


        /// <summary>
        /// This method stops an organism when it's eating, then de-constructs the food object
        /// </summary>
        public static class Eating{

            public static void Eat(Organism organism, Grid grid, Food itemBeingEaten)
        {
                // do nothing at the moment, organism is going to stay still

                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;

                if (organism.MilliSecondsSinceLastMovement > (Organism.MS_PER_DIRECTION_CHANGE * 5))
                {
                    
                    //take the food off the map
                    

                }
            

       }



        public static class SeekingFood
        {
            public static void SeekFood(Organism organism, Grid grid)
            {
                organism.MilliSecondsSinceLastMovement += Graphics.ELAPSED_TIME;

                if (FoodInRange(organism, grid))
                {
                    
                    // Path to food
                }
                else
                {
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
                }

                //if destination full decide again.
            }

            private static Boolean FoodInRange(Organism organism,Grid grid)
            {
                int firstX = organism.GridPosition.X - 2;
                int firstY = organism.GridPosition.Y - 2;

                for (int i = 0; i < 5; i++)
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



                return false;
            }

            private static Boolean InBounds(int x, int y)
            {
                if (y >= Grid.verticalCount || y < 0 || x >= Grid.horizontalCount || x < 0)
                {
                    return false;
                }
                return true;
            }


        }
       


    }
}
