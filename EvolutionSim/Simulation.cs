using System;
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
        private readonly Dictionary<string, Texture2D> textures;
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

        public Simulation(Dictionary<string, Texture2D> textures)
        {
            this.textures = textures;

            this.particleTextures = new List<Texture2D>() { this.textures["star"], this.textures["diamond"], this.textures["circle"] };

            this.bearBreeds = new List<Attributes>()
            {
                new Attributes() { Species = "MiniGreen", Texture = textures["bear_0"], DietType = DietTypes.Herbivore, MaxHealth = 10, Strength = 0.3f, Speed = 0.7f, ResistCold = false, ResistHeat = false },
                new Attributes() { Species = "MysteryPurp", Texture = textures["bear_1"], DietType = DietTypes.Herbivore, MaxHealth = 20, Strength = 0.5f, Speed = 0.6f, ResistCold = true, ResistHeat = false },
                new Attributes() { Species = "Blastoise", Texture = textures["bear_2"], DietType = DietTypes.Omnivore, MaxHealth = 25, Strength = 0.7f, Speed = 0.1f, ResistCold = true, ResistHeat = true },
                new Attributes() { Species = "AngryRed", Texture = textures["bear_3"], DietType = DietTypes.Canivore, MaxHealth = 28, Strength = 0.8f, Speed = 0.2f, ResistCold = false, ResistHeat = true },
                new Attributes() { Species = "YellowBoi", Texture = textures["bear_4"], DietType = DietTypes.Omnivore, MaxHealth = 15, Strength = 0.5f, Speed = 0.5f, ResistCold = true, ResistHeat = true }
            };

            this.healthbarTextures = new Tuple<Texture2D, Texture2D>(textures["healthbar_red"], textures["healthbar_green"]);

            this.background = new FullScreenSprite(textures["grass_background"]);

            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"]);
            this.grid.ShouldSpawnCorpse += SpawnCorpseHandler;

            this.TimeManager = new TimeManager();

            this.fsm = new StateMachine();
            this.fsm.MatingOccurred += this.BirthHandler;

            this.GridInteractionManager = new GridInteractionManager(textures["tile"]);

            this.WeatherOverlay = new WeatherOverlay(textures["cold_overlay"], textures["hot_overlay"]);
        }

        public void Update(GameTime gameTime)
        {
            TimeManager.Update(gameTime);
            GridInteractionManager.Update(this, this.grid);

            if (TimeManager.Paused)
            {
                return;
            }


            Utility.AttributeUpdater.UpdateAttributes(this.grid.Organisms, this.WeatherOverlay.WeatherSetting, TimeManager.HasSimulationTicked, TimeManager);


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

        private Tuple<int, int> MakeUseableValues(float rawX, float rawY)
        {
            var x = (int)(rawX * 10);
            var y = (int)(rawY * 10);
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }

        private Tuple<int, int> MakeUseableValues(int x, int y)
        {
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }

        public void BirthHandler(object sender, EventArgs args)
        {
            var matingArgs = (MatingArgs)args;
            var mother = matingArgs.Mother;
            var father = matingArgs.Father;
            var positioned = false;

            var orderedMaxHealth = MakeUseableValues(mother.Attributes.MaxHealth, father.Attributes.MaxHealth);
            var orderedStrength = MakeUseableValues(mother.Attributes.Strength, father.Attributes.Strength);
            var orderedSpeed = MakeUseableValues(mother.Attributes.Speed, father.Attributes.Speed);

            var simpleCrossbreed = new Attributes()
            {
                Species = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.Species : mother.Attributes.Species,
                Texture = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Texture : mother.Texture,
                DietType = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.DietType : mother.Attributes.DietType,
                MaxHealth = Graphics.RANDOM.Next(orderedMaxHealth.Item1, orderedMaxHealth.Item2),
                Strength = Graphics.RANDOM.Next(orderedStrength.Item1, orderedStrength.Item2) * 0.1f,
                Speed = Graphics.RANDOM.Next(orderedStrength.Item1, orderedStrength.Item2) * 0.1f,
                ResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold,
                ResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat,
            };

            var child = new Organism(simpleCrossbreed, this.healthbarTextures);

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

        public void AddOrganisms(int amount)
        {
            Organism organism;
            for (var i = 0; i < amount; i++)
            {
                organism = new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures);
                PositionAtRandom(organism);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
            }
        }
        public void AddOrganisms(Attributes attributes, int amount)
        {
            Organism organism;
            for (var i = 0; i < amount; i++)
            {
                organism = new Organism(attributes, this.healthbarTextures);
                PositionAtRandom(organism);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
            }



        }

        public void AddFoods(int amount)
        {
            Food food;
            for (var i = 0; i < amount; i++)
            {
                food = new Food(this.textures["food"], true, Graphics.RANDOM.Next(3, Food.MAX_GRASS_HEALTH));
                PositionAtRandom(food);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(food).Center));
            }
        }

        public void AddOrganism(int x, int y)
        {
            var positioned = this.grid.AttemptToPositionAt(new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }

        }

        public void AddFood(int x, int y)
        {
            var positioned = this.grid.AttemptToPositionAt(new Food(this.textures["food"], true, Graphics.RANDOM.Next(1, Food.MAX_GRASS_HEALTH)), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }
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
            var organism = ((Organism)sender);
            var tile = (Tile)grid.GetTileAt(organism);
            this.grid.AttemptToPositionAt(new Food(this.textures["meat"], false, organism.Attributes.MaxHealth), tile.GridIndex.X, tile.GridIndex.Y);
        }
    }
}
