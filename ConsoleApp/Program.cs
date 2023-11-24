using Maze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

internal class Program
{
    private static void Main(string[] args)
    {
        //C:\Users\2133009\Downloads\dragomirassignment2\map5x5.txt
        bool notlocation = true;
        List<string> directions = new List<string>();
        int length = 0;
        int width = 0;
        string url = "";
        while (notlocation)
        {
            try
            {
                Console.WriteLine("Input file path to maze: ");
                url = Console.ReadLine();
                string[] lines = File.ReadAllLines(url, Encoding.UTF8);

                length = lines.Length;
                width = lines[0].Split(',').Length;
                directions = SplitLines(lines, length, width);

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

    public static List<string> SplitLines(string[] lines, int length, int width)
    {
        List<string> directions = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            for (int j = 0; j < line.Length; j++)
            {
                directions.Add(line[j]);
            }
        }
        return directions;
    }
}