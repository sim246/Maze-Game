using Maze;
using MazeHuntKill;
using MazeRecursion;
using System;
using System.Diagnostics;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        WriteToFile("C:\\Users\\2133009\\Downloads\\dragomirassignment5\\testdoc.txt", "text");
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