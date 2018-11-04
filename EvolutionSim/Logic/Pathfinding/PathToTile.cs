using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Logic.Pathfinding
{
    public static class PathFinding
     {
        public static List<Tile> FindShortestPath(Tile startPosition, Tile endPosition,Tile[][] tiles)
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

            bool goalFound = false;
            //3.  while the open list is not empty
            while (open.Any() && !goalFound)
            {
                //    a) find the node with the least f on 
                //       the open list, call it current
                open = open.OrderBy(x => x.FOfS).ToList();//custom sort by heuristic (distance to goal)


                //    b) pop current off the open list
                var current = open.First();
                open.Remove(current);

                //    c) generate currents's 8 successors and set their 
                //       parents to current
                var expanded = Pathfinding.NodeExpander.expand(tiles, current.Current, current.Goal, current);
                //open.AddRange(expanded);

                foreach (var node in expanded)
                {
                    if (node.Current.GridPositionX == node.Goal.GridPositionX && node.Current.GridPositionY == node.Goal.GridPositionY)
                    {
                        goalNode = node;
                        goalFound = true;
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
                        var openCheck = open.Exists(x => x.Current == node.Current && x.FOfS > node.FOfS);
                        var closedCheck = closed.Exists(x => x.Current == node.Current && x.FOfS > node.FOfS);
                        if (openCheck || closedCheck)
                        {
                            break;
                        }
                        else
                        {
                            open.Add(node);
                        }
                    }
                }
                //    push current on the closed list
                closed.Add(current);
            }
            var endTile = false;
            var path = new List<Tile>();
            Node t = goalNode;
            if (goalFound)
            {
                while (!endTile)
                {
                    path.Add(t.Current);
                    if(t.Current.GridPositionX == startPosition.GridPositionX && t.Current.GridPositionY == startPosition.GridPositionY)
                    {
                        endTile = true;
                    }
                    t = t.Previous;
                }
                path.Reverse();
                // Remove first tile as its occupied by the organism being moved
                path.RemoveAt(0);
                // Remove last tile as its occupied by whatever the hell it's going to
                path.RemoveAt(path.Count-1);


            }

            return path;
        }
    }
}
