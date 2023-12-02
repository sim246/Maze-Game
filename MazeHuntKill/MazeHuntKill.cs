using Maze;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeHuntKillTests")]
namespace MazeHuntKill;
public class HuntKill : IMapProvider
{
    internal Direction[,] _mazeDirections;
    private Random _rnd;
    MapVector _startingMapVector;
    MapVector _forwardPosition;
    Direction _oppositeDirection;
    List<Direction> _directionList;

    public HuntKill(int? seed = null)
    {
        if (seed != null)
        {
            _rnd = new Random((int)seed);
            _directionList = new() { Direction.N, Direction.S, Direction.E, Direction.W };
        }
        else
        {
            _rnd = new Random();
            _directionList = new() { Direction.N, Direction.S, Direction.E, Direction.W };
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
        List<Direction> validDirectionList = new() { };
        foreach (Direction direction in _directionList)
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
        List<Direction> validDirectionList = new() { };
        foreach (Direction direction in _directionList)
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

    private MapVector Walk()
    {
        List<Direction> directionList = ValidDirectionsWalk(_startingMapVector);
        if (directionList.Count > 0)
        {
            int ran = _rnd.Next(directionList.Count);
            Direction direction = directionList[ran];
            _oppositeDirection = OppositeDirection(direction);
            _forwardPosition = VectorForwardPosition(direction, _startingMapVector);
            if (_mazeDirections[_startingMapVector.X, _startingMapVector.Y] != Direction.None)
            {
                _mazeDirections[_startingMapVector.X, _startingMapVector.Y] = _mazeDirections[_startingMapVector.X, _startingMapVector.Y] | direction;
            }
            else
            {
                _mazeDirections[_startingMapVector.X, _startingMapVector.Y] = direction;
            }
            if (_mazeDirections[_forwardPosition.X, _forwardPosition.Y] != Direction.None)
            {
                _mazeDirections[_forwardPosition.X, _forwardPosition.Y] = _mazeDirections[_forwardPosition.X, _forwardPosition.Y] | _oppositeDirection;
            }
            else
            {
                _mazeDirections[_forwardPosition.X, _forwardPosition.Y] = _oppositeDirection;
            }
            return _forwardPosition;
        }
        return _startingMapVector;
    }

    private MapVector Hunt()
    {
        for (int i = 0; i < _mazeDirections.GetLength(0); i++)
        {
            for (int j = 0; j < _mazeDirections.GetLength(1); j++)
            {
                if (_mazeDirections[i, j] == Direction.None)
                {
                    _startingMapVector = new MapVector(i, j);
                    List<Direction> directionList = ValidDirectionsHunt(_startingMapVector);
                    if (directionList.Count > 0)
                    {
                        int ran = _rnd.Next(directionList.Count);
                        Direction direction = directionList[ran];
                        _oppositeDirection = OppositeDirection(direction);
                        _forwardPosition = VectorForwardPosition(direction, _startingMapVector);
                        if (_mazeDirections[_startingMapVector.X, _startingMapVector.Y] != Direction.None)
                        {
                            _mazeDirections[_startingMapVector.X, _startingMapVector.Y] = _mazeDirections[_startingMapVector.X, _startingMapVector.Y] | direction;
                        }
                        else
                        {
                            _mazeDirections[_startingMapVector.X, _startingMapVector.Y] = direction;
                        }
                        if (_mazeDirections[_forwardPosition.X, _forwardPosition.Y] != Direction.None)
                        {
                            _mazeDirections[_forwardPosition.X, _forwardPosition.Y] = _mazeDirections[_forwardPosition.X, _forwardPosition.Y] | _oppositeDirection;
                        }
                        else
                        {
                            _mazeDirections[_forwardPosition.X, _forwardPosition.Y] = _oppositeDirection;
                        }
                        return _forwardPosition;
                    }
                }
            }
        }
        return _startingMapVector;
    }

    public Direction[,] CreateMap(int width, int height)
    {
        _mazeDirections = new Direction[height, width];
        int x = _rnd.Next(height);
        int y = _rnd.Next(width);
        _startingMapVector = new(x, y);

        bool continueHuntKill = true;
        while (continueHuntKill)
        {
            MapVector newVector = Walk();
            while (newVector == _startingMapVector)
            {
                newVector = Hunt();
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

    public Direction[,] CreateMap()
    {
        throw new NotImplementedException();
    }
}