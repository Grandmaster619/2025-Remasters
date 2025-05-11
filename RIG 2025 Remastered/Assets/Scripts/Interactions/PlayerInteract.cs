using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerUI.UpdateText(string.Empty);

        //create a ray
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable to store collision info
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            //check if it has an interactable component
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                //Debug.Log("Has component");
                //update the prompt on-screen
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);

                //if the E button is pressed, interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("GameObject: " + gameObject);
                    Debug.Log("interactable: " + interactable);
                    interactable.BaseInteract(gameObject);
                }
            }
            else
            {
                playerUI.ResetText();
            }
        }
        else
        {
            playerUI.ResetText();
        }
    }
}
