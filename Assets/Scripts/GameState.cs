// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ----------------------------------------------------------------------------------------------------------
// GameState class
// In theory this manages the current game state. In practice it doesn't do much at the moment - there is
//  probably too much going on in PlayerController.
// ----------------------------------------------------------------------------------------------------------
public class GameState : MonoBehaviour 
{
    // ------------------------------------------------------------------------------------------------------

    // The UI canvas, so we can control the visibility of the UI
    public Canvas TheCanvas;

    // ------------------------------------------------------------------------------------------------------

    // The Player Controller - we need this to reset the player
    public PlayerController Avatar;

    // ------------------------------------------------------------------------------------------------------

    // Initialization
    void Start () 
    {
        TheCanvas.GetComponent<Canvas>().enabled = false;
    }

    // ------------------------------------------------------------------------------------------------------

    // Called when we come to a stop on the winning floor tile.
    public void Winner()
    {
        // Show the UI
        TheCanvas.GetComponent<Canvas>().enabled = true;
    }

    // ------------------------------------------------------------------------------------------------------

    // UI button callback.
    public void OnOkButton()
    {
        // Hide the UI 
        TheCanvas.GetComponent<Canvas>().enabled = false;

        Avatar.Restart(new Vector3(4.5f, 0.5f, 5.5f));
    }
}
