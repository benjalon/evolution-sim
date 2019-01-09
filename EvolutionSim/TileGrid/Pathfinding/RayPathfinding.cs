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
    class RayPathfinding
    {
       private OrganismLocation OrganismChased;
       private Organism Predator;
    
        //tiles between predator and prey
       private List<Tile> Ray;


        /// <summary>
        /// Construct a ray between two objects which is updated with 
        /// each lambda tick as the organisms move
        /// </summary>
        /// <param name="huntedOrganism"></param>
        /// <param name="predator"></param>
        public RayPathfinding(OrganismLocation huntedOrganism, Organism predator)
        {

            this.OrganismChased = huntedOrganism;
            this.Predator = predator;


        }

        /// <summary>
        /// returns the path for the organism to follow to its prey
        /// </summary>
        /// <returns></returns>
        public List<Tile> CalculateRay()
        {
            this.HuntedOrganism


        }



        /// <summary>
        /// update the ray with each tick of the timer
        /// </summary>
        /// <param name="timeManager"></param>
        /// <param name="grid"></param>
        public void UpdateRay(TimeManager timeManager, Grid grid)
        {


        }




    }
}
