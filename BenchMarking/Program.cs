using Maze;
using MazeHuntKill;
using MazeRecursion;
using System;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
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
}