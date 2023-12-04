using Moq;

namespace Maze.Tests
{
    [TestClass()]
    public class MazeTests
    {

        [TestMethod()]
        public void MapTest()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };
            IPlayer player = new Player(MapGrid, 5, 5);
            var map = new Mock<IMap>();
            map.Setup(m => m.MapGrid).Returns(MapGrid);
            map.Setup(m => m.Height).Returns(5);
            map.Setup(m => m.Width).Returns(5);
            map.Setup(m => m.Player.Position).Returns(new MapVector(0, 0));
            map.Setup(m => m.Player.StartX).Returns(0);
            map.Setup(m => m.Player.StartY).Returns(0);
            map.Setup(m => m.Goal).Returns(new MapVector(0, 0));

            Assert.AreEqual(map.Object.Height, 5);
            Assert.AreEqual(map.Object.Width, 5);
            Assert.AreEqual(map.Object.MapGrid, MapGrid);
            Assert.AreEqual(map.Object.Player.StartX, 0);
            Assert.AreEqual(map.Object.Player.StartY, 0);
            Assert.IsTrue(map.Object.Goal.Equals(new MapVector(0, 0)));
            Assert.IsTrue(map.Object.Player.Position.Equals(new MapVector(0, 0)));
        }

        [TestMethod()]
        public void MapTestFinishedTrue()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };
            IPlayer player = new Player(MapGrid, 5, 5);
            var map = new Mock<IMap>();
            map.Setup(m => m.MapGrid).Returns(MapGrid);
            map.Setup(m => m.Height).Returns(5);
            map.Setup(m => m.Width).Returns(5);
            map.Setup(m => m.Player).Returns(player);
            map.Setup(m => m.Player.Position).Returns(new MapVector(0,0));
            map.Setup(m => m.Goal).Returns(new MapVector(0, 0));
            map.Setup(m => m.IsGameFinished).Returns(true);

            Assert.AreEqual(map.Object.Height, 5);
            Assert.AreEqual(map.Object.Width, 5);
            Assert.AreEqual(map.Object.MapGrid, MapGrid);
            Assert.IsTrue(map.Object.IsGameFinished);
        }

        [TestMethod()]
        public void MapTestFinishedFalse()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };
            IPlayer player = new Player(MapGrid, 5, 5);
            var map = new Mock<IMap>();
            map.Setup(m => m.MapGrid).Returns(MapGrid);
            map.Setup(m => m.Height).Returns(5);
            map.Setup(m => m.Width).Returns(5);
            map.Setup(m => m.Player).Returns(player);
            map.Setup(m => m.Player.Position).Returns(new MapVector(0, 0));
            map.Setup(m => m.Goal).Returns(new MapVector(1, 1));
            map.Setup(m => m.IsGameFinished).Returns(false);

            Assert.AreEqual(map.Object.Height, 5);
            Assert.AreEqual(map.Object.Width, 5);
            Assert.AreEqual(map.Object.MapGrid, MapGrid);
            Assert.IsFalse(map.Object.IsGameFinished);
        }

        [TestMethod()]
        public void MapTestPlayer()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };
            IPlayer player = new Player(MapGrid, 5, 5);
            var map = new Mock<IMap>();
            map.Setup(m => m.Player).Returns(player);

            Assert.IsNotNull(map.Object.Player);
        }

        [TestMethod()]
        public void MapVectorAddTest()
        {
            MapVector v1 = new MapVector(3, 6);
            MapVector v2 = new MapVector(1, 1);
            MapVector v = v1 + v2;

            Assert.AreEqual(v.X, 4);
            Assert.AreEqual(v.Y, 7);
        }

        [TestMethod()]
        public void MapVectorSubtractTest()
        {
            MapVector v1 = new MapVector(3, 6);
            MapVector v2 = new MapVector(1, 1);
            MapVector v = v1 - v2;

            Assert.AreEqual(v.X, 2);
            Assert.AreEqual(v.Y, 5);
        }

        [TestMethod()]
        public void MapVectorMultiplyTest()
        {
            MapVector testA = new MapVector(3, 6);
            int testB = 3;
            MapVector testC = testA * testB;

            Assert.AreEqual(testC.X, 9);
            Assert.AreEqual(testC.Y, 18);
        }

        [TestMethod()]
        public void MapVectorEqualTest()
        {
            MapVector v1 = new MapVector(3, 6);
            MapVector v2 = new MapVector(3, 6);
            MapVector v3 = new MapVector(4, 5);

            bool test = v1.Equals(v2);
            Assert.IsTrue(test);

            test = v1.Equals(v3);
            Assert.IsFalse(test);

            test = v1.Equals(null);
            Assert.IsFalse(test);

            String str = "test";
            test = v1.Equals(str);
            Assert.IsFalse(test);
        }

        [TestMethod()]
        public void MapVectorGetHashCodeTest()
        {
            var v = new MapVector(3, 6);

            var test = v.GetHashCode();
            Assert.AreEqual(test, 5);
        }

        [TestMethod()]
        public void MapVectorMagnitudeTest()
        {
            MapVector v = new MapVector(4, 4);
            double test = v.Magnitude();

            Assert.AreEqual(test, 5.656854249492381);
        }

        [TestMethod()]
        public void MapVectorInsideBoundaryTest()
        {
            MapVector v = new MapVector(4, 4);
            bool test = v.InsideBoundary(3, 3);
            Assert.IsTrue(test);
            Assert.IsTrue(v.IsValid);
            test = v.InsideBoundary(5, 5);
            Assert.IsFalse(test);
            Assert.IsFalse(v.IsValid);
        }

        [TestMethod()]
        public void MapVectorCastDirectionTest()
        {
            Direction dir = Direction.None;
            MapVector v = (MapVector)dir;
            Assert.AreEqual(v.X, 0);
            Assert.AreEqual(v.Y, 0);

            dir = Direction.N;
            v = (MapVector)dir;
            Assert.AreEqual(v.X, 0);
            Assert.AreEqual(v.Y, -1);

            dir = Direction.S;
            v = (MapVector)dir;
            Assert.AreEqual(v.X, 0);
            Assert.AreEqual(v.Y, 1);

            dir = Direction.E;
            v = (MapVector)dir;
            Assert.AreEqual(v.X, 1);
            Assert.AreEqual(v.Y, 0);

            dir = Direction.W;
            v = (MapVector)dir;
            Assert.AreEqual(v.X, -1);
            Assert.AreEqual(v.Y, 0);
        }

        [TestMethod()]
        public void PlayerTestPlacePlayer()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid } };

            Player player = new Player(MapGrid, 5, 5);
            player.PlacePlayer(5, 5, 0);
            Assert.IsTrue(player.Position.Equals(new MapVector(3,2)));
            Assert.AreEqual(player.StartX, 3);
            Assert.AreEqual(player.StartY, 2);
            Assert.AreEqual(player.Facing, Direction.E);
        }

        [TestMethod()]
        public void PlayerTurnLeftAndGetTest()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid } };

            IPlayer player = new Player(MapGrid, 5, 5);


            player.TurnLeft();
            Assert.AreEqual(player.GetRotation(), (float)((Math.PI * 3) / 2));
            Assert.AreEqual(player.Facing, Direction.N);

            player.TurnLeft();
            Assert.AreEqual(player.GetRotation(), (float)Math.PI);
            Assert.AreEqual(player.Facing, Direction.W);

            player.TurnLeft();
            Assert.AreEqual(player.GetRotation(), (float)(Math.PI / 2));
            Assert.AreEqual(player.Facing, Direction.S);

            player.TurnLeft();
            Assert.AreEqual(player.GetRotation(), 0);
            Assert.AreEqual(player.Facing, Direction.E);
        }

        [TestMethod()]
        public void PlayerTurnRightTest()
        {
            Block[,] MapGrid = new Block[5, 5] {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid } };
            IPlayer player = new Player(MapGrid, 5, 5);

            player.TurnRight();
            Assert.AreEqual(player.Facing, Direction.S);
            player.TurnRight();
            Assert.AreEqual(player.Facing, Direction.W);
            player.TurnRight();
            Assert.AreEqual(player.Facing, Direction.N);
            player.TurnRight();
            Assert.AreEqual(player.Facing, Direction.E);
        }
    }
}