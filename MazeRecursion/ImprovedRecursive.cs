using Maze;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeRecursionTests")]
namespace BenchMarking
{
    public class ImprovedRecursive
    {
        internal Direction[,] _mazeDirections;
        private Random _rnd;
        List<Direction> _directionList;
        Direction _oppositeDirection;
        MapVector _forwardPosition;

        public ImprovedRecursive(int? seed = null)
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

        private void Walk(MapVector startingMapVector)
        {
            _directionList = _directionList.OrderBy(a => _rnd.Next()).ToList();

            foreach (Direction dir in _directionList)
            {
                _oppositeDirection = OppositeDirection(dir);
                _forwardPosition = VectorForwardPosition(dir, startingMapVector);

                if (_forwardPosition.InsideBoundary(_mazeDirections.GetLength(0), _mazeDirections.GetLength(1)) == false &&
                    _mazeDirections[_forwardPosition.X, _forwardPosition.Y] == Direction.None)
                {
                    if (_mazeDirections[startingMapVector.X, startingMapVector.Y] != Direction.None)
                    {
                        _mazeDirections[startingMapVector.X, startingMapVector.Y] = _mazeDirections[startingMapVector.X, startingMapVector.Y] | dir;
                    }
                    else
                    {
                        _mazeDirections[startingMapVector.X, startingMapVector.Y] = dir;
                    }
                    _mazeDirections[_forwardPosition.X, _forwardPosition.Y] = _oppositeDirection;
                    Walk(_forwardPosition);
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
}
