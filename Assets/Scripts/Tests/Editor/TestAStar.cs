// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// TestAStar class
// Test the AStar graph-searching class.
// ----------------------------------------------------------------------------------------------------------
public class TestAStar 
{
    // ------------------------------------------------------------------------------------------------------
    [Test]
    public void PriorityQueueTest()
    {
        // Tests the priority queue implementation
        PriorityQueue pq = new PriorityQueue();

        Assert.IsTrue(pq.IsEmpty());

        pq.Enqueue(1, 5); // key, priority, LOWER priority wins
        Assert.IsFalse(pq.IsEmpty());

        Assert.AreEqual(1, pq.Front());

        pq.Enqueue(2, 3);
        Assert.AreEqual(2, pq.Front()); // 2 is more important (lower priority value)

        pq.UpdatePriority(1, 1); // Now 1 should be at front again
        Assert.AreEqual(1, pq.Front());

        Assert.AreEqual(1, pq.Dequeue());

        Assert.IsFalse(pq.IsEmpty());

        // 1 removed, so 2 should now be at the front
        Assert.AreEqual(2, pq.Front());

        pq.Enqueue(3, 4);

        // 2 should still be at the front
        Assert.AreEqual(2, pq.Front());

        Assert.AreEqual(2, pq.Dequeue());

        // Now 3 should be at the front
        Assert.AreEqual(3, pq.Dequeue());

        // Now the queue is empty.
        Assert.IsTrue(pq.IsEmpty());
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void EmptyGraphTest()
    {
        AStar astar = new AStar();

        Connections c = new Connections();

        // There is no path between any two node IDs
        List<int> path = new List<int>();
        Assert.IsFalse(astar.PathExists(c, 0, 1, path));
        Assert.AreEqual(0, path.Count);
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void GraphOneWayTest()
    {
        // Test pathfinding when we set up a graph with some one-way connections 
        AStar astar = new AStar();

        Connections c = new Connections();

        // Set up a simple graph
        c.AddConnection(0, 1);
        c.AddConnection(1, 2);
        c.AddConnection(2, 3);
        c.AddConnection(4, 5);
        c.AddConnection(3, 6);
        c.AddConnection(2, 7);
        c.AddConnection(3, 7);

        List<int> path = new List<int>();

        // These paths should exist:
        Assert.IsTrue(astar.PathExists(c, 0, 1, path));
        Assert.AreEqual(2, path.Count);
        Assert.AreEqual(0, path[0]);
        Assert.AreEqual(1, path[1]);

        Assert.IsTrue(astar.PathExists(c, 0, 2, path));
        Assert.AreEqual(3, path.Count);
        Assert.AreEqual(0, path[0]);
        Assert.AreEqual(1, path[1]);
        Assert.AreEqual(2, path[2]);

        Assert.IsTrue(astar.PathExists(c, 0, 3, path));
        Assert.IsTrue(astar.PathExists(c, 4, 5, path));

        // These paths should not exist:
        Assert.IsFalse(astar.PathExists(c, 0, 5, path));
        Assert.IsFalse(astar.PathExists(c, 3, 5, path));

        // The connections in this Test are one-way, so paths do not exist going the other way
        Assert.IsFalse(astar.PathExists(c, 2, 0, path));
        Assert.IsFalse(astar.PathExists(c, 1, 0, path));
        Assert.IsFalse(astar.PathExists(c, 3, 0, path));
        Assert.IsFalse(astar.PathExists(c, 5, 4, path));
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void GraphTwoWayTest()
    {
        // Test pathfinding when we set up a graph with some two-way connections
        AStar astar = new AStar();

        Connections c = new Connections();

        // Set up graph
        c.AddConnectionTwoWay(0, 1);
        c.AddConnectionTwoWay(1, 2);
        c.AddConnectionTwoWay(2, 3);
        c.AddConnectionTwoWay(4, 5);
        c.AddConnectionTwoWay(3, 6);
        c.AddConnectionTwoWay(2, 7);
        c.AddConnectionTwoWay(3, 7);

        List<int> path = new List<int>();

        // These paths should exist:
        Assert.IsTrue(astar.PathExists(c, 0, 2, path));
        Assert.IsTrue(astar.PathExists(c, 0, 1, path));
        Assert.IsTrue(astar.PathExists(c, 0, 3, path));
        Assert.IsTrue(astar.PathExists(c, 4, 5, path));

        // These paths should not exist:
        Assert.IsFalse(astar.PathExists(c, 0, 5, path));
        Assert.IsFalse(astar.PathExists(c, 3, 5, path));

        // The connections in this Test are two-way, so paths DO exist going the other way
        Assert.IsTrue(astar.PathExists(c, 2, 0, path));
        Assert.IsTrue(astar.PathExists(c, 1, 0, path));
        Assert.IsTrue(astar.PathExists(c, 3, 0, path));
        Assert.IsTrue(astar.PathExists(c, 5, 4, path));
    }

    // ------------------------------------------------------------------------------------------------------
}

