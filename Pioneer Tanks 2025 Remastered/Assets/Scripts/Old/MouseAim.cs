using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour {

    [SerializeField]
    private GameObject tankTube;
    private Camera main_camera;
    private Vector3 mousePosition, directionVector, angleOffset;
    private Ray ray;
    private RaycastHit hit;
    private int groundLayerMask;

    void Start ()
    {
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        main_camera = Camera.main;
        mousePosition = Input.mousePosition;
        ray = main_camera.ScreenPointToRay(Input.mousePosition);
    }

    void Update () {
        FollowMouse();
	}

    void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        RaycastHit hit;
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            Vector3 directionVector = (hit.point - transform.position).normalized;
            Vector3 rote = new Vector3(-90, Mathf.Rad2Deg * Mathf.Atan(directionVector.x/directionVector.z), 90);
            if(hit.point.z < transform.position.z)
            {
                rote.y += 180;
            }
            transform.localEulerAngles = rote;
        }

        //Vector3 direction = new Vector3(
        //    mousePosition.x - transform.position.x,
        //    mousePosition.y - transform.position.y
        //    );

        //transform.rotation = Quaternion.Euler(0, direction.y, 0);
    }
}
