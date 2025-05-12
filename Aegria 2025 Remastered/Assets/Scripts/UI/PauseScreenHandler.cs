// Author(s): Lincoln Schroeder
//
// Purpose: This file handles the enabling/disabling of the pause screen when in game.
// This script cannot be placed on the pause screen itself, because if the screen is 
// disabled, this script will not run anymore.
//
// Date Last Modified: 10/31/2019

using UnityEngine;

public class PauseScreenHandler : MonoBehaviour
{

    private PersistentController persistentController;
    private GameObject pauseCanvas;

	// Initializes the two gameobjects when this object is initialized.
	void Start ()
    {
        persistentController = GameObject.FindWithTag("Persistent").GetComponent<PersistentController>();
        pauseCanvas = GameObject.Find("PauseCanvas");
	}
	
	// Checks if the game is paused, enabling or disabling the pause screen.
	void Update ()
    {
        if (persistentController.IsPaused())
            pauseCanvas.SetActive(true);
        else if (this.gameObject.activeSelf)
            pauseCanvas.SetActive(false);
	}
}
