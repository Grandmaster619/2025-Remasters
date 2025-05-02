using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behavior
{
    OFFENSIVE,
    DEFENSIVE
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float treadDropInterval = 0.1f;
    [SerializeField]
    private Behavior behavior = Behavior.DEFENSIVE;
    [SerializeField]
    private float movementLeniency = 1f;

    [SerializeField]
    private GameObject treadSpawner;
    [SerializeField]
    private GameObject treadmarkPrefab;
    [SerializeField]
    private GameObject turret;

    [SerializeField] private int maxMines = 0;
    [SerializeField] private GameObject minePrefab;

    private Vector3 movement, startingPosition;
    private bool moving = false;
    private float treadTimer = 0, mineTimer = 0, backAwayTimer = 0, checkTimer = 0, wanderTimer = 0, wanderChangeTimer = 0;
    private bool pauseToShoot = false, backAway = false;
    private int curMines;
    public bool PauseToShoot { get => pauseToShoot; set => pauseToShoot = value; }

    private GameObject player;

    public float turnSpeed;
    public float turnAngle;
    public float h;
    public float v;
    public float hh = 0;
    public float vv = 0;
    public float objectRotation;
    public bool stopMovementH = false;
    public bool stopMovementV = false;


    private void Awake()
    {
        startingPosition = transform.position;
        curMines = maxMines;
        hh = vv = 1;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mineTimer += Time.deltaTime;
        backAwayTimer += Time.deltaTime;
        checkTimer += Time.deltaTime;
        wanderTimer += Time.deltaTime;

        objectRotation = this.transform.eulerAngles.y;
        moving = false;
        if (wanderTimer >= 2)
            h = v = 0;
        // logic here for where tank should move
        if (!pauseToShoot)
        {
            DetermineMovementDirection();
        }


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
            moving = true;



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

    private void DetermineMovementDirection()
    {
        RaycastHit upRay, downRay, leftRay, rightRay;
        float rayDistance = 1f;

        RaycastHit playerVisible;
        Vector3 playerDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, playerDirection, out playerVisible))
        {
            if (playerVisible.transform.CompareTag("Player"))
            {
                if (behavior == Behavior.OFFENSIVE && backAwayTimer > 2f)
                {
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > movementLeniency)
                    {
                        if (player.transform.position.x < transform.position.x)
                        {
                            h = -1;
                        }
                        else
                        {
                            h = 1;
                        }
                    }

                    if (Mathf.Abs(player.transform.position.z - transform.position.z) > movementLeniency)
                    {
                        if (player.transform.position.z < transform.position.z)
                        {
                            v = -1;
                        }
                        else
                        {
                            v = 1;
                        }
                    }
                }



                if (behavior == Behavior.DEFENSIVE && backAwayTimer > 2f)
                {
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) < movementLeniency &&
                        Mathf.Abs(player.transform.position.z - transform.position.z) < movementLeniency)
                    {
                        if (player.transform.position.x < transform.position.x)
                        {
                            h = 1;
                        }
                        else
                        {
                            h = -1;
                        }

                        if (player.transform.position.z < transform.position.z)
                        {
                            v = 1;
                        }
                        else
                        {
                            v = -1;
                        }
                    }
                }
            }
            else
                wanderTimer = 0;
        }



        if (wanderTimer <= 2f)
        {
            Debug.Log("Wandering");
            wanderChangeTimer -= Time.deltaTime;
            if (wanderChangeTimer <= 0)
            {
                h = Random.Range(-1, 2);
                v = Random.Range(-1, 2);
                wanderChangeTimer = Random.Range(0.5f, 1.5f);
            }
        }



        if (checkTimer >= 1f)
        {
            checkTimer = 0;
            Collider[] objectsClose = Physics.OverlapSphere(transform.position, 1.75f);
            foreach (Collider collider in objectsClose)
            {
                if (collider.CompareTag("Destructible") || collider.CompareTag("Player"))
                {
                    if (curMines > 0 && mineTimer >= 2f)
                    {
                        mineTimer = 0;
                        curMines--;
                        GameObject.Instantiate(minePrefab, treadSpawner.transform.position, new Quaternion());
                    }
                }
            }

            Collider[] objectsClose1 = Physics.OverlapSphere(transform.position, 3f);
            foreach (Collider collider in objectsClose1)
            {
                if (collider.CompareTag("Mine"))
                {
                    Debug.Log("next to a mine");
                    backAwayTimer = 0;

                }
            }
        }

        if (backAwayTimer <= 2f)
        {
            Collider[] objectsClose1 = Physics.OverlapSphere(transform.position, 3f);
            foreach (Collider collider in objectsClose1)
            {
                if (collider.CompareTag("Mine"))
                {
                    if (collider.transform.position.y > transform.position.y)
                        v = -1;
                    else
                        v = 1;


                    if (collider.transform.position.x > transform.position.x)
                        h = -1;
                    else
                        h = 1;
                }
            }


        }

        if ((Physics.Raycast(transform.position, new Vector3(0, 0, 1), out upRay, rayDistance) ||
    Physics.Raycast(transform.position, new Vector3(0.707f, 0, 0.707f), out upRay, rayDistance * 0.5f) ||
    Physics.Raycast(transform.position, new Vector3(-0.707f, 0, 0.707f), out upRay, rayDistance * 0.5f)))
        {
            if (v == 1)
                v = 0;
        }
        if ((Physics.Raycast(transform.position, new Vector3(0, 0, -1), out downRay, rayDistance) ||
            Physics.Raycast(transform.position, new Vector3(0.707f, 0, -0.707f), out downRay, rayDistance * 0.5f) ||
            Physics.Raycast(transform.position, new Vector3(-0.707f, 0, -0.707f), out downRay, rayDistance * 0.5f)))
        {
            if (v == -1)
                v = 0;
        }
        if ((Physics.Raycast(transform.position, new Vector3(1, 0, 0), out rightRay, rayDistance) ||
            Physics.Raycast(transform.position, new Vector3(0.707f, 0, 0.707f), out rightRay, rayDistance) ||
            Physics.Raycast(transform.position, new Vector3(0.707f, 0, -0.707f), out rightRay, rayDistance)))
        {
            if (h == 1)
                h = 0;
        }
        if ((Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out leftRay, rayDistance) ||
            Physics.Raycast(transform.position, new Vector3(-0.707f, 0, 0.707f), out leftRay, rayDistance) ||
            Physics.Raycast(transform.position, new Vector3(-0.707f, 0, -0.707f), out leftRay, rayDistance)))
        {
            if (h == -1)
                h = 0;
        }

    }

    private int findWayAround(string blockDirection, int posNeg)
    {
        RaycastHit xBoundRayPos, xBoundRayNeg, zBoundRayPos, zBoundRayNeg;
        LayerMask wallMask = LayerMask.GetMask("Wall");
        Physics.Raycast(transform.position, new Vector3(1, 0, 0), out xBoundRayPos, 50, wallMask.value);
        Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out xBoundRayNeg, 50, wallMask.value);
        Physics.Raycast(transform.position, new Vector3(0, 0, 1), out zBoundRayPos, 50, wallMask.value);
        Physics.Raycast(transform.position, new Vector3(0, 0, -1), out zBoundRayNeg, 50, wallMask.value);
        if (blockDirection == "x")
        {
            float posDistance = 100f, negDistance = 100f;
            for (float i = transform.position.x; i < xBoundRayPos.point.x; i++)
            {
                Vector3 startPos = new Vector3(i, transform.position.y, transform.position.z);
                RaycastHit checkDist;
                Physics.Raycast(startPos, new Vector3(0, 0, posNeg), out checkDist, 50, wallMask.value);
                if (Mathf.Abs(checkDist.point.z - transform.position.z) > 1.6f)
                {
                    posDistance = Mathf.Abs(transform.position.x - i);
                    break;
                }
            }

            for (float i = transform.position.x; i > xBoundRayNeg.point.x; i--)
            {
                Vector3 startPos = new Vector3(i, transform.position.y, transform.position.z);
                RaycastHit checkDist;
                Physics.Raycast(startPos, new Vector3(0, 0, posNeg), out checkDist, 50, wallMask.value);
                if (Mathf.Abs(checkDist.point.z - transform.position.z) > 1.6f)
                {
                    negDistance = Mathf.Abs(transform.position.x - i);
                    break;
                }
            }
            if (posDistance < negDistance)
                return 1;
            else
                return -1;
        }
        else
        {
            float posDistance = 100f, negDistance = 100f;
            for (float i = transform.position.z; i < zBoundRayPos.point.z; i++)
            {
                Vector3 startPos = new Vector3(transform.position.x, transform.position.y, i);
                RaycastHit checkDist;
                Physics.Raycast(startPos, new Vector3(posNeg, 0, 0), out checkDist, 50, wallMask.value);
                if (Mathf.Abs(checkDist.point.x - transform.position.x) > 1.5f)
                {
                    posDistance = Mathf.Abs(transform.position.z - i);
                    break;
                }
            }

            for (float i = transform.position.z; i > zBoundRayNeg.point.z; i--)
            {
                Vector3 startPos = new Vector3(transform.position.x, transform.position.y, i);
                RaycastHit checkDist;
                Physics.Raycast(startPos, new Vector3(posNeg, 0, 0), out checkDist, 50, wallMask.value);
                if (Mathf.Abs(checkDist.point.x - transform.position.x) > 1.5f)
                {
                    negDistance = Mathf.Abs(transform.position.z - i);
                    break;
                }
            }
            if (posDistance < negDistance)
                return 1;
            else
                return -1;
        }
    }

    void rotateObject(float turnAngle)
    {
        Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, turnAngle, transform.rotation.eulerAngles.z);
        Quaternion targetRotation = Quaternion.Euler(direction);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void FixedUpdate()
    {

        // If the tank is moving then we plant a treadmark on every drop interval. The treadmarks spawn underneath the
        // center of the tank.
        if (moving)
        {
            treadTimer += Time.deltaTime;
            if (treadTimer >= treadDropInterval && treadDropInterval != 0)
            {
                GameObject.Instantiate(treadmarkPrefab, treadSpawner.transform.position, treadSpawner.transform.rotation);
                treadTimer = 0;
            }

            // This if statement could also be used for playing sounds during tank movement and other elements that 
            // only change when the tank is moving.
        }
    }

    public void ReturnToStart()
    {
        transform.position = startingPosition;
        player = GameObject.FindGameObjectWithTag("Player");
    }
}