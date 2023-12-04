using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeTests")]
namespace Maze
{
    public class Map : IMap
    {
        public MapVector Goal { get; private set; }

        public int Height
        {
            get
            {
                return _height;
            }
            private set
            {
                _height = (value) * 2 + 1;
            }
        }
        public int Width
        {
            get
            {
                return _width;
            }
            private set
            {
                _width = (value) * 2 + 1;
            }
        }

        private bool _isGameFinished;
        public bool IsGameFinished
        {
            get
            {
                if (Goal.Equals(Player.Position))
                {
                    _isGameFinished = true;
                }
                else
                {
                    _isGameFinished = false;
                }
                return _isGameFinished;
            }
        }

        public Block[,] MapGrid { get; private set; }

        public IPlayer Player { get; private set; }

        private int _height;

        private int _width;

        private Direction[,] _mazeDirections;

        private readonly IMapProvider _map;

        public Map(IMapProvider mp)
        {
            _map = mp;
        }

        private void PlaceGoal(List<MapVector> mazeEnds, int? seed = null)
        {
            Player = new Player(MapGrid, Height, Width);
            Random rnd = new Random();
            if (seed != null)
            {
                rnd = new Random((int)seed);
            }

            int location = rnd.Next(mazeEnds.Count);
            Goal = mazeEnds[location];
            bool place = false;
            while (!place)
            {
                if (Player.StartX < Goal.X || Player.StartX > Goal.X)
                {
                    if (Player.StartY < Goal.Y || Player.StartY > Goal.Y)
                    {
                        place = true;
                    }
                }
                Player = new Player(MapGrid, Height, Width);
            }
        }

        private void MakeMap()
        {
            Height = _mazeDirections.GetLength(1);
            Width = _mazeDirections.GetLength(0);
            MapGrid = new Block[Height, Width];
            int i = 0;
            int j = 0;
            List<MapVector> mazeEnds = new List<MapVector> { };
            for (int x = 1; x < Height; x++)
            {
                for (int y = 1; y < Width; y++)
                {
                    MapGrid[x, y] = Block.Empty;

                    if ((_mazeDirections[i, j] & Direction.N) != Direction.None)
                    {
                        MapGrid[x, y - 1] = Block.Empty;
                    }
                    if ((_mazeDirections[i, j] & Direction.S) != Direction.None)
                    {
                        MapGrid[x, y + 1] = Block.Empty;
                    }
                    if ((_mazeDirections[i, j] & Direction.W) != Direction.None)
                    {
                        MapGrid[x - 1, y] = Block.Empty;
                    }
                    if ((_mazeDirections[i, j] & Direction.E) != Direction.None)
                    {
                        MapGrid[x + 1, y] = Block.Empty;
                    }

                    if (_mazeDirections[i, j] == Direction.N || _mazeDirections[i, j] == Direction.S || _mazeDirections[i, j] == Direction.E || _mazeDirections[i, j] == Direction.W)
                    {
                        mazeEnds.Add(new MapVector(x, y));
                    }

                    i++;
                    y++;
                }
                i = 0;
                j++;
                x++;
            }
            PlaceGoal(mazeEnds);
        }

        public void CreateMap()
        {
            _mazeDirections = _map.CreateMap();
            MakeMap();
        }

        public void CreateMap(int width, int height)
        {
            _mazeDirections = _map.CreateMap(height, width);
            MakeMap();
        }

        public void SaveDirectionMap(string path)
        {
            throw new NotImplementedException();
        }
    }
}
