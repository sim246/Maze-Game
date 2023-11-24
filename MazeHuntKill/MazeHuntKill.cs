using Maze;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeHuntKillTests")]
namespace MazeHuntKill;
public class HuntKill : IMapProvider
{
    internal Direction[,] _mazeDirections;
    private readonly int _height;
    private readonly int _width;
    private Random _rnd;

    public HuntKill(int? seed = null)
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
        MapVector vector = v;
        if (dir == Direction.N)
        {
            vector = new MapVector(v.X - 1, v.Y);
        }
        else if (dir == Direction.S)
        {
            vector = new MapVector(v.X + 1, v.Y);
        }
        else if (dir == Direction.E)
        {
            vector = new MapVector(v.X, v.Y + 1);
        }
        else if (dir == Direction.W)
        {
            vector = new MapVector(v.X, v.Y - 1);
        }
        return vector;
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

    internal Direction RandomDirection(MapVector startingMapVector)
    {
        List<Direction> directionList = ValidDirections(startingMapVector);
        if (directionList.Count > 0)
        {
            int ran = _rnd.Next(directionList.Count);
            return directionList[ran];
        }
        else
        {
            return Direction.None;
        }
    }

    internal List<Direction> ValidDirections(MapVector startingMapVector)
    {
        List<Direction> directionList = new() { Direction.N, Direction.S, Direction.E, Direction.W };
        List<Direction> validDirectionList = new() { };
        foreach (Direction direction in directionList)
        {
            MapVector forward = VectorForwardPosition(direction, startingMapVector);
            if (forward.InsideBoundary(_mazeDirections.GetLength(0), _mazeDirections.GetLength(1)) == false &&
                _mazeDirections[forward.X, forward.Y] == Direction.None)
            {
                validDirectionList.Add(direction);
            }
        }
        return validDirectionList;
    }

    private void Walk(MapVector startingMapVector)
    {
        Direction direction = RandomDirection(startingMapVector);
        if (direction != Direction.None)
        {
            Direction oppositeDirection = OppositeDirection(direction);
            MapVector forwardPosition = VectorForwardPosition(direction, startingMapVector);
            if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
            {
                _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | direction;
            }
            else
            {
                _mazeDirections[startingMapVector.X, startingMapVector.Y] = direction;
            }
            _mazeDirections[forwardPosition.X, forwardPosition.Y] = oppositeDirection;
            Walk(forwardPosition);
        }
        Hunt(startingMapVector);
    }

    private void Hunt(MapVector startingMapVector)
    {
        for (int i = 0; i < _mazeDirections.GetLength(0); i++)
        {
            for (int j = 0; j < _mazeDirections.GetLength(1); j++)
            {
                if (_mazeDirections[i, j] == Direction.None)
                {
                    Direction direction = RandomDirection(startingMapVector);
                    if (direction != Direction.None)
                    {
                        Direction oppositeDirection = OppositeDirection(direction);
                        MapVector forwardPosition = VectorForwardPosition(direction, startingMapVector);
                        if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
                        {
                            _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | direction;
                        }
                        else
                        {
                            _mazeDirections[startingMapVector.X, startingMapVector.Y] = direction;
                        }
                        _mazeDirections[forwardPosition.X, forwardPosition.Y] = oppositeDirection;
                        Walk(forwardPosition);
                    }
                }
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