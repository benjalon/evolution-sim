using System;

namespace EvolutionSim.Data
{
    public class CreationArgs : EventArgs
    {
        public int Count { get; private set; }

        public CreationArgs(int count) : base()
        {
            Count = count;
        }
    }
}
