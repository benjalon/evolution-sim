using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic.Pathfinding
{
    public class Node
    {
        public Tile Current { get; }
        public Tile Goal { get; }
        public double Heuristic { get; }//diagonal(actual) distance from goal.
        public double Difficulty { get; }//.
        public double FOfS { get; }//heuristic + distance
        public Node Previous { get; }

        public Node(Tile current, Tile goal, Node previous)
        {
            this.Current = current;
            this.Goal = goal;
            this.Heuristic = CalculateDistance(this.Goal, this.Current);
            if (previous == null)
            {
                this.FOfS = 0;
                this.Difficulty = 0;
            }
            else
            {
                var terrainDifficulty = 1; //this should be taken from the tile terrain type.
                //difficulty = difficulty of previous tile + terrain difficulty. 
                this.Difficulty = previous.Difficulty + terrainDifficulty;

                this.FOfS = this.Heuristic + this.Difficulty;
            }
            this.Previous = previous;

        }

        /// <summary>
        /// Provides diagonal distance between two Tiles. I.e. Max distance of y OR x coordinates. 
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Goal"></param>
        /// <returns>The diagonal distance between two Tiles.</returns>
        private static int CalculateDistance(Tile Goal, Tile Location)
        {
            return Math.Max(Math.Abs(Location.GridPositionX - Goal.GridPositionX), Math.Abs(Location.GridPositionY - Goal.GridPositionY));
        }

   



    }

  
}
