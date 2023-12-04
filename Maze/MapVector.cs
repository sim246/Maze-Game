namespace Maze
{
    public class MapVector : IMapVector
    {
        public bool IsValid { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public MapVector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool InsideBoundary(int width, int height)
        {
            IsValid = false;
            if (this.X >= width || this.Y >= height || this.X < 0 || this.Y < 0)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
            return IsValid;
        }

        public double Magnitude()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public static MapVector operator +(MapVector vector1, MapVector vector2)
        {
            int x = vector1.X + vector2.X;
            int y = vector1.Y + vector2.Y;

            return new MapVector(x, y);
        }

        public static MapVector operator -(MapVector vector1, MapVector vector2)
        {
            int x = vector1.X - vector2.X;
            int y = vector1.Y - vector2.Y;

            return new MapVector(x, y);
        }

        public static MapVector operator *(MapVector vector, int scalar)
        {
            int x = vector.X * scalar;
            int y = vector.Y * scalar;

            return new MapVector(x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not MapVector)
            {
                return false;
            }
            MapVector p = (MapVector)obj;
            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static implicit operator MapVector(Direction dir)
        {
            MapVector v = new MapVector(0, 0);
            if ((dir & Direction.N) != Direction.None)
            {
                v.Y--;
            }
            if ((dir & Direction.S) != Direction.None)
            {
                v.Y++;
            }
            if ((dir & Direction.W) != Direction.None)
            {
                v.X--;
            }
            if ((dir & Direction.E) != Direction.None)
            {
                v.X++;
            }
            return v;
        }
    }
}
