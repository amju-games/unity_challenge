// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// ----------------------------------------------------------------------------------------------------------
// Graph class
// Represents graph of connected FloorTiles
// ----------------------------------------------------------------------------------------------------------
public class Graph  
{
    // ------------------------------------------------------------------------------------------------------
    // SINGLETON

    public static Graph Instance()
    {
        if (s_instance == null)
        {
            s_instance = new Graph();
        }
        return s_instance;
    }

    // ------------------------------------------------------------------------------------------------------

    // Get the total number of connections between floor tiles (only useful for testing)
    public int NumConnections
    {
        get
        {
            return m_connections.NumConnections;
        }
    }
        
    // ------------------------------------------------------------------------------------------------------

    // Get the number of floor tile nodes
    public int NumNodes
    {
        get
        {
            return m_nodes.Count;
        }
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns true if two floor tiles are connected.
    public bool AreConnected(int nodeId1, int nodeId2)
    {
        return m_connections.AreConnected(nodeId1, nodeId2);
    }

    // ------------------------------------------------------------------------------------------------------

    // Check new node against all existing nodes, add edges where two nodes are adjacent in "iso coords".
    public void AddNode(FloorTile ft)
    {
        // ID of node is the same as index in m_nodes
        ft.ID = m_nodes.Count;

        foreach (var node in m_nodes)
        {
            if (ft.IsAdjacentTo(node))
            {
                m_connections.AddConnection(ft.ID, node.ID);
                m_connections.AddConnection(node.ID, ft.ID);
            }
        }

        // Add the new node. 
        m_nodes.Add(ft);
    }

    // ------------------------------------------------------------------------------------------------------

    // Find the node with the closest coords to the given position, when we convert the position to 
    //  "iso coords". So we will find a tile which is close in screen coords to the given 3D position, 
    //  but which could be a long way away in 3D space.
    public int FindClosestNodeInIsoSpace(Vector3 pos)
    {
        int closestNodeID = 0;
        Vector2 isoPos = FloorTile.CalcIsoCoord(pos);
        float closestSqrDist = 999999.9f; // FLT_MAX ?
        // Find node with closest iso coord to isoPos.
        foreach (var node in m_nodes)
        {
            Vector2 thisIsoPos = node.IsoCoord;
            float sqrDist = (isoPos - thisIsoPos).SqrMagnitude();
            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closestNodeID = node.ID;
            }
        }
        return closestNodeID;
    }

    // ------------------------------------------------------------------------------------------------------

    // Find path from tile with ID "fromId" to tile with ID "toId". 
    // For the resulting path, return the 3D positions of the tiles.
    // Returns empty list if there is no path.
    public List<FloorTile> FindPath(int fromId, int toId)
    {
        AStar astar = new AStar();
        List<int> path = new List<int>();

        bool isPath = astar.PathExists(m_connections, fromId, toId, path);

        List<FloorTile> result = new List<FloorTile>();
        if (isPath)
        {
            foreach (var n in path)
            {
                Assert.IsTrue(n >= 0);
                Assert.IsTrue(n < m_nodes.Count);

                FloorTile ft = m_nodes[n];
                result.Add(ft);
            }
        }
        return result;
    }

    // ======================================================================================================

    // SINGLETON
    private Graph()
    {
        m_nodes = new List<FloorTile>();
        m_connections = new Connections();
    }
        
    // ======================================================================================================
    // Private member variables

    // SINGLETON
    private static Graph s_instance = null;

    // FloorTile nodes
    private List<FloorTile> m_nodes;

    // Graph of Floor Tile IDs. These IDs are indices into m_nodes.
    Connections m_connections;
}
