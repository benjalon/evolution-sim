using System.Collections.Generic;

namespace EvolutionSim.TileGrid.Pathfinding
{
    public static class NodeExpander
    {
        public static List<Node> Expand(Grid grid, Tile center, Tile goal, Node previous)
        {
            var expanded = new List<Node>();
            Node node;
            //get all tiles around the current.
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if(grid.InBounds(center.GridIndex.X + i, center.GridIndex.Y + j) )
                    {

                        node = new Node(grid.GetTileAt(center.GridIndex.X + i, center.GridIndex.Y + j), goal, previous);
                        if (node.Current.GridIndex != center.GridIndex)
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
