using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float treadDropInterval = 0.1f;

    [SerializeField]
    private GameObject treadSpawner;
    [SerializeField]
    private GameObject treadmarkPrefab;

    private Vector3 movement, startingPosition;
    private bool moving = false;
    private float treadTimer;
    public bool stopMovementH = false;
    public bool stopMovementV = false;
    private bool playingSound = false;

    public float turnSpeed;
    private float turnAngle;
    private float h = 0;
    private float v = 0;
    private float hh = 1;
    private float vv = 1;
    private float objectRotation;

    private AudioManager audio;



    private void Awake()
    {
        startingPosition = transform.position;
        hh = vv = 1;
        audio = FindObjectOfType<AudioManager>();
    }


    // Use this for initialization
    void Start()
    {
        treadTimer = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        objectRotation = this.transform.eulerAngles.y;
        moving = false;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");


        //if (objectRotation > 0f && objectRotation < 45f || objectRotation > 225f && objectRotation < 360f) {}




        if (h == v && h != 0 && v != 0)
        {
            if (objectRotation > 0f && objectRotation < 45f || objectRotation > 225f && objectRotation < 360f)
            {
                rotateObject(315.0f);
            }
            else
            {
                rotateObject(135.0f);
            }
        }

        else if (h != v && h != 0 && v != 0)
        {
            if (objectRotation > 315f && objectRotation < 360f || objectRotation > 0f && objectRotation < 135f)
            {
                rotateObject(45.0f);
            }
            else
            {
                rotateObject(225.0f);
            }
        }


        else if (h != 0)
        {
            if ((objectRotation > 275f && objectRotation < 360f) || (objectRotation > 0f && objectRotation < 85f))
            {
                rotateObject(360.0f);
            }
            else if (objectRotation < 265f && objectRotation > 85f)
            {
                rotateObject(180.0f);
            }
            else if (h == 1 && vv == 1 && objectRotation > 265f && objectRotation < 275f)
            {
                rotateObject(315.0f);
            }
            else if (h == 1 && vv == -1 && objectRotation > 265f && objectRotation < 275f)
            {
                rotateObject(135.0f);
            }
            else if (h == -1 && vv == 1 && objectRotation > 265f && objectRotation < 275f)
            {
                rotateObject(225.0f);
            }
            else if (h == -1 && vv == -1 && objectRotation > 265f && objectRotation < 275f)
            {
                rotateObject(45.0f);
            }
            else if (h == 1 && vv == 1 && objectRotation > 85f && objectRotation < 95f)
            {
                rotateObject(90.0f);
            }
            else if (h == 1 && vv == -1 && objectRotation > 85f && objectRotation < 95f)
            {
                rotateObject(90.0f);
            }
            else if (h == -1 && vv == 1 && objectRotation > 85f && objectRotation < 95f)
            {
                rotateObject(90.0f);
            }
            else if (h == -1 && vv == -1 && objectRotation > 85f && objectRotation < 95f)
            {
                rotateObject(90.0f);
            }
        }

        else if (v != 0)
        {
            if (objectRotation > 5f && objectRotation < 175f)
            {
                rotateObject(90.0f);
            }
            else if (objectRotation > 185f && objectRotation < 355f)
            {
                rotateObject(270.0f);
            }
            else if (v == 1 && hh == 1 && (objectRotation > 355f && objectRotation <= 360f || objectRotation >= 0 && objectRotation < 5f))
            {
                rotateObject(315.0f);
            }
            else if (v == 1 && hh == -1 && (objectRotation > 355f && objectRotation <= 360f || objectRotation >= 0 && objectRotation < 5f))
            {
                rotateObject(135.0f);
            }
            else if (v == -1 && hh == 1 && (objectRotation > 355f && objectRotation <= 360f || objectRotation >= 0 && objectRotation < 5f))
            {
                rotateObject(45.0f);
            }
            else if (v == -1 && hh == -1 && (objectRotation > 355f && objectRotation <= 360f || objectRotation >= 0 && objectRotation < 5f))
            {
                rotateObject(225.0f);
            }
            else if (v == 1 && hh == 1 && objectRotation > 175f && objectRotation < 185f)
            {
                rotateObject(135.0f);
            }
            else if (v == 1 && hh == -1 && objectRotation > 175f && objectRotation < 185f)
            {
                rotateObject(315.0f);
            }
            else if (v == -1 && hh == 1 && objectRotation > 175f && objectRotation < 185f)
            {
                rotateObject(225.0f);
            }
            else if (v == -1 && hh == -1 && objectRotation > 175f && objectRotation < 185f)
            {
                rotateObject(45.0f);
            }
        }

        if (h != 0)
        {
            hh = h;
            if (v == 0 && (objectRotation > 80 && objectRotation < 100 || objectRotation > 260 && objectRotation < 280))
            {
                stopMovementH = true;
            }
            if (v == 0 && (objectRotation > -10 && objectRotation < 10 || objectRotation > 170 && objectRotation < 190))
            {
                stopMovementH = false;
            }
        }

        if (v != 0)
        {
            vv = v;
            if (h == 0 && (objectRotation > -10 && objectRotation < 10 || objectRotation > 170 && objectRotation < 190))
            {
                stopMovementV = true;
            }
            if (h == 0 && (objectRotation > 80 && objectRotation < 100 || objectRotation > 260 && objectRotation < 280))
            {
                stopMovementV = false;
            }
        }


        if (h == 0 || v != 0)
        {
            stopMovementH = false;
        }

        if (v == 0 || h != 0)
        {
            stopMovementV = false;
        }

        if (h != 0 && v != 0)
        {
            h /= 1.5f;
            v /= 1.5f;
        }

        if (!stopMovementH && !stopMovementV)
        {
            Vector3 movement = new Vector3(h, 0.0f, v);
            transform.Translate(movement * Time.deltaTime * speed, Space.World);
        }

        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            audio.Play("Movement");
            treadTimer += Time.deltaTime;
            if (treadTimer >= treadDropInterval)
            {
                GameObject.Instantiate(treadmarkPrefab, treadSpawner.transform.position, treadSpawner.transform.rotation);
                treadTimer = 0;
            }
        }
        else
        {
            audio.Stop("Movement");
            playingSound = false;
        }

        /*
        if ((float)Mathf.Abs(objectRotation) % 90f < 50.0f)
        {
            Vector3 movement = new Vector3(h, 0.0f, v);
            transform.Translate(movement * Time.deltaTime * speed, Space.World);
        }
        */

        /*
        Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, turnAngle, transform.rotation.eulerAngles.z);
        Quaternion targetRotation = Quaternion.Euler(direction);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
      */

    }

    void rotateObject(float turnAngle)
    {
        Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, turnAngle, transform.rotation.eulerAngles.z);
        Quaternion targetRotation = Quaternion.Euler(direction);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(movement.x * speed * Time.fixedDeltaTime, 
                                                         GetComponent<Rigidbody>().velocity.y, 
                                                         movement.z * speed * Time.fixedDeltaTime);

        
    }

    public void ReturnToStart()
    {
        transform.position = startingPosition;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Destructible"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}