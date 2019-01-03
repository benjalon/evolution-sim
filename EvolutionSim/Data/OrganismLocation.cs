using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.Sprites;
using EvolutionSim.TileGrid;


namespace EvolutionSim.Data
{
    /// <summary>
    /// This class will be reposnisble for containing information related to organisms in pursuit by 
    /// a hunting organism
    /// </summary>
    class OrganismLocation
    {
        private int gridPositionX;
        private int gridPositionY;

        //defines how often we can update the tracked organisms location
        public const int UPDATE_COOLDOWN = 1000;

        /// <summary>
        /// take a readonly instance of the grid and the
        /// organism being hunted
        /// </summary>
        public OrganismLocation(int gridPositionX, int gridPositionY)
        {
            this.gridPositionX = gridPositionX;
            this.gridPositionY = gridPositionY;




        }




    }
}
