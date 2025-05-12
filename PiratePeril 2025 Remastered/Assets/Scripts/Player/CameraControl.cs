using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject playerObject;
    public float followTime = 0.5f;

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 destination = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -9);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, followTime);
    }
}
