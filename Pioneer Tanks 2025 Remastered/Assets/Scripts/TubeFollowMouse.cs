using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Authors: Charles Roltgen and Ryan Wolff
/// <summary>
/// Turns the turret towards the mouse pointer.
/// Should be attatched to the 
/// </summary>
public class TubeFollowMouse : MonoBehaviour {

    [SerializeField]
    private GameObject turretRotationPoint;
    [SerializeField]
    private Camera main_camera;
    private Vector3 mousePosition, directionVector;
    private Ray ray;
    private RaycastHit hit;
    private int groundLayerMask;

    void Start ()
    {
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        mousePosition = Input.mousePosition;
        ray = main_camera.ScreenPointToRay(Input.mousePosition);
    }

    void Update ()
    {
        FollowMouse();
	}

    /// <summary>
    /// Uses the position of the cursor and the tanks turret to
    /// rotate the turret towards where the mouse points.
    /// </summary>
    /// <remarks>
    /// This currently ignores all layers except the "Ground".
    /// This is to reduce jittering when the mouse passes over
    /// walls.
    /// </remarks>
    void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        RaycastHit hit;
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            //Find out where we are pointing
            Vector3 direction = hit.point - turretRotationPoint.transform.position;
            //Find the global angle. Ignore the vertical component
            Quaternion rot = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            turretRotationPoint.transform.rotation = rot;
        }
    }
}
