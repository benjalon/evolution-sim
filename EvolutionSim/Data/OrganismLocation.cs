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
    /// This class will be reposnsible for containing information related to organisms in pursuit by 
    /// a hunting organism
    /// </summary>
    public class OrganismLocation
    {

        public Organism HuntedOrganism { get; private set;}
        public Coordinates Corodinates { get; set; }

        //defines how often we can update the tracked organisms location
        public const int UPDATE_COOLDOWN = 1000;

        public OrganismLocation(Organism organism, int gridPositionX, int gridPositionY)
        {
            this.Corodinates = new Coordinates(gridPositionX, gridPositionY);
            this.HuntedOrganism = organism;

        }

        public class Coordinates
        {

            private int GridPositionX;
            private int GridPositionY;


            public Coordinates(int posX, int posY)
            {

                this.GridPositionX = posX;
                this.GridPositionY = posY;


            }

        }
    }

}
