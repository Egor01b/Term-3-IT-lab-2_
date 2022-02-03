using System.ComponentModel.DataAnnotations;

namespace Kse.Algorithms.Samples;

using System.Collections.Generic;

public class PathFinder
{
    public List<Point> GetShortestPath(string[,] map, Point start, Point goal)
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

            unvisited.Remove(unvisitedMin);
            var tempDistance = 0;
            var offsets = new Point[]
            {
                new Point(0, 1),
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, -1)
            };
            foreach (var offset in offsets)
            {
                var neighbor = new Point(unvisitedMin.Column + offset.Column, unvisitedMin.Row + offset.Row);
                if (neighbor.Column == -1 || neighbor.Row == -1)
                    continue;
                if (neighbor.Column == distances.GetLength(0) ||
                    neighbor.Row == distances.GetLength(1))
                    continue;
                if (distances[neighbor.Column, neighbor.Row] == -1) continue;
                if (unvisited.IndexOf(neighbor) == -1) continue;
                tempDistance = unvisitedMinValue + 1;
                if (tempDistance < distances[neighbor.Column, neighbor.Row])
                {
                    distances[neighbor.Column, neighbor.Row] = tempDistance;
                    previous[neighbor.Column, neighbor.Row] = unvisitedMin;
                }
            }
        }
        new MapPrinter().PrintPrevious(previous, start);
        var path = NavigateArray(new Point(start.Column + 1, start.Row), goal, previous);
        if (path != null)
            return path;
        path = NavigateArray(new Point(start.Column, start.Row + 1), goal, previous);
        if (path != null)
            return path;
        path = NavigateArray(new Point(start.Column - 1, start.Row), goal, previous);
        if (path != null)
            return path;
        path = NavigateArray(new Point(start.Column, start.Row - 1), goal, previous);
        return path;
    }

    private List<Point> NavigateArray(Point start, Point goal, Point[,] previous)
    {
        if (start.Column == -1 || start.Row == -1) return null;
        if (start.Column == previous.GetLength(0) || start.Row == previous.GetLength(1)) return null;
        var shortestPath = new List<Point>();
        var current = start;
        while (current.Column != goal.Column || current.Row != goal.Row)
        {
            shortestPath.Add(current);
            current = previous[current.Column, current.Row];
            if (current.Column == -1)
                return null;
        }
        shortestPath.Add(current);
        return shortestPath;
    }
}