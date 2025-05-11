using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour

{
    public float speed = 5;
    public Rigidbody rb;
    
    public bool gas = false;
    public bool propellor = false;
    public bool tool_kit = false;
    public bool boat_moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tool_kit && gas && propellor)
        {
            Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
            rb.MovePosition(rb.position + forwardMove);
            boat_moving = true;
        }
        else
        {
            boat_moving = false;
        }
    }
}
