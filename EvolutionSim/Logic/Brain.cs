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
            organism.GridPosition.X = _random.Next(0, Grid.horizontalCount);
            organism.GridPosition.Y = _random.Next(0, Grid.verticalCount);

            // Add to grid
             grid._tiles[organism.GridPosition.X][organism.GridPosition.Y].AddMapItem(organism);
        }

        public void AddFood(Food food,Grid grid)
        {
            // Keep track of newly added food so we can get them later.
            Foods.Add(food); 
            food.GridPosition.X = _random.Next(0, Grid.horizontalCount);
            food.GridPosition.Y = _random.Next(0, Grid.verticalCount);

            // Add to grid
            grid._tiles[food.GridPosition.X][food.GridPosition.Y].AddMapItem(food);
        }



    }
}
