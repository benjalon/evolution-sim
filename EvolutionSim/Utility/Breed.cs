using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Utility
{
    public struct Breed
    {
        public string Species { get; set; }
        public Texture2D Texture { get; set; }
        public DietTypes DietType { get; set; }
        public int MaxHealth { get; set; }
        public float Speed { get; set; }
        public float Strength { get; set; }
        public bool ResistCold { get; set; }
        public bool ResistHeat { get; set; }
    }
}
