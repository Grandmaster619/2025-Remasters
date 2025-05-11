using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    //public Transform InteractorSource;
    public Camera mainCamera; // Reference to the main camera
    public float InteractRange;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Attempting to interact...");

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, InteractRange))
            {
                if (hit.collider.gameObject.CompareTag("Button")) // Assuming "Button" is the tag for the GameButton
                {
                    if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}