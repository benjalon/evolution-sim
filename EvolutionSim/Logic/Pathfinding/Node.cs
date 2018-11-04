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
            this.Heuristic = CalculateDistance(this.Goal);
            this.Distance = CalculateDistance(previous.Current);
            if (previous == null)
            {
                this.FOfS = 0;
            }
            else
            {
                this.FOfS = this.Heuristic + this.Distance;
            }
            this.Previous = previous;

        }

        private static int CalculateDistance(Tile Location)
        {
            return Math.Abs(Location.GridPositionX - Location.GridPositionX) + Math.Abs(Location.GridPositionY - Location.GridPositionY);
        }

   



    }

  
}
