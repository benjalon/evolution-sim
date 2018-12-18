using EvolutionSim.Sprites;
using System;

namespace EvolutionSim.Data
{
    public class MatingArgs : EventArgs
    {
        public Organism Father { get; private set; }
        public Organism Mother { get; private set; }

        public MatingArgs(Organism father, Organism organism) : base()
        {
            Father = father;
            Mother = organism;
        }
    }
}
