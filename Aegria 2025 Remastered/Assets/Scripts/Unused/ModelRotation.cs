// Author(s): Lincoln Schroeder
//
// Purpose: This file is an example of how a runtime script should interact with the 
// persistent object's pause mechanic. This script simply rotates the bee model, but
// doesn't rotate anymore if the game is paused.
//
// Date Last Modified: 10/31/2019

using UnityEngine;

public class ModelRotation : MonoBehaviour
{

    private PersistentController persistentController;

	// Initializes the persistentController.
	void Start ()
    {
        persistentController = GameObject.FindWithTag("Persistent").GetComponent<PersistentController>();
	}
	
	// Rotates the object, but only if the game is not paused.
	void Update ()
    {
        if (!persistentController.IsPaused())
        {
            transform.Rotate(1, 0, 0);
        }
	}
}
