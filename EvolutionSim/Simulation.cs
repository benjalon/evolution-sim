using System;
using System.Collections.Generic;
using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Logic
{
    public class Simulation
    {
        private Dictionary<string, Texture2D> textures;
        private StateMachine fsm;
        private Grid grid;
        private Random random = new Random();

        public Simulation(Dictionary<string, Texture2D> textures, int screenWidth, int screenHeight)
        {
            this.textures = textures;
            this.grid = new Grid(textures["tile"], screenWidth, screenHeight);
            this.fsm = new StateMachine(this.grid);
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

        public void AddOrganism(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Organism(this.textures["face"]));
            }
        }

        public void AddFood(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PositionAtRandom(new Food(this.textures["pizza"]));
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
