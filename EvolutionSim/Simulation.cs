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
        private Dictionary<string, Texture2D> textures;
        private Texture2D[] bearTextures;

        private StateMachine fsm;
        private Grid grid;

        private MouseManager mouseManager = new MouseManager();
        private TileHighlight tileHighlight;

        public TerrainTypes SelectedTerrainType { private get; set; } = TerrainTypes.Grass;

        private Random random = new Random();

        public Simulation(Dictionary<string, Texture2D> textures, int screenWidth, int screenHeight)
        {
            this.textures = textures;
            this.bearTextures = new Texture2D[] { textures["bear_0"], textures["bear_1"], textures["bear_2"], textures["bear_3"], textures["bear_4"] };

            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"], screenWidth - Overlay.PANEL_WIDTH, screenHeight);

            this.fsm = new StateMachine(this.grid);
            this.fsm.MatingOccurred += this.CreateOrganismHandler;

            this.tileHighlight = new TileHighlight(textures["tile"]);
        }

        public void Update()
        {
            this.mouseManager.Update();

            this.tileHighlight.Update(this.mouseManager, this.grid, SelectedTerrainType);

            var organismsCount = this.grid.Organisms.Count;
            Organism organism;
            for (var i = 0; i < organismsCount; i++)
            {
                organism = this.grid.Organisms[i];
                this.fsm.checkState(organism);
                this.fsm.determineBehaviour(organism);
                this.fsm.UpdateOrganismAttributes(organism);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.grid.Draw(spriteBatch);

            this.tileHighlight.Draw(spriteBatch);
        }
        
        public void CreateOrganismHandler(object sender, EventArgs args)
        {
            var mother = ((MatingArgs)args).Mother;
            var positioned = false;

            var child = new Organism(this.bearTextures);

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
        
        public void AddOrganism(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Organism(this.bearTextures));
            }
        }

        public void AddFood(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Food(this.textures["food"]));
            }
        }

        private void PositionAtRandom(GridItem item)
        {
            if (!this.grid.AttemptToPositionAt(item, this.random.Next(0, Grid.TileCountX), this.random.Next(0, Grid.TileCountY)))
            {
                PositionAtRandom(item); // Try again
            }
        }
    }
}
