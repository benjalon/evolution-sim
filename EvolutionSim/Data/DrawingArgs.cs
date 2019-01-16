using System;

namespace EvolutionSim.Data
{
    /// <summary>
    /// Types of terrian/objects to draw 
    /// </summary>
    public enum DrawingSettings
    {
        Grass,
        Mountain,
        Water,
        Organism,
        Food
    }

    /// <summary>
    /// Set the drawing args in the draw constructor
    /// </summary>
    public class DrawingArgs : EventArgs
    {
        public DrawingSettings DrawingSetting { get; private set; }

        public DrawingArgs(DrawingSettings drawingSetting) : base()
        {
            DrawingSetting = drawingSetting;
        }
    }
}