using BenchMarking;
using Maze;

namespace MazeRecursionTests;
[TestClass]
public class ImprovedRecursiveTests
{
    [TestMethod]
    public void ImprovedRecursiveTest()
    {
        ImprovedRecursive map = new ImprovedRecursive();
        Assert.IsNotNull(map);
    }

    [TestMethod]
    public void ImprovedRecursiveTestVectorForwardPosition()
    {
        ImprovedRecursive map = new ImprovedRecursive();

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
    public void ImprovedRecursiveTestOppositeDirection()
    {
        ImprovedRecursive map = new ImprovedRecursive();

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
    public void ImprovedRecursiveTestCreateMapRecursive()
    {
        ImprovedRecursive map = new ImprovedRecursive(0);
        Direction[,] mazeDirections = map.CreateMap(2, 2);

        Assert.AreEqual(mazeDirections[0, 0], Direction.E | Direction.S);
        Assert.AreEqual(mazeDirections[0, 1], Direction.W);
        Assert.AreEqual(mazeDirections[1, 0], Direction.N | Direction.E);
        Assert.AreEqual(mazeDirections[1, 1], Direction.W);
    }

}
