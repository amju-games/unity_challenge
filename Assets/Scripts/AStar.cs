// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

// ----------------------------------------------------------------------------------------------------------
// Simple priority queue to use in our A*/Dijkstra algos.
// ----------------------------------------------------------------------------------------------------------
public class PriorityQueue
{
    // ------------------------------------------------------------------------------------------------------

    public PriorityQueue()
    {
        m_valuePriorityMap = new Dictionary<int, int>();
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns true if nothing enqueued
    public bool IsEmpty()
    {
        return (m_valuePriorityMap.Count == 0);
    }

    // ------------------------------------------------------------------------------------------------------

    // Add a key and its priority. For our purposes, LOWEST VALUE priority wins, i.e. the key with 
    //  the lowest priority is what you will get when you call Front()/Dequeue().
    public void Enqueue(int key, int priority)
    {
        Assert.IsFalse(m_valuePriorityMap.ContainsKey(key));
        m_valuePriorityMap[key] = priority;
    }

    // ------------------------------------------------------------------------------------------------------

    // Update the priority of a key.
    public void UpdatePriority(int key, int newPriority)
    {
        Assert.IsTrue(m_valuePriorityMap.ContainsKey(key));
        m_valuePriorityMap[key] = newPriority;
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns the key with the LOWEST priority value.
    // Removes that (key, priority) pair from the queue.
    public int Dequeue()
    {
        int key = Front();
        m_valuePriorityMap.Remove(key);
        return key;
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns the key with the LOWEST priority value.
    public int Front()
    {
        Assert.IsTrue(m_valuePriorityMap.Count > 0);

        int key = 0;
        int lowestPrioritySoFar = 999999; // a big int. Is there something like INT_MAX?
        foreach (var kv in m_valuePriorityMap)
        {
            int thisPriority = kv.Value;
            if (thisPriority < lowestPrioritySoFar)
            {
                lowestPrioritySoFar = thisPriority;
                key = kv.Key;
            }
        }
        return key;
    }

    // ======================================================================================================

    // (value, priority) pairs. 
    private Dictionary<int, int> m_valuePriorityMap;
}

// ==========================================================================================================

// ----------------------------------------------------------------------------------------------------------
// AStar class
// Implements graph searching functions
// ----------------------------------------------------------------------------------------------------------
public class AStar 
{
    // ------------------------------------------------------------------------------------------------------

    // Depth first search. This is the simplest algo and good to start with. Once tested, we can then
    //  use it as a control against which to test more complicated algos.
    public bool DepthFirst(Connections c, int from, int to, List<int> path)
    {
        Stack<int> toVisit = new Stack<int>();
        // For the small numbers of nodes we have, it might be more efficient to just store in a vector?
        HashSet<int> visited = new HashSet<int>();
        Dictionary<int, int> breadcrumbs = new Dictionary<int, int>();

        toVisit.Push(from);
        visited.Add(from);

        while (toVisit.Count > 0)
        {
            int node = toVisit.Pop();
            if (node == to)
            {
                // Found a path!
                MakeTrailFromBreadcrumbs(breadcrumbs, path, from, to);
                return true;
            }
            List<int> neighbours = c.GetNeighbours(node);
            foreach (var n in neighbours)
            {
                if (!visited.Contains(n))
                {
                    toVisit.Push(n);
                    visited.Add(n); // so we won't add it again
                    // Store how to get to n
                    breadcrumbs[n] = node;
                }
            }
        }

        return false;
    }

    // ------------------------------------------------------------------------------------------------------

    // Breadth-first search
    public bool BreadthFirst(Connections c, int from, int to, List<int> path)
    {
        Queue<int> toVisit = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();
        Dictionary<int, int> breadcrumbs = new Dictionary<int, int>();

        toVisit.Enqueue(from);
        visited.Add(from);

        while (toVisit.Count > 0)
        {
            int node = toVisit.Dequeue();
            if (node == to)
            {
                // Found a path!
                MakeTrailFromBreadcrumbs(breadcrumbs, path, from, to);
                return true;
            }
            List<int> neighbours = c.GetNeighbours(node);
            foreach (var n in neighbours)
            {
                if (!visited.Contains(n))
                {
                    toVisit.Enqueue(n);
                    visited.Add(n); // so we won't add it again
                    // Store how to get to n
                    breadcrumbs[n] = node;
                }
            }
        }

        return false;
    }
        
    // ------------------------------------------------------------------------------------------------------

    public bool PathExists(Connections c, int node1, int node2, List<int> path)
    {        
        // Hmm, Breadth-first seems to work here, no need for A* today
        return BreadthFirst(c, node1, node2, path);
    }

    // ======================================================================================================

    // Generates trail of nodes from 'breadcrumbs' map.
    private static void MakeTrailFromBreadcrumbs( 
        Dictionary<int, int> breadcrumbs, // map of connected nodes, as ("from", "to") pairs
        List<int> trail, // the trail result, allocated by caller
        int from, 
        int to)
    {
        trail.Clear();

        #if WRITE_TO_LOG
        string s;
        s = "Make trail: From: " + from.ToString() + " to: " + to.ToString();
        Console.WriteLine(s);

        s = "Breadcrumbs: ";
        foreach (var kv in breadcrumbs)
        {
        s += "(" + kv.Key.ToString() + ": " + kv.Value.ToString() + ") ";
        }
        Console.WriteLine(s);
        #endif // WRITE_TO_LOG

        trail.Add(to);
        int n = to;
        while (true)
        {
            if (   !breadcrumbs.ContainsKey(n)
                || breadcrumbs[n] == n)
            {
                #if WRITE_TO_LOG
                s = "Trail: ";
                foreach (var t in trail)
                {
                s += t.ToString() + " ";
                }
                Console.WriteLine(s);
                #endif // WRITE_TO_LOG

                return;
            }
            n = breadcrumbs[n];
            trail.Insert(0, n);
        }
    }

    // ------------------------------------------------------------------------------------------------------
}

