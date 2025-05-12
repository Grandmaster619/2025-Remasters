using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementbarriers : MonoBehaviour
{     
    public Transform tnf;
    public float speed = 10f;
    void Start()
    {
        //rb = this.gameObject.GetComponent<Rigidbody>();
        tnf = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    void FixedUpdate()
    {

    }

    void movePlayer()
    {
        //float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        //float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        //var pos = transform.position;
        //pos.x = Mathf.Clamp(pos.x + horizontal, -5, 5);
        //pos.z = Mathf.Clamp(pos.z + vertical, -5, 5);
        //transform.position = pos;
    }

    void OnTriggerEnter(Collider other)                    //Picking up the shield
    {
        if (other.gameObject.tag == "Shield")   
        {
            other.gameObject.SetActive(false);
        }
    }
}
/*
        var sm = GameObject;
        sm = (GameObject)Instantiate(Shield, transform.position, transform.rotation);
        sm.transform.parent = Player.transform; 

        //private KeyCode mvRight = KeyCode.D, mvLeft = KeyCode.A, mvUp = KeyCode.W, mvDown = KeyCode.S;
*/
