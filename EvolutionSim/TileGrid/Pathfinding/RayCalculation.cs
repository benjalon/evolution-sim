using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.Utility;

namespace EvolutionSim.TileGrid.Pathfinding
{

    /// <summary>
    /// This gives us simplified path finding
    /// </summary>
   public class RayCalculation
    {
       private Organism OrganismChased;
       private Organism Predator;
       private List<Tile> Ray;


        /// <summary>
        /// Construct a ray between two objects which is updated with 
        /// each lambda tick as the organisms move
        /// </summary>
        /// <param name="huntedOrganism"></param>
        /// <param name="predator"></param>
        public RayCalculation(Organism organismChased, Organism predator)
        {

            this.OrganismChased = organismChased;
            this.Predator = predator;
            Ray = new List<Tile>();

        }

        /// <summary>
        /// returns the path for the organism to follow to its prey
        /// //is oblivious to potential obstructions
        /// </summary>
        /// <returns></returns>
        public void CalculateRay(Grid grid)
        {
            

            int desiredLocationX = this.OrganismChased.GridIndex.X;
            int desiredLocationY = this.OrganismChased.GridIndex.Y;

            int predatorLocationX = this.Predator.GridIndex.X;
            int predatorLocationY = this.Predator.GridIndex.Y;

            //getting the absolute difference between the organism positions
            int xMagnitude = Math.Abs(desiredLocationX - predatorLocationX) - 1;

            int yMagnitude = Math.Abs(desiredLocationY - predatorLocationY) - 1;

            
        
            //while the co-ordinates aren't adjacent to the desired organisms location
             while((predatorLocationX != desiredLocationX + 1 || predatorLocationX != desiredLocationX - 1)
                    && (predatorLocationY != desiredLocationY + 1 || predatorLocationY != desiredLocationY - 1))
            {
                if(predatorLocationX > desiredLocationX + 1 && xMagnitude != 0)
                {
                    predatorLocationX--;
                    xMagnitude--;

                }

                else if(predatorLocationX < desiredLocationX - 1 && xMagnitude != 0)
                {
                    predatorLocationX++;
                    xMagnitude--;

                }

                if (predatorLocationY > desiredLocationY + 1 && yMagnitude != 0)
                {
                    predatorLocationY--;
                    yMagnitude--;

                }

                else if (predatorLocationY < desiredLocationY - 1 && yMagnitude != 0)
                {
                    predatorLocationY++;
                    yMagnitude--;

                }

                //add the next step to the 
                Ray.Add(grid.GetTileAt(predatorLocationX, predatorLocationY));


            }

             //set the predator's path to this rey
            Predator.Path = this.Ray;

            }



        }

}
