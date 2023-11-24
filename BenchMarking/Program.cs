using Maze;
using MazeHuntKill;
using MazeRecursion;
using System;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        bool notvalid = true;
        int type = 0;
        int height = 0;
        int width = 0;
        TimeSpan timeTaken = TimeSpan.Zero;

        while (notvalid)
        {
            try
            {
                Console.WriteLine("Input size of maze: ");
                Console.WriteLine("Input height: ");
                height = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Input width: ");
                width = Int32.Parse(Console.ReadLine());
                
                if (height > 1 && width > 1)
                {
                    notvalid = false;
                }
            }
            catch
            {
                Console.WriteLine("Maze size invalid!");
            }
        }
        notvalid = true;
        while (notvalid)
        {
            try
            {
                Console.WriteLine("Input type of maze: ");
                Console.WriteLine("MazeHuntKill maze - input 1");
                Console.WriteLine("MazeRecursion maze - input 2");
                type = Int32.Parse(Console.ReadLine());
                if (type == 1)
                {
                    notvalid = false;
                    IMapProvider mazeHuntKill = new HuntKill();
                    IMap bench = new Map(mazeHuntKill);
                    timeTaken = TimeIt(bench, height, width);
                }
                else if (type == 2)
                {
                    notvalid = false;
                    IMapProvider mazeRecursive = new MazeRecursive();
                    IMap bench = new Map(mazeRecursive);
                    timeTaken = TimeIt(bench, height, width);
                }
            }
            catch
            {
                Console.WriteLine("Type of maze invalid!");
            }
        }
        WriteToFile("C:\\Users\\2133009\\Downloads\\dragomirassignment5\\testdoc.txt", timeTaken.ToString(@"m\:ss\.fff"));
    }

    public static TimeSpan TimeIt(IMap bench, int height, int width)
    {
        var timer = new Stopwatch();
        timer.Start();
        bench.CreateMap(height, width);
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        return timeTaken;
    }

    public static void WriteToFile(string path, string data)
    {
        File.AppendAllText(@path, data + Environment.NewLine);
    }
}