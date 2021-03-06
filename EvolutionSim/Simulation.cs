﻿using System;
using System.Collections.Generic;
using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Simulation
    {

        private readonly Tuple<Texture2D, Texture2D> healthbarTextures;

        private readonly StateMachine fsm;
        private readonly Grid grid;
        private readonly FullScreenSprite background;

        public TimeManager TimeManager { get; private set; }
        public GridInteractionManager GridInteractionManager { get; private set; }
        public WeatherOverlay WeatherOverlay { get; private set; }

        private readonly List<Texture2D> particleTextures;
        private List<ParticleEffect> particleEffects = new List<ParticleEffect>();

        private readonly List<Attributes> bearBreeds;

        public Simulation()
        {


            this.particleTextures = new List<Texture2D>() { Graphics.SimulationTextures["star"], Graphics.SimulationTextures["diamond"], Graphics.SimulationTextures["circle"] };

            this.bearBreeds = new List<Attributes>()
            {

                new Attributes() { Species = "MiniGreen", Texture = Graphics.SimulationTextures["organism_0"], DietType = DietTypes.Herbivore, MaxHealth = 10, Strength = 0.3f, Speed = 0.7f, Intelligence = 0.3f, ResistCold = false, ResistHeat = false },
                new Attributes() { Species = "MysteryPurp", Texture = Graphics.SimulationTextures["organism_1"], DietType = DietTypes.Herbivore, MaxHealth = 20, Strength = 0.5f, Speed = 0.6f, Intelligence = 0.7f, ResistCold = true, ResistHeat = false },
                new Attributes() { Species = "Blastoise", Texture = Graphics.SimulationTextures["organism_2"], DietType = DietTypes.Omnivore, MaxHealth = 25, Strength = 0.7f, Speed = 0.5f, Intelligence = 0.1f, ResistCold = true, ResistHeat = true },
                new Attributes() { Species = "AngryRed", Texture = Graphics.SimulationTextures["organism_3"], DietType = DietTypes.Canivore, MaxHealth = 28, Strength = 1.0f, Speed = 2.0f, Intelligence = 0.4f, ResistCold = false, ResistHeat = true },
                new Attributes() { Species = "YellowBoi", Texture = Graphics.SimulationTextures["organism_4"], DietType = DietTypes.Omnivore, MaxHealth = 15, Strength = 0.5f, Speed = 0.5f, Intelligence = 0.6f, ResistCold = true, ResistHeat = true }
            };
            //this.bearBreeds = new List<Attributes>()
            //{
            //    new Attributes() { Species = "Smart Boi", Texture = Graphics.SimulationTextures["organism_0"], DietType = DietTypes.Herbivore, MaxHealth = 10, Intelligence = 0.9f, Strength = 0.3f, Speed = 0.4f, ResistCold = false, ResistHeat = false },
            //    new Attributes() { Species = "Strong Boi", Texture = Graphics.SimulationTextures["organism_1"], DietType = DietTypes.Canivore, MaxHealth = 20, Intelligence = 0.1f, Strength = 0.9f, Speed = 0.2f, ResistCold = true, ResistHeat = false },
            //    new Attributes() { Species = "Average Boi", Texture = Graphics.SimulationTextures["organism_2"], DietType = DietTypes.Herbivore, MaxHealth = 10, Intelligence = 0.5f, Strength = 0.5f, Speed = 0.5f, ResistCold = false, ResistHeat = false }

            //};

            this.healthbarTextures = new Tuple<Texture2D, Texture2D>(Graphics.SimulationTextures["healthbar_red"], Graphics.SimulationTextures["healthbar_green"]);

            this.background = new FullScreenSprite(Graphics.SimulationTextures["grass_background"]);

            this.grid = new Grid(Graphics.SimulationTextures["tile"], Graphics.SimulationTextures["mountain"], Graphics.SimulationTextures["water"]);
            this.grid.ShouldSpawnCorpse += SpawnCorpseHandler;

            this.TimeManager = new TimeManager();

            this.fsm = new StateMachine();
            this.fsm.MatingOccurred += this.BirthHandler;

            this.GridInteractionManager = new GridInteractionManager(Graphics.SimulationTextures["tile"]);

            this.WeatherOverlay = new WeatherOverlay(Graphics.SimulationTextures["cold_overlay"], Graphics.SimulationTextures["hot_overlay"]);
        }


        /// <summary>
        /// This is the highest level update method,
        /// which is responsible for calling all of the updates methods associated with
        /// object states which change over time
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            TimeManager.Update(gameTime);
            GridInteractionManager.Update(this, this.grid);

            if (TimeManager.Paused)
            {
                //Graphics.state = GameState.Pause;
                return;
            }
            if (TimeManager.HasGrassTicked)
                this.AddHerbivoreFood();


            Utility.AttributeUpdater.UpdateAttributes(this.grid.Organisms, this.WeatherOverlay.WeatherSetting, TimeManager.HasSimulationTicked, TimeManager);

            this.grid.UpdateFood();

            this.fsm.UpdateStates(this.grid, TimeManager);
            
            for (var i = this.particleEffects.Count - 1; i >= 0; i--)
            {
                this.particleEffects[i].Update(gameTime);

                if (this.particleEffects[i].Complete)
                {
                    this.particleEffects.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Handles the drawing of particle effects when 
        /// spawning in organisms or food
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.background.Draw(spriteBatch);
            this.WeatherOverlay.Draw(spriteBatch);

            this.grid.Draw(spriteBatch);

            this.GridInteractionManager.Draw(spriteBatch);
            
            for (var i = this.particleEffects.Count - 1; i >= 0; i--)
            {
                this.particleEffects[i].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// works by comparing the two floats and creating a new tuple 
        /// where the first element within the tuple is always higher
        /// </summary>
        /// <param name="rawX"></param>
        /// <param name="rawY"></param>
        /// <returns></returns>
        private Tuple<int, int> MakeUseableValues(float rawX, float rawY)
        {
            var x = (int)(rawX * 10);
            var y = (int)(rawY * 10);
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }

        /// <summary>
        /// works by comparing the two ints and creating a new tuple 
        /// where the first element within the tuple is always higher
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Tuple<int, int> MakeUseableValues(int x, int y)
        {
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }


        /// <summary>
        /// This is the event listning for the trigger in state machine
        /// when this is called we create the stats for the new organism
        /// based on the crossover algorithm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void BirthHandler(object sender, EventArgs args)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
            {
                return;
            }


            const float EXTREME = 0.4f;
            const float MIDDLE = 0.2f;
            const float MILD = 0.1f;
   
            var matingArgs = (MatingArgs)args;
            var mother = matingArgs.Mother;
            var father = matingArgs.Father;
            var mutation = matingArgs.Mutation;
            var positioned = false;

            bool newResistHeat = false;
            bool newResistCold = false;
            float newStrength = 0.0f;
            float newSpeed = 0.0f;
            float newIntelligence = 0.0f;

            var orderedMaxHealth = MakeUseableValues(mother.Attributes.MaxHealth, father.Attributes.MaxHealth);
            var orderedStrength = MakeUseableValues(mother.Attributes.Strength, father.Attributes.Strength);
            var orderedSpeed = MakeUseableValues(mother.Attributes.Speed, father.Attributes.Speed);
            var orderIntelligence = MakeUseableValues(mother.Attributes.Intelligence, father.Attributes.Intelligence);


            //this takes into account the stdDeviation from the normal
            //workout an average of the mother and father's attributes
            //then offset the change based on the mutation variation




            #region Handle Mutation
            switch (mutation)
            {
                //organism is not resistant to cold or hot, and is crippled in both strength and speed (it will die fast)
                case MatingArgs.Severity.ExtremelyBad:

                    newResistCold = false;
                    newResistHeat = false;

                    //take an average of the parents strength and speed then take away according to mutation category
                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - EXTREME;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - EXTREME;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) - EXTREME;


                    break;
                case MatingArgs.Severity.MiddleBad:

                    newResistCold = false;
                    newResistHeat = false;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - MIDDLE;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - MIDDLE;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) - MIDDLE;


                    break;

                case MatingArgs.Severity.MildBad:

                    newResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold;
                    newResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - MILD;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - MILD;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) - MILD;



                    break;

                case MatingArgs.Severity.MildGood:

                    newResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold;
                    newResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + MILD;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) + MILD;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) + MILD;


                    break;


                case MatingArgs.Severity.MiddleGood:

                    newResistCold = true;
                    newResistHeat = true;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + MIDDLE;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2) *0.1f /2) + MIDDLE;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) + MIDDLE;


                    break;


                case MatingArgs.Severity.ExtremelyGood:

                    newResistCold = true;
                    newResistHeat = true;


                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + EXTREME;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) + EXTREME;
                    newIntelligence = ((orderIntelligence.Item1 + orderIntelligence.Item2) * 0.1f / 2) + EXTREME;
                    break;

                default:
                    break;

            }
            int mute = Graphics.RANDOM.Next(0, 10);
            if (mute < 3)
            {
                switch (WeatherOverlay.WeatherSetting)
                {
                    case WeatherSettings.Hot:
                        newResistHeat = true;
                        break;
                    case WeatherSettings.Cold:
                        newResistHeat = true;
                        break;
                    default:
                        break;
                }
            }

            var advancedCrossBreed = new Attributes()
                    {
                        //randomly pick between the mother and fathers for the following components:
                        Species = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.Species : mother.Attributes.Species,
                        Texture = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Texture : mother.Texture,
                        DietType = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.DietType : mother.Attributes.DietType,

                        MaxHealth = Graphics.RANDOM.Next(orderedMaxHealth.Item1, orderedMaxHealth.Item2),
                        Strength = newStrength,
                        Speed = newSpeed,
                        Intelligence = newIntelligence,
                        ResistCold = newResistCold,
                        ResistHeat = newResistHeat,
                    };

            #endregion

            var child = new Organism(advancedCrossBreed, this.healthbarTextures);

            // Top left corner
            var birthSpot = mother.GridIndex;
            birthSpot.X -= 1;
            birthSpot.Y -= 1;

            // Position the child adjacent to the mother on an empty square
            for (var x = 0; !positioned && x < 3; x++)
            {
                birthSpot.X += x;

                for (var y = 0; !positioned && y < 3; y++)
                {
                    birthSpot.Y += y;

                    if (this.grid.AttemptToPositionAt(child, birthSpot.X, birthSpot.Y))
                    {
                        positioned = true; // We successfully positioned the child so we're done here
                    }
                }
            }

            // We've failed to position the child adjacently so the area must be crowded, just position anywhere on the map
            if (!positioned)
            {
                this.PositionAtRandom(child);
            }

            particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(child).Center));
        }

        /// <summary>
        /// Attempt to randomly add organisms on the screen
        /// will fail if the max cap on the screen is reached
        /// </summary>
        /// <param name="amount"></param>
        public void AddOrganisms(int amount)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;

                Organism organism;
            for (var i = 0; i < amount; i++)
            {
                organism = new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures);
                PositionAtRandom(organism);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
            }
        }

        /// <summary>
        /// Try to add organisms based on a list of precalculated attribute objects 
        /// and with a pre-defined amount
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="amount"></param>
        public void AddOrganisms(List<Attributes> attributes, int amount)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            foreach (var attribute in attributes)
            {
                Organism organism;
                for (var i = 0; i < amount; i++)
                {
                    organism = new Organism(attribute, this.healthbarTextures);
                    PositionAtRandom(organism);
                    particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
                }
            }




        }


        public void AddFoods(int amount)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            Food food;
            for (var i = 0; i < amount; i++)
            {
                food = new Food(Graphics.SimulationTextures["food"], true, Graphics.RANDOM.Next(Food.MAX_GRASS_HEALTH/2, Food.MAX_GRASS_HEALTH));
                PositionAtRandom(food);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(food).Center));
            }
        }

        public void AddOrganism(int x, int y)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            var positioned = this.grid.AttemptToPositionAt(new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }

        }
        /// <summary>
        /// Adds food in a specified location
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddHerbivoreFood(int x, int y)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            var positioned = this.grid.AttemptToPositionAt(new Food(Graphics.SimulationTextures["food"], true, Graphics.RANDOM.Next(Food.MAX_GRASS_HEALTH/2, Food.MAX_GRASS_HEALTH)), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }
        }
        /// <summary>
        /// Simply adds a random bit of food in an available location, does not require coordinates
        /// </summary>
        public void AddHerbivoreFood()
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            Food food = new Food(Graphics.SimulationTextures["food"], true, Graphics.RANDOM.Next(Food.MAX_GRASS_HEALTH / 2, Food.MAX_GRASS_HEALTH));
            PositionAtRandom(food);
            particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(food).Center));
        }

        private void PositionAtRandom(GridItem item)
        {
            if (!this.grid.AttemptToPositionAt(item, Graphics.RANDOM.Next(0, Grid.TILE_COUNT_X), Graphics.RANDOM.Next(0, Grid.TILE_COUNT_Y)))
            {
                PositionAtRandom(item); // Try again
            }
        }
        
        /// <summary>
        /// Handle death by removing the organism from the grid and removing its reference from the list of organisms.
        /// </summary>
        /// <param name="sender">The organism in question.</param>
        /// <param name="e">Event arguments.</param>
        private void SpawnCorpseHandler(object sender, EventArgs e)
        {
            if (GridItem.TOTAL_GRID_ITEMS > GridItem.MAX_GRID_ITEMS)
                return;
            var organism = ((Organism)sender);
            var tile = (Tile)grid.GetTileAt(organism);
            this.grid.AttemptToPositionAt(new Food(Graphics.SimulationTextures["meat"], false, Graphics.RANDOM.Next(Food.MAX_MEAT_HEALTH / 2, Food.MAX_MEAT_HEALTH)), tile.GridIndex.X, tile.GridIndex.Y);
        }
    }
}
