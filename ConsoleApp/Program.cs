using Maze;
using System;
using System.IO;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        //C:\Users\2133009\Downloads\dragomirassignment5\map5x5.txt
        bool notlocation = true;
        string url = "";
        while (notlocation)
        {
            try
            {
                Console.WriteLine("Input file path to maze: ");
                url = Console.ReadLine();
                string[] lines = File.ReadAllLines(url, Encoding.UTF8);
                notlocation = false;
            }
            catch
            {
                Console.WriteLine("Could not find file path to maze!");
            }
        }
        IMapProvider map = new MazeFromFile.MazeFromFile(mapPath: url);
        Map maze = new(map);
        maze.CreateMap();

        for (int i = 0; i < maze.Height; i++)
        {
            for (int j = 0; j < maze.Width; j++)
            {
                if (new MapVector(i, j).Equals(maze.Goal))
                {
                    Console.Write(" # ");
                }
                else if (new MapVector(i, j).Equals(maze.Player.Position))
                {
                    Console.Write(" + ");
                }
                else if (maze.MapGrid[i, j] == Block.Solid)
                {
                    Console.Write("[*]");
                }
                else if (maze.MapGrid[i, j] == Block.Empty)
                {
                    Console.Write("   ");
                }
            }
            Console.WriteLine();
        }

    }
}