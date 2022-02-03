namespace Kse.Algorithms.Samples
{
    using System;
    using System.Collections.Generic;

    public class MapPrinter
    {
        public void Print(string[,] maze, List<Point> path)
        {
            PrintTopLine();
            for (var row = 0; row < maze.GetLength(1); row++)
            {
                Console.Write($"{row}\t");
                for (var column = 0; column < maze.GetLength(0); column++)
                {
                    var indexOf = path.IndexOf(new Point(column, row));
                    if (indexOf != -1)
                    {
                        if(indexOf == 0)
                            Console.Write("B");
                        else if(indexOf == path.Count -1)
                            Console.Write("A");
                        else Console.Write(".");
                    }
                    else Console.Write(maze[column, row]);
                }

                Console.WriteLine();
            }

            void PrintTopLine()
            {
                Console.Write($" \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10 == 0 ? i / 10 : " ");
                }

                Console.Write($"\n \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10);
                }

                Console.WriteLine("\n");
            }
        }

        public void PrintPrevious(Point[,] previous, Point start)
        {
            for (var row = 0; row < previous.GetLength(1); row++)
            {
                for (var column = 0; column < previous.GetLength(0); column++)
                {
                    if (start.Column == column && start.Row == row)
                        Console.Write("A");
                    else if (previous[column, row].Row == -1)
                        Console.Write(" ");
                    else if (previous[column, row].Column == column - 1)
                        Console.Write("<");
                    else if (previous[column, row].Column == column + 1)
                        Console.Write(">");
                    else if (previous[column, row].Row == row + 1)
                        Console.Write("v");
                    else if (previous[column, row].Row == row - 1)
                        Console.Write("^");
                }

                Console.WriteLine();
            }
        }
    }
}