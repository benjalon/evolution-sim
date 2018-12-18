using System;

namespace EvolutionSim.Data
{
    public enum DrawingSettings
    {
        Grass,
        Mountain,
        Water,
        Organism,
        Food
    }

    public class DrawingArgs : EventArgs
    {
        public DrawingSettings DrawingSetting { get; private set; }

        public DrawingArgs(DrawingSettings drawingSetting) : base()
        {
            DrawingSetting = drawingSetting;
        }
    }
}