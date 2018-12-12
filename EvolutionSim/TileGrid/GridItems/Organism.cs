using EvolutionSim.StateManagement;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// Dictates the type of food the organism will be eating
    /// </summary>
    public enum DietTypes
    {
        Herbivore,
        Omnivore,
        Canivore
    }

    public class Organism : GridItem
    {
        public static int TOTAL_POPULATION = 0;

        // Attributes
        public OrganismAttributes Attributes { get; }

        // Pathfinding 
        public bool Computing { get; set; } = false;
        public List<Tile> Path { get; set; } = new List<Tile>();
        public Tile DestinationTile { get; set; } // The next tile along after the path

        // State management
        public States State { get; set; } = States.Roaming;
        public int MsSinceLastMate { get; set; } = 0;
        public bool JustMated { get; set; } = false;
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }

        // Misc
        private readonly Healthbar healthbar;
        public bool IsSelected { get; set; } = false;
        
        public Organism(Breed breed, Tuple<Texture2D, Texture2D> healthbarTextures) : base(breed.Texture, Graphics.RANDOM.Next(10, 30))
        {
            TOTAL_POPULATION++;

            this.Attributes = new OrganismAttributes(breed);
            this.healthbar = new Healthbar(healthbarTextures, rectangle, defaultHealth);
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
            this.Attributes.Hunger += 0.04f;

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
    
    public class OrganismAttributes
    {
        public string Species { get; set; }
        public DietTypes DietType { get; set; }
        public float Speed { get; set; }
        public float Strength { get; set; }
        public bool ResistCold { get; set; }
        public bool ResistHeat { get; set; }

        public int Age { get; set; }
        public float Hunger { get; set; }
        public int DetectionRadius { get; set; }
        public int DetectionDiameter { get; set; }

        public OrganismAttributes(Breed breed)
        {
            Species = breed.Species;
            DietType = breed.DietType;
            Speed = breed.Speed;
            Strength = breed.Strength;
            ResistCold = breed.ResistCold;
            ResistHeat = breed.ResistHeat;

            Age = 0;
            Hunger = 0.2f;
            DetectionRadius = 3;
            DetectionDiameter = DetectionRadius * 2;
        }
    }
}
