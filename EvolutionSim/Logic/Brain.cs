using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic
{
    public class Brain
    {

        private StateMachine _fsm;
        public List<Organism> Organisms { get; private set; } = new List<Organism>();
        public List<Food> Foods { get; private set; } = new List<Food>();
        private Random _random = new Random();

        public Brain(Grid grid)
        {
            _fsm = new StateMachine(grid);
        }

        public void Update()
        {
            foreach (Organism org in Organisms)
            {
                _fsm.checkState(org);
                _fsm.determineBehaviour(org);
                _fsm.UpdateOrganismAttributes(org);
            }
        }
        /// <summary>
        /// Add an inhabitant at a random place in the grid
        /// </summary>
        /// <param name="sprite">the name of the sprite</param>
        /// 
        public void AddOrganism(Organism organism,Grid grid)
        {
            // Keep track of newly added organisms so we can get them later.
            organism.DeathOccurred += OrganismDeathHandler;
            Organisms.Add(organism);
            PositionAtRandom(organism, grid);
        }

        public void AddFood(Food food,Grid grid)
        {
            // Keep track of newly added food so we can get them later.
            food.DeathOccurred += FoodDeathHandler;
            Foods.Add(food);
            PositionAtRandom(food, grid);
        }
        
        private void PositionAtRandom(MapItem item, Grid grid)
        {
            if (!grid.AttemptToPositionAt(item, _random.Next(0, Grid.HorizontalCount), _random.Next(0, Grid.VerticalCount)))
            {
                PositionAtRandom(item, grid); // Try again
            }
        }

        private void OrganismDeathHandler(object sender, EventArgs e)
        {
            Organisms.Remove((Organism)sender);
        }

        private void FoodDeathHandler(object sender, EventArgs e)
        {
            Foods.Remove((Food)sender);
        }
    }
}
