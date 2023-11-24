using Maze;
using MazeRecursion;

namespace MazeRecursionTests;

[TestClass]
public class MazeRecursionTests
{
    [TestMethod]
    public void MazeRecursiveTest()
    {
        MazeRecursive map = new MazeRecursive();
        Assert.IsNotNull(map);
    }

    [TestMethod]
    public void MazeRecursiveTestVectorForwardPosition()
    {
        MazeRecursive map = new MazeRecursive();

        MapVector v = map.VectorForwardPosition(Direction.N, new MapVector(1, 0));
        Assert.IsTrue(v.Equals(new MapVector(0, 0)));

        v = map.VectorForwardPosition(Direction.S, new MapVector(1, 0));
        Assert.IsTrue(v.Equals(new MapVector(2, 0)));

        v = map.VectorForwardPosition(Direction.E, new MapVector(0, 1));
        Assert.IsTrue(v.Equals(new MapVector(0, 2)));

        v = map.VectorForwardPosition(Direction.W, new MapVector(0, 1));
        Assert.IsTrue(v.Equals(new MapVector(0, 0)));
    }

    [TestMethod]
    public void MazeRecursiveTestOppositeDirection()
    {
        MazeRecursive map = new MazeRecursive();

        Direction dir = map.OppositeDirection(Direction.N);
        Assert.AreEqual(dir, Direction.S);

        dir = map.OppositeDirection(Direction.S);
        Assert.AreEqual(dir, Direction.N);

        dir = map.OppositeDirection(Direction.W);
        Assert.AreEqual(dir, Direction.E);

        dir = map.OppositeDirection(Direction.E);
        Assert.AreEqual(dir, Direction.W);
    }

    [TestMethod]
    public void MazeRecursiveTestRandomDirectionList()
    {
        MazeRecursive map = new MazeRecursive(1);
        List<Direction> list = map.RandomDirectionList();

        Assert.AreEqual(list.Count, 4);
        Assert.AreEqual(list[0], Direction.S);
        Assert.AreEqual(list[1], Direction.E);
        Assert.AreEqual(list[2], Direction.W);
        Assert.AreEqual(list[3], Direction.N);
    }

    [TestMethod]
    public void MazeRecursiveTestCreateMapRecursive()
    {
        MazeRecursive map = new MazeRecursive(0);
        Direction[,] mazeDirections = map.CreateMap(2,2);

        Assert.AreEqual(mazeDirections[0, 0], Direction.E | Direction.S);
        Assert.AreEqual(mazeDirections[0, 1], Direction.S | Direction.W);
        Assert.AreEqual(mazeDirections[1, 0], Direction.N);
        Assert.AreEqual(mazeDirections[1, 1], Direction.N);
    }
}