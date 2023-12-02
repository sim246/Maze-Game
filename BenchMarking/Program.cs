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
        string theType = "";
        int height = 0;
        int width = 0;
        int increase = 0;
        int iterations = 0;
        TimeSpan timeTaken = TimeSpan.Zero;

        while (notvalid)
        {
            try
            {
                Console.WriteLine("Input type of maze: ");
                Console.WriteLine("MazeHuntKill maze - input 1");
                Console.WriteLine("MazeRecursion maze - input 2");
                type = Int32.Parse(Console.ReadLine());
                if (type == 1 || type == 2)
                {
                    notvalid = false;
                }
            }
            catch
            {
                Console.WriteLine("Type of maze invalid!");
            }
        }
        notvalid = true;
        while (notvalid)
        {
            try
            {
                Console.WriteLine("Input number of iterations and size increase: ");
                Console.WriteLine("Iterations: ");
                iterations = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Size Increase: ");
                increase = Int32.Parse(Console.ReadLine());
                
                if (iterations > 0 || increase > 0)
                {
                    notvalid = false;
                }
            }
            catch
            {
                Console.WriteLine("Input not invalid!");
            }
        }

        notvalid = true;
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

        while (iterations > 0)
        {
            if (type == 1)
            {
                IMapProvider mazeHuntKill = new HuntKill();
                theType = "Hunt Kill";
                timeTaken = TimeIt(() =>
                {
                    mazeHuntKill.CreateMap(width, height);
                });
            }
            else if (type == 2)
            {
                IMapProvider mazeRecursive = new MazeRecursive();
                theType = "Recursive";
                timeTaken = TimeIt(() =>
                {
                    mazeRecursive.CreateMap(width, height);
                });
            }
            Console.WriteLine(theType + " " + height + "x" + width + ": " + timeTaken.ToString(@"mm\:ss\.fff"));
            WriteToFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.ToString() + "\\timer.txt", theType + " " + height + "x" + width + ": " + timeTaken.ToString(@"mm\:ss\.fff"));
            height += increase;
            width += increase;
            iterations--;
        }
    }

    public static TimeSpan TimeIt(Action bench)
    {
        var timer = new Stopwatch();
        timer.Start();
        bench.Invoke();
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        return timeTaken;
    }

    public static void WriteToFile(string path, string data)
    {
        File.AppendAllText(@path, data + Environment.NewLine);
    }
}