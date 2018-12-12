using EvolutionSim.TileGrid.GridItems;
using System;

namespace EvolutionSim.Utility
{
    public class MatingArgs : EventArgs
    {
        public Organism Father { get; private set; }
        public Organism Mother { get; private set; }

        public MatingArgs(Organism father, Organism organism) : base()
        {
            Father = organism;
            Mother = organism;
        }
    }
}
