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

        private  StateMachine _fsm;
        public List<Organism> Organisms { get; private set; } = new List<Organism>();
        public List<Food> Foods { get; private set; } = new List<Food>();
        private Random _random = new Random();

        public Brain(Grid grid)
        {
            _fsm = new StateMachine(grid);

        }

        public void Update()
        {

            var foodCount = Foods.Count;
            for(var i = foodCount - 1; i > 0; i--)
            {
                if (_fsm.RemoveFood(Foods[i]))
                {
                    Foods.RemoveAt(i);
                }
            }

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
            Organisms.Add(organism);
            PositionAtRandom(organism, grid);
        }

        public void AddFood(Food food,Grid grid)
        {
            // Keep track of newly added food so we can get them later.
            Foods.Add(food);
            PositionAtRandom(food, grid);
        }
        
        private void PositionAtRandom(MapItem item, Grid grid)
        {
            if (!grid.TryToPosition(item, _random.Next(0, Grid.HorizontalCount), _random.Next(0, Grid.VerticalCount)))
            {
                PositionAtRandom(item, grid); // Try again
            }
        }
    }
}
