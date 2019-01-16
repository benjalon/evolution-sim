using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Data
{
    /// <summary>
    /// Properties used to create an organism
    /// </summary>
    public struct Attributes
    {
        public string Species { get; set; }
        public Texture2D Texture { get; set; }
        public DietTypes DietType { get; set; }
        public int MaxHealth { get; set; }
        public float Speed { get; set; }
        public float Strength { get; set; }
        public float Intelligence { get; set; }
        public bool ResistCold { get; set; }
        public bool ResistHeat { get; set; }
    

    }
}
