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
        private Sprite highlight;
        private bool shouldHighlightTile = false;

        private Random random = new Random();

        public Simulation(Dictionary<string, Texture2D> textures, int screenWidth, int screenHeight)
        {
            this.textures = textures;
            this.bearTextures = new Texture2D[] { textures["bear_0"], textures["bear_1"], textures["bear_2"], textures["bear_3"], textures["bear_4"] };

            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"], screenWidth - Overlay.PANEL_WIDTH, screenHeight);

            this.fsm = new StateMachine(this.grid);
            this.fsm.MatingOccurred += this.CreateOrganismHandler;

            this.highlight = new Sprite(textures["tile"], new Rectangle(0, 0, Tile.TILE_SIZE, Tile.TILE_SIZE));
        }

        public void Update()
        {
            this.mouseManager.Update();

            var mouseXIndex = this.mouseManager.PositionX / Tile.TILE_SIZE;
            var mouseYIndex = this.mouseManager.PositionY / Tile.TILE_SIZE;
            this.shouldHighlightTile = Grid.InBounds(mouseXIndex, mouseYIndex);
            if (this.shouldHighlightTile)
            {
                this.grid.HighlightTileAt(mouseXIndex, mouseYIndex);
                this.highlight.SetScreenPosition(this.grid.HighlightedTile.ScreenPositionX, this.grid.HighlightedTile.ScreenPositionY);
            }

            foreach (Organism org in this.grid.Organisms)
            {
                this.fsm.checkState(org);
                this.fsm.determineBehaviour(org);
                this.fsm.UpdateOrganismAttributes(org);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.grid.Draw(spriteBatch);

            if (this.shouldHighlightTile)
            {
                this.highlight.Draw(spriteBatch);
            }
        }
        
        public void CreateOrganismHandler(object sender, EventArgs e)
        {
            //var organism = ((Organism)sender);
            //this.AddOrganism(1);
            //this.grid.AttemptToPositionAt(new Organism(this.bearTextures), organism.GridPosition.X - 1, organism.GridPosition.Y - 1);
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

        public void AddMountain(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                this.grid.SetTerrainAt(TerrainTypes.Mountain, this.random.Next(0, Grid.TileCountX), this.random.Next(0, Grid.TileCountY));
            }
        }

        public void AddWater(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                this.grid.SetTerrainAt(TerrainTypes.Water, this.random.Next(0, Grid.TileCountX), this.random.Next(0, Grid.TileCountY));
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
