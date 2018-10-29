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

        public static void Roam(Organism organism,Grid grid)
        {

            //decide destination
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
                    if (_destinationTileX < Grid.horizontalCount- 1)
                    {
                        _destinationTileX += 1;
                    }
                    break;
            }

            
            if (!grid._tiles[_destinationTileX][_destinationTileY].HasMapItem())
            {
               
                grid._tiles[organism.GridPosition.X][organism.GridPosition.Y].MoveInhabitant(grid._tiles[_destinationTileX][_destinationTileY]);

            }

            //if destination full decide again.
        }
    }
}
