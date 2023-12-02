using Maze;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeHuntKillTests")]
namespace MazeHuntKill;
public class HuntKill : IMapProvider
{
    internal Direction[,] _mazeDirections;
    private Random _rnd;
    MapVector _startingMapVector;
    Direction _direction = Direction.None;

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

    internal List<Direction> ValidDirectionsWalk(MapVector startingMapVector)
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

    internal List<Direction> ValidDirectionsHunt(MapVector startingMapVector)
    {
        List<Direction> directionList = new() { Direction.N, Direction.S, Direction.E, Direction.W };
        List<Direction> validDirectionList = new() { };
        foreach (Direction direction in directionList)
        {
            MapVector forward = VectorForwardPosition(direction, startingMapVector);
            if (forward.InsideBoundary(_mazeDirections.GetLength(0), _mazeDirections.GetLength(1)) == false &&
                _mazeDirections[forward.X, forward.Y] != Direction.None)
            {
                validDirectionList.Add(direction);
            }
        }
        return validDirectionList;
    }

    private MapVector Walk(MapVector startingMapVector)
    {
        List<Direction> directionList = ValidDirectionsWalk(startingMapVector);

        if (directionList.Count > 0)
        {
            int ran = _rnd.Next(directionList.Count);
            _direction = directionList[ran];
        }
        else
        {
            _direction = Direction.None;
        }
        if (_direction != Direction.None)
        {
            Direction oppositeDirection = OppositeDirection(_direction);
            MapVector forwardPosition = VectorForwardPosition(_direction, startingMapVector);
            if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
            {
                _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | _direction;
            }
            else
            {
                _mazeDirections[startingMapVector.X, startingMapVector.Y] = _direction;
            }
            if (_mazeDirections[forwardPosition.X, forwardPosition.Y] != Direction.None)
            {
                _mazeDirections[forwardPosition.X, forwardPosition.Y] = _mazeDirections[forwardPosition.X, forwardPosition.Y] | oppositeDirection;
            }
            else
            {
                _mazeDirections[forwardPosition.X, forwardPosition.Y] = oppositeDirection;
            }
            return forwardPosition;
        }
        return startingMapVector;
    }

    private MapVector Hunt(MapVector startingMapVector)
    {
        for (int i = 0; i < _mazeDirections.GetLength(0); i++)
        {
            for (int j = 0; j < _mazeDirections.GetLength(1); j++)
            {
                if (_mazeDirections[i, j] == Direction.None)
                {
                    startingMapVector = new MapVector(i, j);
                    List<Direction> directionList = ValidDirectionsHunt(startingMapVector);
                    if (directionList.Count > 0)
                    {
                        int ran = _rnd.Next(directionList.Count);
                        _direction = directionList[ran];
                    }
                    else
                    {
                        _direction = Direction.None;
                    }
                    Direction oppositeDirection = OppositeDirection(_direction);
                    MapVector forwardPosition = VectorForwardPosition(_direction, startingMapVector);
                    if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
                    {
                        _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | _direction;
                    }
                    else
                    {
                        _mazeDirections[startingMapVector.X, startingMapVector.Y] = _direction;
                    }
                    if (_mazeDirections[forwardPosition.X, forwardPosition.Y] != Direction.None)
                    {
                        _mazeDirections[forwardPosition.X, forwardPosition.Y] = _mazeDirections[forwardPosition.X, forwardPosition.Y] | oppositeDirection;
                    }
                    else
                    {
                        _mazeDirections[forwardPosition.X, forwardPosition.Y] = oppositeDirection;
                    }
                    return forwardPosition;
                }
            }
        }
        return startingMapVector;
    }

    public Direction[,] CreateMap(int width, int height)
    {
        _mazeDirections = new Direction[height, width];
        int x = _rnd.Next(height);
        int y = _rnd.Next(width);
        _startingMapVector = new(x, y);

        Direction[,] dir = CreateMap();

        return dir;
    }

    public Direction[,] CreateMap()
    {

        bool continueHuntKill = true; 
        while (continueHuntKill)
        {
            MapVector newVector = Walk(_startingMapVector);
            if (newVector == _startingMapVector)
            {
                newVector = Hunt(_startingMapVector);
            }
            _startingMapVector = newVector;
            continueHuntKill = false;
            for (int i = 0; i < _mazeDirections.GetLength(0); i++)
            {
                for (int j = 0; j < _mazeDirections.GetLength(1); j++)
                {
                    if (_mazeDirections[i, j] == Direction.None)
                    {
                        continueHuntKill = true;
                    }
                }
            }
        }
        return _mazeDirections;
    }
}