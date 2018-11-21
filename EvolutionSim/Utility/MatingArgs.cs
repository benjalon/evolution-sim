using EvolutionSim.TileGrid.GridItems;
using System;

namespace EvolutionSim.Utility
{
    public class MatingArgs : EventArgs
    {
        public Organism Mother { get; private set; }

        public MatingArgs(Organism organism) : base()
        {
            Mother = organism;
        }
    }
}
