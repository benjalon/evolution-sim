using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// Herbivore food sources which can grow on Tiles.
    /// </summary>
    public class Food : GridItem
    {
        public Food(Texture2D texture) : base(texture) { }

        /// <summary>
        /// Eats the food, lowering its health over time
        /// </summary>
        /// <returns>Whether or not the food is fully eaten</returns>
        public void Eat()
        {
            LowerHealth(999); // In the future this should take time but for now it's instant
        }
    }
}
