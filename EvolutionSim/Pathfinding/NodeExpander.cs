using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using System.Collections.Generic;

namespace EvolutionSim.Pathfinding
{
    public static class NodeExpander
    {
        public static List<Node> expand(Grid grid, Tile center, Tile goal, Node previous)
        {
            var expanded = new List<Node>();
            Node node;
            //get all tiles around the current.
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if(Grid.InBounds(center.GridPositionX + i, center.GridPositionY + j) )
                    {

                        node = new Node(grid.GetTileAt(center.GridPositionX + i, center.GridPositionY + j), goal, previous);
                        if (node.Current.GridPosition != center.GridPosition)
                        {

                           
                                expanded.Add(node);
                            
                        }
                    }

                }
            }

            return expanded;
        }
    }
}
