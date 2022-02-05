namespace Kse.Algorithms.Samples;

using System.Collections.Generic;

public class PathFinder
{
    public List<Point> GetShortestPath(string[,] map, Point start, Point goal)
    {
        return GetShortestPathAStar(map, start, goal);
        //return GetShortestPathDjikstra(map, start, goal);
    }
    
    private float GetPointWeight(string value)
    {
        if (value == " ") return 1;
        else return 1 / (60 - (float.Parse(value) - 1) * 6);
    }

    private List<Point> GetShortestPathAStar(string[,] map, Point start, Point goal)
    {
        var heuristic = new Dictionary<Point, float>();
        var distances = new Dictionary<Point, float>();
        var cost = new Dictionary<Point, float>();
        var parent = new Dictionary<Point, Point>();
        var open = new List<Point>();
        var closed = new List<Point>();

        heuristic[start] = 0;
        distances[start] = 0;
        cost[start] = 0;

        open.Add(start);

        while(open.Count != 0)
        {
            var unvisitedMin = open[0];
            var unvisitedMinValue = cost[open[0]];
            for (int i = 1; i < open.Count; i++)
            {
                if (cost[open[i]] < unvisitedMinValue)
                {
                    unvisitedMin = open[i];
                    unvisitedMinValue = cost[open[i]];
                }
            }
            
            if (unvisitedMin.Column == goal.Column && unvisitedMin.Row == goal.Row)
            {
                Console.WriteLine("Travel time: " + distances[goal] + "h");
                return NavigateArray(start, goal, parent);
            }

            open.Remove(unvisitedMin);
            closed.Add(unvisitedMin);

            var offsets = new Point[]
            {
                new Point(0, 1),
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, -1)
            };
            foreach(var offset in offsets)
            {
                var neighbor = new Point(unvisitedMin.Column + offset.Column, unvisitedMin.Row + offset.Row);

                if (neighbor.Column == -1 || neighbor.Row == -1) continue;
                if (neighbor.Column == map.GetLength(0) || neighbor.Row == map.GetLength(1)) continue;

                if (map[neighbor.Column, neighbor.Row] == "█") continue;
                if (closed.Contains(neighbor)) continue;

                if (!open.Contains(neighbor))
                {
                    open.Add(neighbor);
                    heuristic[neighbor] = (Math.Abs(neighbor.Column - unvisitedMin.Column) + Math.Abs(neighbor.Row - unvisitedMin.Column));
                    
                    parent[neighbor] = unvisitedMin;
                    distances[neighbor] = distances[unvisitedMin] + GetPointWeight(map[unvisitedMin.Column, unvisitedMin.Row]);
                    cost[neighbor] = heuristic[neighbor] + distances[neighbor];
                }
                else if (distances[unvisitedMin] < distances[neighbor])
                {
                    parent[neighbor] = unvisitedMin;
                    distances[neighbor] = distances[unvisitedMin] + GetPointWeight(map[unvisitedMin.Column, unvisitedMin.Row]);
                    cost[neighbor] = heuristic[neighbor] + distances[neighbor];
                }
            }
        }
        return new List<Point>();
    }
    
    /*
    private List<Point> GetShortestPathDjikstra(string[,] map, Point start, Point goal)
    {
        var previous = new Point[map.GetLength(0), map.GetLength(1)];
        var distances = new int[map.GetLength(0), map.GetLength(1)];
        var unvisited = new List<Point>();
        for (int i = 0; i < distances.GetLength(0); i++)
        {
            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (map[i, j] == " ")
                {
                    if (i != start.Column || j != start.Row) unvisited.Add(new Point(i, j));
                    distances[i, j] = int.MaxValue;
                }
                else
                {
                    distances[i, j] = -1;
                    previous[i, j] = new Point(-1, -1);
                }
            }
        }

        distances[start.Column, start.Row] = 0;
        var offsets = new Point[]
        {
            new Point(0, 1),
            new Point(1, 0),
            new Point(-1, 0),
            new Point(0, -1)
        };
        while (unvisited.Count > 0)
        {
            var unvisitedMinValue = distances[unvisited[0].Column, unvisited[0].Row];
            var unvisitedMin = unvisited[0];
            for (int i = 1; i < unvisited.Count; i++)
            {
                if (distances[unvisited[i].Column, unvisited[i].Row] < unvisitedMinValue)
                {
                    unvisitedMin = unvisited[i];
                    unvisitedMinValue = distances[unvisited[i].Column, unvisited[i].Row];
                }
            }

            if (unvisitedMinValue == int.MaxValue)
            {
                var foundWIthVisitedNeighbor = false;
                for (int i = 0; i < unvisited.Count; i++)
                {
                    foreach (var offset in offsets)
                    {
                        var neighbor = new Point(unvisited[i].Column + offset.Column, unvisited[i].Row + offset.Row);
                        if (neighbor.Column == -1 || neighbor.Row == -1) continue;
                        if (neighbor.Column == distances.GetLength(0) || neighbor.Row == distances.GetLength(1))
                            continue;
                        if (distances[neighbor.Column, neighbor.Row] != -1 &&
                            distances[neighbor.Column, neighbor.Row] != int.MaxValue)
                        {
                            unvisitedMin = unvisited[i];
                            unvisitedMinValue = distances[unvisitedMin.Column, unvisitedMin.Row];
                            foundWIthVisitedNeighbor = true;
                            break;
                        }
                    }
                    if(foundWIthVisitedNeighbor)
                        break;
                }
            }
            unvisited.Remove(unvisitedMin);
            foreach (var offset in offsets)
            {
                var neighbor = new Point(unvisitedMin.Column + offset.Column, unvisitedMin.Row + offset.Row);
                if (neighbor.Column == -1 || neighbor.Row == -1)
                    continue;
                if (neighbor.Column == distances.GetLength(0) ||
                    neighbor.Row == distances.GetLength(1))
                    continue;
                if (distances[neighbor.Column, neighbor.Row] == -1) continue;
                var tempDistance = unvisitedMinValue + 1;
                if (tempDistance < distances[neighbor.Column, neighbor.Row])
                {
                    distances[neighbor.Column, neighbor.Row] = tempDistance;
                    previous[neighbor.Column, neighbor.Row] = unvisitedMin;
                }
            }
        }

        // Make neighbors of Start point at Start if they don't get written into (anomaly zone?)
        foreach (var offset in offsets)
        {
            var neighbor = new Point(start.Column + offset.Column, start.Row + offset.Row);
            if (neighbor.Column == -1 || neighbor.Row == -1)
                continue;
            if (neighbor.Column == previous.GetLength(0) || neighbor.Row == previous.GetLength(1))
                continue;
            previous[neighbor.Column, neighbor.Row] = start;
        }

        // Print origins of each point as a grid of arrows. For debug
        // new MapPrinter().PrintPrevious(previous, start);
        var path = NavigateArray(start, goal, previous);
        previous[start.Column, start.Row] = start;
        return path;
    }
    */

    private List<Point> NavigateArray(Point start, Point goal, Dictionary<Point, Point> previous)
    {
        var shortestPath = new List<Point>();
        var current = goal;
        while (current.Row != start.Row || current.Column != start.Column)
        {
            shortestPath.Add(current);
            current = previous[current];
            if (current.Column == -1)
                return null;
        }
        shortestPath.Add(current);
        return shortestPath;
    }
}