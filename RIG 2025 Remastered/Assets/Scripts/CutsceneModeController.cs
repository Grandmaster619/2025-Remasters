using UnityEngine;

public class CutsceneModeController : MonoBehaviour
{
    // Reference to the script controlling player movement/input
    public PlayerMovement playerMovementScript;

    // Method to enable/disable player input
    public void SetPlayerInputEnabled(bool isEnabled)
    {
        // Enable/disable player input by toggling the enabled state of the player movement script
        playerMovementScript.enabled = isEnabled;
    }
}