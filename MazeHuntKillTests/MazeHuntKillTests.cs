using MazeHuntKill;
using Maze;
using System.Collections.Generic;

namespace MazeHuntKillTests;

[TestClass]
public class MazeHuntKillTests
{
    [TestMethod]
    public void MazeHuntKillTest()
    {
        HuntKill map = new HuntKill();
        Assert.IsNotNull(map);
    }

    [TestMethod]
    public void MazeHuntKillTestOppositeDirection()
    {
       HuntKill map = new HuntKill();

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
    public void MazeHuntKillTestValidDirections()
    {
        HuntKill map = new HuntKill(0);
        map.CreateMap(4, 4);

        for (int i = 0; i < map._mazeDirections.GetLength(0); i++)
        {
            for (int j = 0; j < map._mazeDirections.GetLength(0); j++)
            {
                map._mazeDirections[i, j] = Direction.None;
            }

        }
        List<Direction> list = map.ValidDirections(new MapVector(0,3));
        Assert.AreEqual(list.Count, 2);
        Assert.AreEqual(list[0], Direction.S);
        Assert.AreEqual(list[1], Direction.W);
    }

    [TestMethod]
    public void MazeHuntKillTestVectorForwardPosition()
    {
        HuntKill map = new HuntKill();
        map.CreateMap(4,4);

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
    public void MazeHuntKillTestRandomDirection()
    {
        HuntKill map = new HuntKill(1);
        map.CreateMap(4,4);
        for (int i = 0; i < map._mazeDirections.GetLength(0); i++)
        {
            for (int j = 0; j < map._mazeDirections.GetLength(0); j++)
            {
                map._mazeDirections[i, j] = Direction.None;
            }

        }
        Direction dir = map.RandomDirection(new MapVector(0, 1));
        Assert.AreEqual(dir, Direction.E);
        dir = map.RandomDirection(new MapVector(0, 1));
        Assert.AreEqual(dir, Direction.W);
    }

    [TestMethod]
    public void MazeHuntKillTestCreateMapHuntKill()
    {
        HuntKill map = new HuntKill(0);
        Direction[,] mazeDirections = map.CreateMap(2,2);

        Assert.AreEqual(mazeDirections[0, 0], Direction.E | Direction.S);
        Assert.AreEqual(mazeDirections[0, 1], Direction.W);
        Assert.AreEqual(mazeDirections[1, 0], Direction.N | Direction.E);
        Assert.AreEqual(mazeDirections[1, 1], Direction.W);
    }
}