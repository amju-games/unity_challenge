// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// FloorTile class
// Represents a square tile in the world on which the player can walk.
// Floor tiles have a 3D position and an "iso coord", which is the 3D position projected onto isometric
//  screen coords. 
// ----------------------------------------------------------------------------------------------------------
public class FloorTile : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------------

    public FloorTile()
    {
    }

    // Overloaded constructors for testing... but this might not be the right way to do it, as only Unity can
    //  create MonoBehaviours..?
    public FloorTile(int id_)
    {
        ID = id_;
    }

    public FloorTile(int id_, Vector3 pos_)
    {
        ID = id_;
        Pos = pos_;
        CalcIsoCoord();
    }

    // ------------------------------------------------------------------------------------------------------

    // Public member vars are settable in the Uniy editor.
    // If you check this box, the player crouches on this tile.
    public bool IsCrouchy = false;

    // ------------------------------------------------------------------------------------------------------

    // Set this for a winning tile, which means you have succesfully reached the end of the game.
    public bool IsWinner = false;

    // ------------------------------------------------------------------------------------------------------

    public static PlayerController ThePlayer
    {
        get;
        set;
    }

    // ------------------------------------------------------------------------------------------------------

    public int ID
    {
        get;
        set;
    }

    // ------------------------------------------------------------------------------------------------------

    // The position of the Tile in isometric/screen space
    public Vector2 IsoCoord
    {
        get;
        set;
    }

    // ------------------------------------------------------------------------------------------------------

    // The position of the tile in 3D space
    public Vector3 Pos
    {
        get;
        set;
    }

    // ------------------------------------------------------------------------------------------------------

    // Conceptually private, public for testing. Maybe this should go in Utils? 
    // It's really a property of the camera though.
    // Project 3D coord onto iso coord space. 
    public static Vector2 CalcIsoCoord(Vector3 pos)
    {
        Vector2 ic = new Vector2(pos.x - pos.y, pos.z - pos.y);
        return ic;
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns true if two iso coords are adjacent
    public static bool AreIsoCoordsAdjacent(Vector2 u, Vector2 v)
    {
        // "Table layout": easier to spot mistakes in copy-pastey code: http://www.viva64.com/en/b/0391/
        return 
            (   (u.x == v.x     && u.y == v.y + 1)    // x same, y differ by 1
             || (u.x == v.x     && u.y == v.y - 1)      
             || (u.x == v.x + 1 && u.y == v.y    )    // y same, x differ by 1
             || (u.x == v.x - 1 && u.y == v.y    ));   
        
        // Watch out for the "last line effect": last line of copy-pasted code is more likely to be wrong!
        // http://www.viva64.com/en/b/0260/
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns true if the other floor tile is adjacent to this one.
    public bool IsAdjacentTo(FloorTile other)
    {
        return AreIsoCoordsAdjacent(this.IsoCoord, other.IsoCoord);
    }

    // ------------------------------------------------------------------------------------------------------

    // Calculate our iso coord from our 3D coord
    public void CalcIsoCoord()
    {
        IsoCoord = CalcIsoCoord(this.Pos);
    }

    // ------------------------------------------------------------------------------------------------------

    // Called from Unity when player clicks on this floor tile
    public void OnMouseUpAsButton()
    {
        // Pathfind to this clicked floor tile.
        ThePlayer.PathfindToFloorTile(this);
    }

    // ------------------------------------------------------------------------------------------------------

	// Initialization
	void Start() 
    {
        Pos = transform.position;
        CalcIsoCoord();

        // Add this floor tile to the graph
        Graph.Instance().AddNode(this);

        // Make sure mesh rendering is off
        GetComponent<Renderer>().enabled = false;
	}

    // ------------------------------------------------------------------------------------------------------
}
