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
        public double Heuristic { get; }//euclidian(actual) distance from goal.
        public double Distance { get; }//distance from previous node.
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
            }
            else
            {
                this.Distance = CalculateDistance(this.Goal, previous.Current);

                this.FOfS = this.Heuristic + this.Distance;
            }
            this.Previous = previous;

        }

        /// <summary>
        /// WAS LITERALLY SUBTRACTING BY ITS OWN CO-ORDINATE VALUES XD
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Goal"></param>
        /// <returns></returnsL
        private static int CalculateDistance(Tile Goal, Tile Location)
        {
            return Math.Abs(Location.GridPositionX - Goal.GridPositionX) + Math.Abs(Location.GridPositionY - Goal.GridPositionY);
        }

   



    }

  
}
