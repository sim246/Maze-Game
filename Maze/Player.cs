using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeTests")]
namespace Maze
{
    public class Player : IPlayer
    {
        public Direction Facing { get; private set; }

        public MapVector Position { get; private set; }

        public int StartX { get; private set; }

        public int StartY { get; private set; }

        private readonly Block[,] _mapGrid;

        public Player(Block[,] MapGrid, int height, int width)
        {
            _mapGrid = MapGrid;
            PlacePlayer(height, width);
            Facing = Direction.E;
        }

        internal void PlacePlayer(int height, int width, int? seed = null)
        {
            bool place = false;
            Random rnd = new Random();
            if (seed != null)
            {
                rnd = new Random((int)seed);
            }

            while (!place)
            {
                int x = rnd.Next(height);
                int y = rnd.Next(width);
                if (x < height && y < width && _mapGrid[x, y] != Block.Solid)
                {
                    Position = new MapVector(x, y);
                    StartX = x;
                    StartY = y;
                    place = true;
                }
            }
        }

        public float GetRotation()
        {
            if (Facing == Direction.N)
            {
                return (float)((Math.PI * 3) / 2);
            }
            else if (Facing == Direction.S)
            {
                return (float)Math.PI / 2;
            }
            else if (Facing == Direction.E)
            {
                return 0;
            }
            else if (Facing == Direction.W)
            {
                return (float)Math.PI;
            }
            return 0;
        }

        public void MoveBackward()
        {
            if (Facing == Direction.W && _mapGrid[Position.X + 1, Position.Y] != Block.Solid)
            {
                Position = new MapVector(Position.X + 1, Position.Y);
            }
            else if (Facing == Direction.N && _mapGrid[Position.X, Position.Y + 1] != Block.Solid)
            {
                Position = new MapVector(Position.X, Position.Y + 1);
            }
            else if (Facing == Direction.E && _mapGrid[Position.X - 1, Position.Y] != Block.Solid)
            {
                Position = new MapVector(Position.X - 1, Position.Y);
            }
            else if (Facing == Direction.S && _mapGrid[Position.X, Position.Y - 1] != Block.Solid)
            {
                Position = new MapVector(Position.X, Position.Y - 1);
            }
        }

        public void MoveForward()
        {
            if (Facing == Direction.W && _mapGrid[Position.X - 1, Position.Y] != Block.Solid)
            {
                Position = new MapVector(Position.X - 1, Position.Y);
            }
            else if (Facing == Direction.N && _mapGrid[Position.X, Position.Y - 1] != Block.Solid)
            {
                Position = new MapVector(Position.X, Position.Y - 1);
            }
            else if (Facing == Direction.E && _mapGrid[Position.X + 1, Position.Y] != Block.Solid)
            {
                Position = new MapVector(Position.X + 1, Position.Y);
            }
            else if (Facing == Direction.S && _mapGrid[Position.X, Position.Y + 1] != Block.Solid)
            {
                Position = new MapVector(Position.X, Position.Y + 1);
            }
        }

        public void TurnLeft()
        {
            if (Facing == Direction.N)
            {
                Facing = Direction.W;
            }
            else if (Facing == Direction.W)
            {
                Facing = Direction.S;
            }
            else if (Facing == Direction.S)
            {
                Facing = Direction.E;
            }
            else if (Facing == Direction.E)
            {
                Facing = Direction.N;
            }
        }

        public void TurnRight()
        {
            if (Facing == Direction.N)
            {
                Facing = Direction.E;
            }
            else if (Facing == Direction.E)
            {
                Facing = Direction.S;
            }
            else if (Facing == Direction.S)
            {
                Facing = Direction.W;
            }
            else if (Facing == Direction.W)
            {
                Facing = Direction.N;
            }
        }
    }
}
