using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EvolutionSim.Utility;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// Herbivore food sources which can grow on Tiles.
    /// </summary>
    ///




    public class Food : GridItem
    {
        public bool IsHerbivoreFood { get; private set; }

        public Food(Texture2D texture, bool herbivoreFood, int health) : base(texture, health)
        {
            this.IsHerbivoreFood = herbivoreFood;
        }

        /// <summary>
        /// Eats the food, lowering its health over time
        /// </summary>
        /// <returns>Whether or not the food is fully eaten</returns>
        public void BeEaten()
        {
            DecreaseHealth(1);
        }
    }
}
