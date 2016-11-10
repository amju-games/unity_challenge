// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// ----------------------------------------------------------------------------------------------------------
// PlayerController class
// Controls player avatar movement
// ----------------------------------------------------------------------------------------------------------
public class PlayerController : MonoBehaviour 
{
    // ------------------------------------------------------------------------------------------------------
    // Game state: controls overall behaviour of the game 
    public GameState TheGameState;

    // ------------------------------------------------------------------------------------------------------

	// Initialization
    public void Start() 
    {
        // Tell floor tiles about this player controller
        FloorTile.ThePlayer = this;

        m_currentPos = transform.position; 
        m_targetPos = m_currentPos;
        m_direction = new Vector3();
        m_isWalking = false;
        m_desiredRotDegs = 0;

        m_trail = new List<FloorTile>();
   	}

    // ------------------------------------------------------------------------------------------------------

    // Called to restart the game
    public void Restart(Vector3 pos) 
    {
        transform.position = pos;
        m_currentPos = pos; 
        m_targetPos = m_currentPos;
        m_direction = new Vector3();
        m_isWalking = false;
        m_isBouncing = false;
        m_desiredRotDegs = 0;
        transform.rotation = new Quaternion(0, 1, 0, 0); // 180 deg about y
        m_trail.Clear();
    }

    // ------------------------------------------------------------------------------------------------------

    // Update is called once per frame
    // As this is kind of physics, should it be FixedUpdate? TODO UNITY_Q 
    public void Update() 
    {
        // Move towards target
        float dt = Time.deltaTime;
        m_currentPos += m_direction * PLAYER_SPEED * m_speedCompensation * dt;

        // Bounce
        // Only bounce when we are moving, but if we are mid bounce when we stop, finish the current bounce.
        float bounce = 0;
        if (m_isBouncing)
        {
            m_bounceTime += dt; 
            bounce = Mathf.Sin(m_bounceTime * BOUNCE_SPEED) * BOUNCE_HEIGHT;

            if (m_isCrouching)
            {
                bounce *= 0.5f; // bounce lower
            }

            if (bounce < 0)
            {
                bounce = 0;
                m_bounceTime = 0;
                if (!m_isWalking)
                {
                    m_isBouncing = false;
                }
            }
        }

        transform.position = m_currentPos + new Vector3(0, bounce, 0);

        if (m_isWalking)
        {
            // Reached target pos?
            // Compare in iso coords, and check if we have overshot, to be on the safe side.
            Vector2 isoCurrent = FloorTile.CalcIsoCoord(m_currentPos);
            Vector2 isoTarget = FloorTile.CalcIsoCoord(m_targetPos);
            float newSqrDist = (isoCurrent - isoTarget).SqrMagnitude();
            if (   Utils.VeryClose(isoCurrent, isoTarget, 0.1f)
                || newSqrDist > m_previousSqrDist)
            {
                m_currentPos = m_targetPos;
                m_direction = new Vector3();
                m_isWalking = false;

                // Another target pos to head towards?
                SetTargetPosToNextTrailElement();
            }
            else
            {
                // Remember this distance so next time we can detect if we have overshot.
                m_previousSqrDist = newSqrDist;
            }

            // Turn to face target pos
            Quaternion currentRot = transform.rotation;
            Quaternion idealRot = Quaternion.AngleAxis(m_desiredRotDegs, new Vector3(0, 1, 0));
            Quaternion newRot = Quaternion.RotateTowards(currentRot, idealRot, PLAYER_ROTATION_SPEED);
            transform.rotation = newRot;
        }

        // Shrink vertically if crouching
        if (m_isCrouching)
        {
            transform.localScale = new Vector3(1, CROUCH_SCALE, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
	}

    // ------------------------------------------------------------------------------------------------------

    // Find closest node to our current position. This is the start node. 
    // Pathfind to the given destination node.
    public void PathfindToFloorTile(FloorTile destinationTile)
    {
        Graph g = Graph.Instance(); // TODO Hmm, pass in graph
        int currentTileID = g.FindClosestNodeInIsoSpace(m_currentPos);
        List<FloorTile> trail = g.FindPath(currentTileID, destinationTile.ID);
        if (trail.Count > 0)
        {
            SetTrail(trail);
        }
    }
        
    // ======================================================================================================
    // Private member functions

    // Get the next target position from the trail, then head towards it.
    private void SetTargetPosToNextTrailElement()
    {
        Assert.IsTrue(m_trail.Count > 0);
        Assert.IsTrue(m_trailIndex <= m_trail.Count);
            
        // Are we done?
        if (m_trailIndex == m_trail.Count)
        {
            // We have reached the end of the trail
            m_isWalking = false; 

            // Turn off rendering on the destination tile, as we have reached it
            FloorTile destFt = m_trail[m_trail.Count - 1];
            destFt.GetComponent<Renderer>().enabled = false;

            // Have we reached the winning floor tile?
            if (destFt.IsWinner)
            {
                // Omg! We have won the game!
                TheGameState.Winner();
            }
            return;
        }

        Assert.IsTrue(m_trailIndex < m_trail.Count);

        FloorTile ft = m_trail[m_trailIndex];

        m_isWalking = true;
        m_isBouncing = true;

        m_isCrouching = ft.IsCrouchy;

        Vector3 targetpos = ft.Pos;
        m_trailIndex++;
        SetTargetPos(ft);

        // Calc desired heading
        if (m_trailIndex < m_trail.Count)
        {
            // Calc desired rotation about y axis
            //  - the desired rotation is in ISO COORDS, not 3D coords! Agh!

            // Get dir from previous to new tile in iso space
            FloorTile ftNew = m_trail[m_trailIndex]; // new tile
            Vector2 dir = ftNew.IsoCoord - ft.IsoCoord; // Dir from previous to new tile
            m_desiredRotDegs = Utils.CalcDesiredYAxisRot(new Vector3(dir.x, 0, dir.y));
        }
    }

    // ------------------------------------------------------------------------------------------------------

    // Set the given coord are the target, so we immediately start moving towards it
    private void SetTargetPos(FloorTile ft)
    {
        m_targetPos = ft.Pos;
        m_direction = (m_targetPos - m_currentPos);

        // Store sqr dist from current to target in iso coords. 
        // If the distance ever gets larger, something has gone wrong, and we will treat the target as
        //  reached.
        Vector2 isoCurrent = FloorTile.CalcIsoCoord(m_currentPos);
        Vector2 isoTarget = FloorTile.CalcIsoCoord(m_targetPos);
        m_previousSqrDist = (isoCurrent - isoTarget).SqrMagnitude();

        // Check direction is a non-zero vec before normalising or trying to turn to face.
        // Ignore very small magnitude vector.
        if (m_direction.sqrMagnitude > 0.01f)
        {
            m_speedCompensation = m_direction.magnitude;

            m_direction.Normalize();
        }
    }
        
    // ------------------------------------------------------------------------------------------------------

    // Set up a new trail (sequence of coords) for us to follow.
    private void SetTrail(List<FloorTile> trail_)
    {
        Assert.IsTrue(trail_.Count > 0);

        m_trail = trail_;
        m_trailIndex = 0;
        SetTargetPosToNextTrailElement(); 
    }
        
    // ======================================================================================================

    // Constants

    // Speed at which we move, in visual/screen space 
    private const float PLAYER_SPEED = 3.0f;

    // Speed at which player rotates to face direction of movement
    private const float PLAYER_ROTATION_SPEED = 10.0f;

    // Max height of bounce
    private const float BOUNCE_HEIGHT = 0.5f;

    // Speed at which we bounce
    private const float BOUNCE_SPEED = 20.0f;

    // Vertical scale factor when we are "crouching"
    private const float CROUCH_SCALE = 0.7f;

    // ------------------------------------------------------------------------------------------------------

    // Position we are moving towards 
    private Vector3 m_targetPos;

    // Our current position
    private Vector3 m_currentPos;

    // Direction in which we are moving, i.e. pointing from current to target
    private Vector3 m_direction;

    // Desired rotation about y-axis, in degrees, in order to face in direction m_direction.
    private float m_desiredRotDegs = 0;

    // True if we are moving from current position to target position
    private bool m_isWalking = false;

    // True if we are in a bounce
    private bool m_isBouncing = false;

    // Time elapsed in current bounce
    private float m_bounceTime = 0.0f;

    // Adjust speed so we move at a constant speed in screen space, although the distance between two 
    //  tiles varies depending on how far away they are in 3D.
    private float m_speedCompensation = 1.0f;

    // Trail: list of floor tile coords we should visit in turn to get to the target,
    //  i.e. the final element in the trail.
    private List<FloorTile> m_trail;

    // Index into m_trail: we are moving towards m_trail[m_trailIndex].
    private int m_trailIndex = 0;

    // If true, reduce player avatar height to minimise visual artefacts.
    private bool m_isCrouching = false;

    // The squared dist between current and target. If the new value is greater, we have overshot, so treat as 
    //  having reached the target. Overshooting has been a problem a few times with a large speed multiplier.
    private float m_previousSqrDist;
}
