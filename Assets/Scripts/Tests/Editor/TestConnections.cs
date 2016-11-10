// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// TestConnections class
// Test the Connections class.
// ----------------------------------------------------------------------------------------------------------
public class TestConnections 
{
    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void EmptyTest()
    {
        Connections c = new Connections();

        Assert.AreEqual(0, c.NumConnections);

        // There are no connections between any node IDs.
        Assert.IsFalse(c.AreConnected(0, 0));
        Assert.IsFalse(c.AreConnected(1, 2));
        Assert.IsFalse(c.AreConnected(3, 4)); // etc
    }

    // ----------------------------------------------------------------------------------------------------------

    [Test]
    public void TwoNodeOneWayConnectionTest()
    {
        Connections c = new Connections();

        c.AddConnection(0, 1);

        Assert.AreEqual(1, c.NumConnections);
        Assert.IsTrue(c.AreConnected(0, 1));
        Assert.IsFalse(c.AreConnected(0, 0));
        Assert.IsFalse(c.AreConnected(1, 0)); // one-way connection, 0->1, not 1->0
        Assert.IsFalse(c.AreConnected(1, 1));

        // Add another connection
        c.AddConnection(1, 2);

        Assert.AreEqual(2, c.NumConnections);
        Assert.IsTrue(c.AreConnected(1, 2));
        Assert.IsFalse(c.AreConnected(2, 1));
        Assert.IsFalse(c.AreConnected(2, 2));                            
        Assert.IsFalse(c.AreConnected(0, 2)); // not directly connected, this would only be found with a search
    }

    // ----------------------------------------------------------------------------------------------------------

    [Test]
    public void TwoNodeNeighbourTest()
    {
        Connections c = new Connections();

        c.AddConnection(0, 1);

        List<int> neighbours = c.GetNeighbours(0);
        Assert.AreEqual(1, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(1));
        Assert.IsFalse(neighbours.Contains(0));

        c.AddConnection(0, 2);

        neighbours = c.GetNeighbours(0);
        Assert.AreEqual(2, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(1));
        Assert.IsTrue(neighbours.Contains(2));
        Assert.IsFalse(neighbours.Contains(0));
    }

    // ----------------------------------------------------------------------------------------------------------

    [Test]
    public void TwoWayConnectionTest()
    {
        Connections c = new Connections();

        c.AddConnectionTwoWay(0, 1);

        Assert.AreEqual(2, c.NumConnections);

        List<int> neighbours = c.GetNeighbours(0);
        Assert.AreEqual(1, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(1));
        Assert.IsFalse(neighbours.Contains(0));

        neighbours = c.GetNeighbours(1);
        Assert.AreEqual(1, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(0));
        Assert.IsFalse(neighbours.Contains(1));

        c.AddConnectionTwoWay(0, 2);

        Assert.AreEqual(4, c.NumConnections);

        neighbours = c.GetNeighbours(0);
        Assert.AreEqual(2, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(1));
        Assert.IsTrue(neighbours.Contains(2));
        Assert.IsFalse(neighbours.Contains(0));

        neighbours = c.GetNeighbours(2);
        Assert.AreEqual(1, neighbours.Count);
        Assert.IsTrue(neighbours.Contains(0));
        Assert.IsFalse(neighbours.Contains(1));
        Assert.IsFalse(neighbours.Contains(2));
    }

    // ----------------------------------------------------------------------------------------------------------
    // Exceptions: compiler crashes when trying to use exceptions!? 
    // So not using exceptions and not testing for them.
    // (Compiler crashes on line "throw System.ArgumentException")

    #if USING_EXCEPTIONS
    private static void ThrowsBecauseFirstIDNegative()
    {
        Connections c = new Connections();
        c.AddConnection(-1, 0);
    }

    private static void ThrowsBecauseSecondIDNegative()
    {
        Connections c = new Connections();
        c.AddConnection(-1, 0);
    }

     
    [Test]
    public void NegativeIDsThrowTest()
    {
        Assert.Throws(typeof(System.ArgumentException), new TestDelegate(ThrowsBecauseFirstIDNegative));

        Assert.Throws(typeof(System.ArgumentException), new TestDelegate(ThrowsBecauseSecondIDNegative));
    }
    #endif // USING_EXCEPTIONS
    // ----------------------------------------------------------------------------------------------------------
}

