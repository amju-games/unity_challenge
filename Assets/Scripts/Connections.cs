// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// ----------------------------------------------------------------------------------------------------------
// Connections class
// Maybe rename this IntGraph - it is a graph of ints.
// ----------------------------------------------------------------------------------------------------------
public class Connections 
{
    // ------------------------------------------------------------------------------------------------------

    public Connections()
    {
        m_numConnections = 0;
        m_connections = new List<List<int>>();
    }

    // ------------------------------------------------------------------------------------------------------

    public void Print()
    {
        string s;
        Console.WriteLine("Connections:");
        for (int i = 0; i < m_connections.Count; i++)
        {
            s = "From: " + i.ToString() + ": ";
            var neighbours = m_connections[i];
            foreach (int n in neighbours)
            {
                s += n.ToString() + " ";
            }
            Console.WriteLine(s);
        }
    }

    // ------------------------------------------------------------------------------------------------------

    public int NumConnections
    {
        // Get the number of ONE-WAY connections between nodes
        get
        {
            return m_numConnections;
        }
    }

    // ------------------------------------------------------------------------------------------------------

    // Convenience function: add two one-way connections between two nodes
    public void AddConnectionTwoWay(int nodeId1, int nodeId2)
    {
        AddConnection(nodeId1, nodeId2);
        AddConnection(nodeId2, nodeId1);
    }

    // ------------------------------------------------------------------------------------------------------

    // Add a ONE-WAY connection between two nodes
    public void AddConnection(int nodeId1, int nodeId2)
    {
        // Connections are stored as a vector of vectors:
        // Node ID is used to index into the first vector. So if the index is greater than the vector size, 
        //  we add elements as required.

        Assert.IsTrue(nodeId1 >= 0);
        Assert.IsTrue(nodeId2 >= 0);

        while (nodeId1 >= m_connections.Count)
        {
            m_connections.Add(new List<int>());
        }

        var neighbours = m_connections[nodeId1];

        if (!neighbours.Contains(nodeId2))
        {
            neighbours.Add(nodeId2);
        }

        m_numConnections++;

        Assert.IsTrue(AreConnected(nodeId1, nodeId2));
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns true if there is a connection from node 1 to node 2. 
    public bool AreConnected(int nodeId1, int nodeId2)
    {
        if (m_connections.Count <= nodeId1)
        {
            return false;
        }

        var neighbours = m_connections[nodeId1];

        return (neighbours.Contains(nodeId2));            
    }
 
    // ------------------------------------------------------------------------------------------------------

    // Get the nodes directly connected to the node given as param.
    public List<int> GetNeighbours(int id)
    {
        if (m_connections.Count <= id)
        {
            return new List<int>();
        }

        return m_connections[id];
    }

    // ======================================================================================================
    // Private member variables

    // Connections between nodes. We want a good way of getting all neighbours from a node, for A*.
    // Node IDs are zero-based contiguous integers, so vector of vectors is a good choice.
    private List<List<int>> m_connections;

    // Total number of connections: this is the sum of the sizes of the lists in m_connections.
    private int m_numConnections;
}

