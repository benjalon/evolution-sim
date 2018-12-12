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
        private readonly Texture2D[] bearTextures;
        private readonly Tuple<Texture2D, Texture2D> healthbarTextures;

        private readonly StateMachine fsm;
        private readonly Grid grid;
        private readonly FullScreenSprite background; 
        
        public TileHighlight TileHighlight { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public WeatherManager WeatherManager { get; private set; }

        public RadioAddSprites SelectedRadioItem { private get; set; } = RadioAddSprites.Grass;

        public Simulation(Dictionary<string, Texture2D> textures)
        {
            this.textures = textures;
            this.bearTextures = new Texture2D[] { textures["bear_0"], textures["bear_1"], textures["bear_2"], textures["bear_3"], textures["bear_4"] };
            this.healthbarTextures = new Tuple<Texture2D, Texture2D>(textures["healthbar_red"], textures["healthbar_green"]);
            
            this.background = new FullScreenSprite(textures["grass_background"]);

            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"]);
            this.TimeManager = new TimeManager();

            this.fsm = new StateMachine(this.grid, this.TimeManager);
            this.fsm.MatingOccurred += this.BirthHandler;

            this.TileHighlight = new TileHighlight(textures["tile"]);

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
        
        public void BirthHandler(object sender, EventArgs args)
        {
            var mother = ((MatingArgs)args).Mother;
            var positioned = false;

            var child = new Organism(this.bearTextures, this.healthbarTextures);

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
                PositionAtRandom(new Organism(this.bearTextures, this.healthbarTextures));
            }
        }

        public void AddFoods(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Food(this.textures["food"]));
            }
        }

        public void AddOrganism(int x, int y)
        {
            this.grid.AttemptToPositionAt(new Organism(this.bearTextures, this.healthbarTextures), x, y);
        }

        public void AddFood(int x, int y)
        {
            this.grid.AttemptToPositionAt(new Food(this.textures["food"]), x, y);
        }

        private void PositionAtRandom(GridItem item)
        {
            if (!this.grid.AttemptToPositionAt(item, Graphics.RANDOM.Next(0, Grid.TILE_COUNT_X), Graphics.RANDOM.Next(0, Grid.TILE_COUNT_Y)))
            {
                PositionAtRandom(item); // Try again
            }
        }
    }
}
