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
        public const int KILL_HEALTH = int.MaxValue;
        public const int DETECTION_RADIUS = 8;
        public const int DETECTION_DIAMETER = DETECTION_RADIUS * 2;
        private const int INCREMENT_HEALTH = 1;
        private const float EATING_REGEN = 0.075f;
        private const float SCALE_MULTIPLIER = 0.2f;
        private const float SCALE_LIMIT = 1.0f; // we want an upper limit of 1.0f on both strength and speed
        private const int AGE_LOWER_BOUND = 70; // represents the range of ages possible
        private const int AGE_UPPER_BOUND = 130;


        // Breed attributes
        public Attributes Attributes { get; set; }

        // Simulation Attributes
        public int Age { get; set; } = 0;
        public float Hunger { get; set; } = 0.2f;
        private float scale = 1.0f;

        public int MaxAge { get; set; } = 0;

        // Pathfinding 
        public bool Computing { get; set; } = false;
        public List<Tile> Path { get; set; } = new List<Tile>();
        public Tile DestinationTile { get => Path.Count > 0 ? Path[Path.Count - 1] : null; }

        // State management
        public States State { get; set; } = States.Roaming;
        public int MsSinceLastRoam { get; set; } = 0;

        public int MsSinceLastWeather { get; set; } = 0;

        public int MsSinceLastWaited{ get; set; } = 0;

        public int MsSinceLastMate { get; set; } = 0;
        

        //these booleans are used inconjection with the state machine to determine 
        //it's 
        public int MsSinceLastHunted { get; set; } = 0;
        public bool JustMated { get; set; } = false;
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }
        public bool Hunting { get; set; }

        //this determines if the organism is being targeted for being hunted
        public bool Frozen { get; set; }

        public bool RecentlyHunted { get; set; } = false;

        // Misc
        private readonly Healthbar healthbar;
        public bool IsSelected { get; set; } = false;
        
        /// <summary>
        /// Construct an organism, create attributes and allocate health bar and textures
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="healthbarTextures"></param>
        public Organism(Attributes attributes, Tuple<Texture2D, Texture2D> healthbarTextures) : base(attributes.Texture, attributes.MaxHealth)
        {

            TOTAL_POPULATION++;
            this.MaxAge = Graphics.RANDOM.Next(AGE_LOWER_BOUND, AGE_UPPER_BOUND);
            this.Attributes = attributes;

            this.scale = this.Attributes.Strength;

            if (this.scale > SCALE_LIMIT)
            {
                this.scale = SCALE_LIMIT;
            }

            this.healthbar = new Healthbar(healthbarTextures, rectangle, this.defaultHealth);
        }

        /// <summary>
        /// Draw the organism, scales based on strength
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //this is where the size of the organism is calculated based on strengh
            var scaleOffset = (Tile.TILE_SIZE * (1.0f - this.Attributes.Strength)) * SCALE_MULTIPLIER; // TODO: if organisms never get stronger, this can be pre-calculated at birth
            
            if (IsSelected)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.rectangle.Location.X + scaleOffset, this.rectangle.Location.Y + scaleOffset), null, Color.Purple, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.rectangle.Location.X + scaleOffset, this.rectangle.Location.Y + scaleOffset), null, Color.White, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0.0f);
            }

            this.healthbar.Draw(spriteBatch);
        }
        
        /// <summary>
        /// Called when organism is consuming food
        /// </summary>
        public void Eat()
        {
            //increase the health of organism as well as their health
            Hunger += EATING_REGEN;
            IncreaseHealth(INCREMENT_HEALTH);
        }
        
        public override void SetScreenPosition(int x, int y)
        {
            base.SetScreenPosition(x, y);
            this.healthbar.SetScreenPosition(x, y);
        }

 
        /// <summary>
        /// Method called to increase the health of the organism
        /// </summary>
        /// <param name="value"></param>
        public override void IncreaseHealth(int value)
        {
            base.IncreaseHealth(value);
            this.healthbar.CurrentHealth = this.Health;
        }

        /// <summary>
        /// Method called to reduce the health of organism
        /// </summary>
        /// <param name="value"></param>
        public override void DecreaseHealth(int value)
        {
            base.DecreaseHealth(value);
            this.healthbar.CurrentHealth = this.Health;
        }
    }
}
