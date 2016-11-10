// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// TestGraph class
// Unit tests for the Graph class.
// ----------------------------------------------------------------------------------------------------------
public class TestGraph 
{
    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void EmptyGraphTest()
    {
        // Create a new empty graph - test constructed correctly
        Graph g = Graph.Instance();
        Assert.AreEqual(0, g.NumConnections); // expected, actual
        Assert.AreEqual(0, g.NumNodes); // expected, actual
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void TwoNodesGraphTest()
    {
        // Create a graph and add two nodes. Check consistency.
        Graph g = Graph.Instance();

        // Hmm, "new FloorTile" is not allowed because it's a MonoBehaviour - only Unity can create.
        // Does that mean these tests are being ignored, or will always fail?
        // TODO
        g.AddNode(new FloorTile(0, new Vector3(0, 0, 0)));
        g.AddNode(new FloorTile(1, new Vector3(1, 0, 0)));

        Assert.AreEqual(2, g.NumNodes); // expected, actual

        // 2 connections (connections are one-way)
        // When we add the second node to the graph, we should see that it is adjacent to the first in 
        //  "iso coords", so create an edge between the two nodes.
        Assert.AreEqual(2, g.NumConnections); // expected, actual

        // Check nodes are connected - nodes are identified by their IDs.
        Assert.IsTrue(g.AreConnected(0, 1));
        Assert.IsTrue(g.AreConnected(1, 0));

        // Nodes are not connected to themselves
        Assert.IsFalse(g.AreConnected(0, 0));
        Assert.IsFalse(g.AreConnected(1, 1));
    }

    // ------------------------------------------------------------------------------------------------------
}
