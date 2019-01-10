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
   public class RayPathfinding
    {
       private Organism OrganismChased;
       private Organism Predator;
        private readonly Grid grid;
    
        //tiles between predator and prey
       private List<Tile> Ray;


        /// <summary>
        /// Construct a ray between two objects which is updated with 
        /// each lambda tick as the organisms move
        /// </summary>
        /// <param name="huntedOrganism"></param>
        /// <param name="predator"></param>
        public RayPathfinding(Organism organismChased, Organism predator, Grid grid)
        {

            this.OrganismChased = organismChased;
            this.Predator = predator;


        }

        //private Tuple<int, int> OrgLocation(int posX, int posY)
        //{
        //    var X = posX;
        //    var Y = posY;

        //    Tuple createdTuple;

        //    return 
        //}

        /// <summary>
        /// returns the path for the organism to follow to its prey
        /// //needs to take into account potential obstructions
        /// </summary>
        /// <returns></returns>
        public List<Tile> CalculateRay()
        {
            
            Tile huntedOrganismPos = grid.GetTileAt(this.OrganismChased);
            Tile predatorOrganismPos = grid.GetTileAt(this.Predator);

            int desiredLocationX = huntedOrganismPos.Inhabitant.GridIndex.X;
            int desiredLocationY = huntedOrganismPos.Inhabitant.GridIndex.Y;

            int predatorLocationX = predatorOrganismPos.Inhabitant.GridIndex.X;
            int predatorLocationY = predatorOrganismPos.Inhabitant.GridIndex.Y;

            //getting the absolute difference between the organism positions
            int xMagnitude = Math.Abs(huntedOrganismPos.Inhabitant.GridIndex.X - predatorOrganismPos.Inhabitant.GridIndex.X) - 1;

            int yMagnitude = Math.Abs(huntedOrganismPos.Inhabitant.GridIndex.Y - predatorOrganismPos.Inhabitant.GridIndex.Y) - 1;

            
        
            //while the co-ordinates aren't adjacent to the desired organisms location
             while(predatorLocationX != desiredLocationX + 1 || predatorLocationX != desiredLocationX - 1
                    || predatorLocationY != desiredLocationY + 1 || predatorLocationY != desiredLocationY - 1)
            {
                if(predatorLocationX > desiredLocationX && xMagnitude != 0)
                {
                    predatorLocationX--;
                    xMagnitude--;

                }

                else if(predatorLocationX < desiredLocationX && xMagnitude != 0)
                {
                    predatorLocationX++;
                    xMagnitude--;

                }

                if (predatorLocationY > desiredLocationY && yMagnitude != 0)
                {
                    predatorLocationY--;
                    yMagnitude--;

                }

                else if (predatorLocationY < desiredLocationY && yMagnitude != 0)
                {
                    predatorLocationY++;
                    yMagnitude--;

                }

                //add the next step to the 
                Ray.Add(grid.GetTileAt(predatorLocationX, predatorLocationY));


            }
           
                  return Ray;

            }


         


        }



        /// <summary>
        /// update the ray object with each tick of the timer
        /// </summary>
        /// <param name="timeManager"></param>
        /// <param name="grid"></param>
        public void UpdateRay(TimeManager timeManager, Grid grid)
        {


        }




}
