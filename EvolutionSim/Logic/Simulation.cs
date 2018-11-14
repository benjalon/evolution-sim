using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic
{
    public class Simulation
    {
        private StateMachine fsm;
        private Grid grid;
        private Random random = new Random();

        public Simulation(Grid grid)
        {
            this.fsm = new StateMachine(grid);
        }

        public void Update()
        {
            foreach (Organism org in grid.Organisms)
            {
                this.fsm.checkState(org);
                this.fsm.determineBehaviour(org);
                this.fsm.UpdateOrganismAttributes(org);
            }
        }
        /// <summary>
        /// Add an inhabitant at a random place in the grid
        /// </summary>
        /// <param name="sprite">the name of the sprite</param>
        /// 
        public void AddOrganism(Organism organism)
        {
            // Keep track of newly added organisms so we can get them later.
            this.grid.AddOrganism(organism);

            organism.DeathOccurred += OrganismDeathHandler;

            PositionAtRandom(organism);
        }

        public void AddFood(Food food)
        {
            // Keep track of newly added food so we can get them later.
            this.grid.AddFood(food);

            food.DeathOccurred += FoodDeathHandler;

            PositionAtRandom(food);
        }
        
        private void PositionAtRandom(MapItem item)
        {
            if (!grid.AttemptToPositionAt(item, random.Next(0, Grid.HorizontalCount), random.Next(0, Grid.VerticalCount)))
            {
                PositionAtRandom(item); // Try again
            }
        }

        private void OrganismDeathHandler(object sender, EventArgs e)
        {
            this.grid.RemoveOrganism((Organism)sender);
        }

        private void FoodDeathHandler(object sender, EventArgs e)
        {
            this.grid.RemoveFood((Food)sender);
        }
    }
}
