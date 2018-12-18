using EvolutionSim.Data;
using EvolutionSim.TileGrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim.Sprites
{
    public class Organism : GridItem
    {
        public static int TOTAL_POPULATION = 0;

        public const int DETECTION_RADIUS = 5;
        public const int DETECTION_DIAMETER = DETECTION_RADIUS * 2;

        // Breed attributes
        public Attributes Attributes { get; }

        // Simulation Attributes
        public int Age { get; set; } = 0;
        public float Hunger { get; set; } = 0.2f;

        // Pathfinding 
        public bool Computing { get; set; } = false;
        public List<Tile> Path { get; set; } = new List<Tile>();
        public Tile DestinationTile { get => Path.Count > 0 ? Path[Path.Count - 1] : null; }

        // State management
        public States State { get; set; } = States.Roaming;
        public int MsSinceLastRoam { get; set; } = 0;
        public int MsSinceLastMate { get; set; } = 0;
        public bool JustMated { get; set; } = false;
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }

        // Misc
        private readonly Healthbar healthbar;
        public bool IsSelected { get; set; } = false;
        
        public Organism(Attributes attributes, Tuple<Texture2D, Texture2D> healthbarTextures) : base(attributes.Texture, attributes.MaxHealth)
        {
            TOTAL_POPULATION++;

            this.Attributes = attributes;
            this.healthbar = new Healthbar(healthbarTextures, rectangle, this.defaultHealth);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var scaleOffset = (Tile.TILE_SIZE * (1.0f - this.Attributes.Strength)) * 0.5f; // TODO: if organisms never get stronger, this can be pre-calculated at birth

            if (IsSelected)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.rectangle.Location.X + scaleOffset, this.rectangle.Location.Y + scaleOffset), null, Color.Purple, 0, Vector2.Zero, this.Attributes.Strength, SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.rectangle.Location.X + scaleOffset, this.rectangle.Location.Y + scaleOffset), null, Color.White, 0, Vector2.Zero, this.Attributes.Strength, SpriteEffects.None, 0.0f);
            }

            this.healthbar.Draw(spriteBatch);
        }
        
        public void Eat()
        {
            Hunger += 0.04f;

            IncreaseHealth(1);
        }
        
        public override void SetScreenPosition(int x, int y)
        {
            base.SetScreenPosition(x, y);
            this.healthbar.SetScreenPosition(x, y);
        }

        public override void IncreaseHealth(int value)
        {
            base.IncreaseHealth(value);
            this.healthbar.CurrentHealth = this.Health;
        }

        public override void DecreaseHealth(int value)
        {
            base.DecreaseHealth(value);
            this.healthbar.CurrentHealth = this.Health;
        }
    }
}
