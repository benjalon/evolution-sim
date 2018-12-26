using EvolutionSim.TileGrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Sprites
{
    /// <summary>
    /// Herbivore food sources which can grow on Tiles.
    /// </summary>
    ///




    public class Food : GridItem
    {
        private const float MIN_SCALE = 0.4f;
        private const int MAX_MEAT_HEALTH = 40;
        public const int MAX_GRASS_HEALTH = 10;
        private const float OFFSET_MULTIPLIER = 0.5f;

        public bool IsHerbivoreFood { get; private set; }
        private int MaxFoodHealth { get => IsHerbivoreFood ? MAX_GRASS_HEALTH : MAX_MEAT_HEALTH;  }

        //this is where the scale is being  calculated for food based on how much health that food item has over the whole food item
        private readonly float scale;
        private readonly float scaleOffset;

        public Food(Texture2D texture, bool herbivoreFood, int health) : base(texture, health)
        {
            this.IsHerbivoreFood = herbivoreFood;

            // Clamp food health
            if (health > MaxFoodHealth)
            {
                health = MaxFoodHealth; // There has to be a limit here or organisms will be eating food that takes forever to finish
            }
            
            // The more health a piece of food gives, the bigger it is drawn
            this.scale = (float)health / MaxFoodHealth;
            if (this.scale < MIN_SCALE)
            {
                this.scale = MIN_SCALE; // Limit how small food can be drawn
            }
            this.scaleOffset = (Tile.TILE_SIZE * (1.0f - this.scale)) * OFFSET_MULTIPLIER;
        }

        /// <summary>
        /// Eats the food, lowering its health over time
        /// </summary>
        /// <returns>Whether or not the food is fully eaten</returns>
        public void BeEaten()
        {
            DecreaseHealth(1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, new Vector2(this.rectangle.Location.X + this.scaleOffset, this.rectangle.Location.Y + this.scaleOffset), null, Color.White, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0.0f);
        }
    }
}
