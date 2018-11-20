using EvolutionSim.TileGrid.GridItems;
using System;

namespace EvolutionSim.Pathfinding
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
                this.Difficulty = previous.Difficulty + current.MovementDifficulty;

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
            return Math.Max(Math.Abs(Location.TileIndex.X - Goal.TileIndex.X), Math.Abs(Location.TileIndex.Y - Goal.TileIndex.Y));
        }

   



    }

  
}
