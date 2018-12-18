using System.Collections.Generic;
using System.Linq;

namespace EvolutionSim.TileGrid.Pathfinding
{
    public static class PathFinding
    {
        private const int OPEN_LIMIT = 100; // We can get stuck calculating 10k+ nodes when too many organisms group in a corner, better to just let them sit and slowly shuffle away/starve it out

        /// <summary>
        /// Adadpted from pseudocode at https://www.geeksforgeeks.org/a-search-algorithm/
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="grid"></param>
        /// <returns>List of Tiles leading from start to end positions.</returns>
        public static List<Tile> FindShortestPath(Tile startPosition, Tile endPosition, Grid grid)
        {
            Node goalNode = null;

            // A* Search Algorithm
            //1.  Initialize the open list
            var open = new List<Node>();

            //2.  Initialize the closed list
            var closed = new List<Node>();

            //    put the starting node on the open 
            //    list (you can leave its f at zero)
            var startNode = new Node(startPosition, endPosition, null);
            open.Add(startNode);

            Node current;
            List<Node> expanded;
            //3.  while the open list is not empty
            while (open.Count > 0 && goalNode == null && open.Count < OPEN_LIMIT)
            {
                //    a) find the node with the least f on 
                //       the open list, call it current
                open = open.OrderBy(x => x.FOfS).ToList();//custom sort by heuristic (distance to goal)


                //    b) pop current off the open list
                current = open[0];
                open.Remove(current);
                
                //    c) generate currents's 8 successors and set their 
                //       parents to current
                expanded = NodeExpander.Expand(grid, current.Current, current.Goal, current);
                //open.AddRange(expanded);

                Node node;
                var expandedCount = expanded.Count;
                for (var i = 0; i < expandedCount; i++)
                {
                    node = expanded[i];

                    if (node.Current == node.Goal)
                    {
                        goalNode = node;
                        break;
                    }
                    else if (node.Current.HasInhabitant)
                    {
                        continue;
                    }
                    else
                    {
                        //            if a node with the same position as 
                        //            successor is in the OPEN list which has a 
                        //            lower f than successor, skip this successor

                        //            if a node with the same position as 
                        //            successor  is in the CLOSED list which has
                        //            a lower f than successor, skip this successor
                        //            otherwise, add  the node to the open list
                        // if (open.Exists(x => x.Current == node.Current) || closed.Exists(x => x.Current == node.Current))   
                        if (!closed.Exists(x => x.Current == node.Current))
                        {
                            open.Add(node);
                        }
                            
                    }
                }
                //    push current on the closed list
                closed.Add(current);
            }
            var path = new List<Tile>();
            Node t = goalNode;
            if (goalNode != null)
            {
                while (true)
                {
                    path.Add(t.Current);
                    if (t.Current == startPosition)
                    {
                        break;
                    }
                    t = t.Previous;
                }
                path.Reverse();
                // Remove first tile as its occupied by the organism being moved
                path.RemoveAt(0);


            }
            return path;
        }
    }
}
