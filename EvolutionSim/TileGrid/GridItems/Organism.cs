﻿using EvolutionSim.StateManagement;
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
        public DietTypes OrganismPref { get; private set; }

        // Pathfinding 
        public bool Computing { get; set; } = false;
        public List<Tile> Path { get; set; }
        public Tile DestinationTile { get; set; } // The next tile along after the path
        
        // State management
        public States State { get; set; }
        public int MsSinceLastMate { get; set; } = 0;

        // Misc
        public bool IsSelected { get; set; } = false;

        private readonly Healthbar healthbar;

        public Organism(Texture2D[] organismTextures, Tuple<Texture2D, Texture2D> healthbarTextures) : base(organismTextures[Graphics.RANDOM.Next(0, organismTextures.Length - 1)], Graphics.RANDOM.Next(10, 30))
        {
            this.Attributes = new OrganismAttributes(0, 0.2, 500, 50);
            TOTAL_POPULATION++;
            State = States.Roaming;
            Path = new List<Tile>();

            //by default set the organism to be a herbivore
            this.OrganismPref = DietTypes.Herbivore;

            this.healthbar = new Healthbar(healthbarTextures, rectangle, defaultHealth);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsSelected)
            {
                spriteBatch.Draw(this.texture, this.rectangle.Location.ToVector2(), null, Color.Yellow, 0, Vector2.Zero, this.Attributes.Size, SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.Draw(this.texture, this.rectangle.Location.ToVector2(), null, Color.White, 0, Vector2.Zero, this.Attributes.Size, SpriteEffects.None, 0.0f);
            }

            this.healthbar.Draw(spriteBatch);
        }
        
        public void Eat()
        {
            this.Attributes.Hunger += 0.04;
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
        public int Age { get; set; }
        public double Hunger { get; set; }
        public double Speed { get; set; }
        public double Strength { get; set; }
        public int DetectionRadius { get; set; }
        public int DetectionDiameter { get; set; }
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }
        public bool JustMated { get; set; }
        public float Size { get; set; }
        public bool ResistCold { get; set; }
        public bool ResistHeat { get; set; }

        public OrganismAttributes(int age,
                                  double hunger,
                                  double speed,
                                  double strength)
        {
            Species = "Bear";
            DetectionRadius = 3;
            DetectionDiameter = DetectionRadius * 2;
            Age = age;
            Hunger = hunger;
            Speed = speed;
            Strength = strength;
            JustMated = false;
            Size = (Graphics.RANDOM.Next(8) + 3) * 0.1f; // TODO: This should be based off the strength attribute rather than random
            ResistCold = Graphics.RANDOM.NextDouble() >= 0.5; // TODO: This shouldnt be random
            ResistHeat = Graphics.RANDOM.NextDouble() >= 0.5; // TODO: This shouldnt be random
        }
    }
}
