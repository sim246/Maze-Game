using Maze;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeRecursionTests")]
namespace MazeRecursion;
public class MazeRecursive : IMapProvider
{
    internal Direction[,] _mazeDirections;
    private Random _rnd;

    public MazeRecursive(int? seed = null)
    {
        if (seed != null)
        {
            _rnd = new Random((int)seed);
        }
        else
        {
            _rnd = new Random();
        }
    }

    internal MapVector VectorForwardPosition(Direction dir, MapVector v)
    {
        if (dir == Direction.N)
        {
            return new MapVector(v.X - 1, v.Y);
        }
        else if (dir == Direction.S)
        {
            return new MapVector(v.X + 1, v.Y);
        }
        else if (dir == Direction.E)
        {
            return new MapVector(v.X, v.Y + 1);
        }
        else if (dir == Direction.W)
        {
            return new MapVector(v.X, v.Y - 1);
        }
        return new MapVector(v.X, v.Y);
    }

    internal Direction OppositeDirection(Direction dir)
    {
        if (dir == Direction.N)
        {
            return Direction.S;
        }
        else if (dir == Direction.S)
        {
            return Direction.N;
        }
        else if (dir == Direction.E)
        {
            return Direction.W;
        }
        else if (dir == Direction.W)
        {
            return Direction.E;
        }
        return Direction.None;
    }

    internal List<Direction> RandomDirectionList()
    {
        List<Direction> directionList = new() { Direction.N, Direction.S, Direction.E, Direction.W };
        int count = directionList.Count;
        while (count > 1)
        {
            int ran = _rnd.Next(count);
            count--;
            Direction tempValue = directionList[ran];
            directionList[ran] = directionList[count];
            directionList[count] = tempValue;
        }
        return directionList;
    }

    private void Walk(MapVector startingMapVector)
    {
        List<Direction> directionList = RandomDirectionList();

        foreach (Direction dir in directionList)
        {
            Direction oppositeDirection = OppositeDirection(dir);
            MapVector forwardPosition = VectorForwardPosition(dir, startingMapVector);

            if (forwardPosition.InsideBoundary(_mazeDirections.GetLength(0), _mazeDirections.GetLength(1)) == false &&
                _mazeDirections[forwardPosition.X, forwardPosition.Y] == Direction.None)
            {
                if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
                {
                    _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | dir;
                }
                else
                {
                    _mazeDirections[startingMapVector.X, startingMapVector.Y] = dir;
                }
                _mazeDirections[forwardPosition.X, forwardPosition.Y] = oppositeDirection;
                Walk(forwardPosition);
            }
        }
    }

    public Direction[,] CreateMap(int width, int height)
    {
        _mazeDirections = new Direction[height, width];
        int x = _rnd.Next(height);
        int y = _rnd.Next(width);
        MapVector startingMapVector = new(x, y);

        Walk(startingMapVector);
        return _mazeDirections;
    }

    public Direction[,] CreateMap()
    {
        throw new NotImplementedException();
    }
}