using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// Herbivore food sources which can grow on Tiles.
    /// </summary>
    ///

      


    public class Food : GridItem
    {

        public bool herbivoreFriendly { get; private set;}

        public Food(Texture2D texture) : base(texture) {

            this.herbivoreFriendly = true;

        }

        /// <summary>
        /// this can be called to specify if the spawned food cannont be consumed by an omnivore.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="omnivoreFriendly"></param>
        public Food(Texture2D texture, bool herbivoreFriendly) : base(texture) {

            this.herbivoreFriendly = herbivoreFriendly;
        }


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
