using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic.Pathfinding
{
    public static class NodeExpander
    {
        public static List<Node> expand(Tile[][] tiles, Tile center, Tile goal, Node previous)
        {
            var expanded = new List<Node>();
            //get all tiles around the current.
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if(Logic.StateActions.InBounds(center.GridPositionX + i, center.GridPositionY + j))
                    {
                        var node = new Node(tiles[center.GridPositionX + i][center.GridPositionY + j], goal, previous);
                        expanded.Add(node);
                    }

                }
            }
            return expanded;
        }
    }
}
