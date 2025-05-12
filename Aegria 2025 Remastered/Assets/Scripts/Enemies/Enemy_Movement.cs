using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {
    float currentTime;
    int ticksForMovement;
    const int TICKSPERSECOND = 60;
    public bool isMoving;
    int xPathIndex;
    int yPathIndex;
    float secondsToLaunch;
    private Vector3 defaultPosition;
    private float debugDelay;
    private float timeMult = 1.0f;
    public bool hasDebug = false;
    public int index;

    private const int expandValue = 10;

    float lastAngle;
    // Use this for initialization
    void Start() {
        currentTime = 0f;
        isMoving = false;
        defaultPosition = this.transform.position;
        lastAngle = this.transform.rotation.y;
        //defaultPosition = new Vector3(10, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (isMoving && (currentTime - secondsToLaunch) * timeMult > 0)
        {
            transform.position = Vector3.Lerp(transform.position, getPosition(xPathIndex, yPathIndex, (currentTime - secondsToLaunch) * timeMult), (currentTime - secondsToLaunch));

            if (currentTime * timeMult >= 12 + secondsToLaunch)
            {
                isMoving = false;
            }
        }

        Vector3 vec = GetVelocity();

        float yAngle = 0;
        if (isMoving)
        {
            if (currentTime <= 1)
            {
                //set it to angle
                yAngle = GetAngleFrom2Pos(defaultPosition, new Vector3(0, 0, 1));
            }
            else if (currentTime <= 11)
            {
                //set it to derivative
                yAngle = GetAngleFrom2Floats(GetDerivativeAtIndex(xPathIndex, currentTime - 1), GetDerivativeAtIndex(yPathIndex, currentTime - 1));
                // yAngle = 0;
            }
            else
            {
                //set it to -angle
                yAngle = GetAngleFrom2Pos(new Vector3(0, 0, 1), defaultPosition);
            }

        }
        else
            GetComponent<Rigidbody>().velocity = vec;

        defaultPosition += vec * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, yAngle + 180, 0);

        if (hasDebug)
        {
            Debug.Log("Calculated angle is:");
            Debug.Log(yAngle);
            Debug.Log("My angle is:");
            Debug.Log(transform.rotation);

        }
    }

    Vector3 getPosition(int pathX, int pathY, float time)
    {
        //second 1: move downwards
        if (time < 1)
        {
            return GetLinearMovement(defaultPosition, new Vector3(0, 0, 1), 1f, time);
        }
        //second 11: move upwards - back to pos
        else if (time > 11)
        {
            return GetLinearMovement(new Vector3(0, 0, 1), defaultPosition, 1f, time - 11);
        }

        time -= 1;


        return new Vector3(GetValueAtIndex(pathX, time), 0, GetValueAtIndex(pathY, time) + 1);
    }

    public void StartMovement(int xPathIndex, int yPathIndex, float secondsToLaunch)

    {
        isMoving = true;
        this.xPathIndex = xPathIndex;
        this.yPathIndex = yPathIndex;
        this.secondsToLaunch = secondsToLaunch;
        currentTime = 0;
    }

    public Vector3 GetLinearMovement(Vector3 start, Vector3 end, float time, float elapsedTime)
    {
        float movementPercentage = (time - elapsedTime) / time;
        Vector3 displacement = end - start;
        return (end - (displacement * movementPercentage));

    }
    float GetValueAtIndex(int i, float t)
    {
        float[] values = new float[]
            {
            //X movements
            Mathf.Sin(t * 1f * Mathf.PI) * 50,
            (t * (t - 10) * Mathf.Tan(t)),
            //Y movements
            (Mathf.Sin(t * .1f * Mathf.PI) * -30),
            t * (t-10) * Mathf.Abs(Mathf.Tan(t)) * -1,
            Mathf.Abs(t * (t-10) * (t - 5))
            };
        return -values[i];
    }
    float GetDerivativeAtIndex(int i, float t)
    {
        float thisVal = GetValueAtIndex(i, t);
        float lastVal = GetValueAtIndex(i, t - 0.001f);
        return ((thisVal - lastVal) / 0.001f);
    }
    float GetAngleFrom2Pos(Vector3 locOne, Vector3 locTwo)
    {
        if (locOne.z > locTwo.z)
        {
            return Mathf.Atan((locOne.x - locTwo.x) / (locOne.z - locTwo.z)) * 180 / Mathf.PI;
        }
        return Mathf.Atan((locOne.x - locTwo.x) / (locOne.z - locTwo.z)) * 180 / Mathf.PI + 180;
    }
    float GetAngleFrom2Floats(float xDer, float yDer)
    {
        return (GetAngleFrom2Pos(new Vector3(0, 0, 0), new Vector3(xDer,0, yDer)));
    }

    Vector3 GetVelocity()
    {
        Vector3 vec = Vector3.zero;

        float verticalDisplacment = (60 - (70 - ((70 - (40 - 2 * expandValue)) * (1f / 3f)))) / 2f;
        float horizontalDisplacement = (((65 - (((90 + 4 * expandValue) / 9f) * 4)) - 5) / 1f);

        if (index > 30)
            vec += new Vector3(0, 0, -expandValue * Mathf.Sin(currentTime / 2));
        else if (index <= 30 && index > 20)
            vec += new Vector3(0, 0, -verticalDisplacment * 2 * Mathf.Sin(currentTime / 2));
        else if (index <= 20 && index > 10)
            vec += new Vector3(0, 0, -verticalDisplacment * Mathf.Sin(currentTime / 2));


        if (index % 10 == 0)
            vec += new Vector3(expandValue * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 1)
            vec += new Vector3(-expandValue * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 2)
            vec += new Vector3(-horizontalDisplacement * 4 * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 3)
            vec += new Vector3(-horizontalDisplacement * 3 * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 4)
            vec += new Vector3(-horizontalDisplacement * 2 * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 5)
            vec += new Vector3(-horizontalDisplacement * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 6)
            vec += new Vector3(horizontalDisplacement * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 7)
            vec += new Vector3(horizontalDisplacement * 2 * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 8)
            vec += new Vector3(horizontalDisplacement * 3 * Mathf.Sin(currentTime / 2), 0, 0);
        else if (index % 10 == 9)
            vec += new Vector3(horizontalDisplacement * 4 * Mathf.Sin(currentTime / 2), 0, 0);

        return vec;
    }
}
