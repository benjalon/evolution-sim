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
        private const int INCREMENT_HEALTH = 1;
        private const float EATING_REGEN = 0.4f;
        private const float SCALE_MULTIPLIER = 0.2f;
        private const float UPPER_LIMIT = 1.0f; // we want an upper limit of 1.0f on both strength and speed



        // Breed attributes
        public Attributes Attributes { get; }

        public event EventHandler BeingHunted;

        // Simulation Attributes
        public int Age { get; set; } = 0;
        public float Hunger { get; set; } = 0.2f;

        // Pathfinding 
        public bool Computing { get; set; } = false;
        public List<Tile> Path { get; set; } = new List<Tile>();

        //this is just always the final tile in the path list
        public Tile DestinationTile { get => Path.Count > 0 ? Path[Path.Count - 1] : null; }

        // State management
        public States State { get; set; } = States.Roaming;
        public int MsSinceLastRoam { get; set; } = 0;
        public int MsSinceLastMate { get; set; } = 0;
        //not sure if I should move this
        public int MsSinceLastHunted { get; set; } = 0;
        public bool JustMated { get; set; } = false;
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }

        //this will be Null if the organism is a omnivore or herbivore
        public bool? PreyFound { get; set; }


        public bool RecentlyHunted { get; set; } = false;

        // Misc
        private readonly Healthbar healthbar;
        public bool IsSelected { get; set; } = false;
        
        public Organism(Attributes attributes, Tuple<Texture2D, Texture2D> healthbarTextures) : base(attributes.Texture, attributes.MaxHealth)
        {
            TOTAL_POPULATION++;

            this.Attributes = attributes;
            this.healthbar = new Healthbar(healthbarTextures, rectangle, this.defaultHealth);

            //if the organism isn't a carnivore then make PreyFound null
            if(this.Attributes.DietType == DietTypes.Canivore)
            {
               this.PreyFound = false; 
            }
            else
            {
                this.PreyFound = null;
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //this is where the size of the organism is calculated based on strengh
            var scaleOffset = (Tile.TILE_SIZE * (1.0f - this.Attributes.Strength)) * SCALE_MULTIPLIER; // TODO: if organisms never get stronger, this can be pre-calculated at birth

            //cap the scaleoffset to a certain size
            //otherwise we get big issues 
           if(scaleOffset > UPPER_LIMIT)
            {

                scaleOffset = UPPER_LIMIT;

            }


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
        
        /// <summary>
        /// Called when organism is consuming food
        /// </summary>
        public void Eat()
        {
            //if the organism reaches 1.0 Hunger then stop eating as they are full
            if (this.Hunger < 1.0)
            {
                Hunger += EATING_REGEN;
                IncreaseHealth(INCREMENT_HEALTH);

            }

            return;
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

        /// <summary>
        /// Mostly used for de-bugging Just kills an organism when called
        /// </summary>
        public void killOrganism()
        {
            this.Health = 0;
        }
    }
}
