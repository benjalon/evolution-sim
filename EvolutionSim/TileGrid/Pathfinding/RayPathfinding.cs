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

        private Tuple<int, int> OrgLocation(int posX, int posY)
        {

        }

        /// <summary>
        /// returns the path for the organism to follow to its prey
        /// </summary>
        /// <returns></returns>
        public List<Tile> CalculateRay()
        {
            
            Tile huntedOrganismPos = grid.GetTileAt(this.OrganismChased);
            Tile predatorOrganismPos = grid.GetTileAt(this.Predator);

            Tuple desiredLocation = new Tuple(huntedOrganismPos.Inhabitant.GridIndex.X, huntedOrganismPos.GridIndex.Y);

            //getting the absolute difference between the organism positions
            int xMagnitude = Math.Abs(huntedOrganismPos.Inhabitant.GridIndex.X - predatorOrganismPos.Inhabitant.GridIndex.X);

            int yMagnitude = Math.Abs(huntedOrganismPos.Inhabitant.GridIndex.Y - predatorOrganismPos.Inhabitant.GridIndex.Y);

            //so we first want to take the difference between the tiles
            for (int i = 0; i < xMagnitude)
            {


            }


           return Ray;


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
}
