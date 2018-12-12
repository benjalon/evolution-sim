using System;
using System.Collections.Generic;
using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.UI;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Logic
{
    public class Simulation
    {
        private readonly Dictionary<string, Texture2D> textures;
        private readonly Tuple<Texture2D, Texture2D> healthbarTextures;

        private readonly StateMachine fsm;
        private readonly Grid grid;
        private readonly FullScreenSprite background; 
        
        public DrawingManager TileHighlight { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public WeatherManager WeatherManager { get; private set; }

        public RadioAddSprites SelectedRadioItem { private get; set; } = RadioAddSprites.Grass;

        private List<Breed> bearBreeds;

        public Simulation(Dictionary<string, Texture2D> textures)
        {
            this.textures = textures;

            this.bearBreeds = new List<Breed>()
            {
                new Breed() { Species = "MiniGreen", Texture = textures["bear_0"], DietType = DietTypes.Herbivore, Strength = 0.3f, Speed = 0.7f, ResistCold = false, ResistHeat = false },
                new Breed() { Species = "MysteryPurp", Texture = textures["bear_1"], DietType = DietTypes.Herbivore, Strength = 0.5f, Speed = 0.6f, ResistCold = true, ResistHeat = false },
                new Breed() { Species = "Blastoise", Texture = textures["bear_2"], DietType = DietTypes.Omnivore, Strength = 0.7f, Speed = 0.1f, ResistCold = true, ResistHeat = true },
                new Breed() { Species = "AngryRed", Texture = textures["bear_3"], DietType = DietTypes.Canivore, Strength = 0.8f, Speed = 0.2f, ResistCold = false, ResistHeat = true },
                new Breed() { Species = "YellowBoi", Texture = textures["bear_4"], DietType = DietTypes.Omnivore, Strength = 0.5f, Speed = 0.5f, ResistCold = true, ResistHeat = true }
            };

            this.healthbarTextures = new Tuple<Texture2D, Texture2D>(textures["healthbar_red"], textures["healthbar_green"]);
            
            this.background = new FullScreenSprite(textures["grass_background"]);

            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"]);
            this.grid.ShouldSpawnCorpse += SpawnCorpseHandler;

            this.TimeManager = new TimeManager();

            this.fsm = new StateMachine(this.grid, this.TimeManager);
            this.fsm.MatingOccurred += this.BirthHandler;

            this.TileHighlight = new DrawingManager(textures["tile"]);

            this.WeatherManager = new WeatherManager(textures["cold_overlay"], textures["hot_overlay"]);
        }

        public void Update(GameTime gameTime)
        {
            this.TimeManager.Update(gameTime);
            this.TileHighlight.Update(this, this.grid, SelectedRadioItem);
            
            if (this.TimeManager.Paused)
            {
                return;
            }

            this.WeatherManager.Update(this.grid.Organisms);

            var organismsCount = this.grid.Organisms.Count;
            Organism organism;
            for (var i = organismsCount - 1; i >= 0; i--)
            {
                organism = this.grid.Organisms[i];
                this.fsm.CheckState(organism);
                this.fsm.DetermineBehaviour(organism);
                this.fsm.UpdateOrganismAttributes(organism);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.background.Draw(spriteBatch);
            this.WeatherManager.Draw(spriteBatch);

            this.grid.Draw(spriteBatch);
            
            this.TileHighlight.Draw(spriteBatch);
        }

        private Tuple<int, int> MakeUseableValues(float rawX, float rawY)
        {
            var x = (int)(rawX * 10);
            var y = (int)(rawY * 10);
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(x, y) : new Tuple<int, int>(y, x);
        }

        public void BirthHandler(object sender, EventArgs args)
        {
            var matingArgs = (MatingArgs)args;
            var mother = matingArgs.Mother;
            var father = matingArgs.Father;
            var positioned = false;

            var orderedStrength = MakeUseableValues(mother.Attributes.Strength, father.Attributes.Strength);
            var orderedSpeed = MakeUseableValues(mother.Attributes.Speed, father.Attributes.Speed);

            var simpleCrossbreed = new Breed()
            {
                Species = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.Species : mother.Attributes.Species,
                Texture = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Texture : mother.Texture,
                DietType = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.DietType : mother.Attributes.DietType,
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
            for (var x = 0; x < 3; x++)
            {
                birthSpot.X += x;

                for (var y = 0; y < 3; y++)
                {
                    birthSpot.Y += y;

                    if (this.grid.AttemptToPositionAt(child, birthSpot.X, birthSpot.Y))
                    {
                        return; // We successfully positioned the child so we're done here
                    }
                }
            }

            // We've failed to position the child adjacently so the area must be crowded, just position anywhere on the map
            if (!positioned)
            {
                this.PositionAtRandom(child);
            }
        }
        
        public void AddOrganisms(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures));
            }
        }

        public void AddFoods(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Food(this.textures["food"], true, Graphics.RANDOM.Next(3, 6)));
            }
        }

        public void AddOrganism(int x, int y)
        {
            this.grid.AttemptToPositionAt(new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures), x, y);
        }

        public void AddFood(int x, int y)
        {
            this.grid.AttemptToPositionAt(new Food(this.textures["food"], true, Graphics.RANDOM.Next(3, 6)), x, y);
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
            var tile = (Tile)sender;
            this.grid.AttemptToPositionAt(new Food(this.textures["meat"], false, 20), tile.GridIndex.X, tile.GridIndex.Y);
        }

    }
}
