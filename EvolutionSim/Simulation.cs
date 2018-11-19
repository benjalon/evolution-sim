using System;
using System.Collections.Generic;
using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.UI;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Logic
{
    public class Simulation
    {
        private Dictionary<string, Texture2D> textures;
        private StateMachine fsm;
        private Grid grid;
        private Random random = new Random();
        private Texture2D[] bearTextures;

        public Simulation(Dictionary<string, Texture2D> textures, int screenWidth, int screenHeight)
        {
            this.textures = textures;
            this.grid = new Grid(textures["tile"], textures["mountain"], textures["water"], screenWidth - Overlay.PANEL_WIDTH, screenHeight);
            this.fsm = new StateMachine(this.grid);
            this.fsm.MatingOccurred += this.CreateOrganismHandler;
            this.bearTextures = new Texture2D[] { textures["bear_0"], textures["bear_1"], textures["bear_2"], textures["bear_3"], textures["bear_4"] };
        }

        public void Update()
        {
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
                this.grid.SetTerrainAt(TerrainTypes.Mountain, this.random.Next(0, Grid.HorizontalCount), this.random.Next(0, Grid.VerticalCount));
            }
        }

        public void AddWater(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                this.grid.SetTerrainAt(TerrainTypes.Water, this.random.Next(0, Grid.HorizontalCount), this.random.Next(0, Grid.VerticalCount));
            }
        }

        private void PositionAtRandom(GridItem item)
        {
            if (!this.grid.AttemptToPositionAt(item, this.random.Next(0, Grid.HorizontalCount), this.random.Next(0, Grid.VerticalCount)))
            {
                PositionAtRandom(item); // Try again
            }
        }
    }
}
