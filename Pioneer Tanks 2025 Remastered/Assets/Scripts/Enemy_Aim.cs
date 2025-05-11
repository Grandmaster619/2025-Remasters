using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aim : MonoBehaviour 
{
    [SerializeField] private GameObject turret;

    private int layerMask, playerLayer, enemyLayer;
    // Use this for initialization
    void Start()
    {
        // Bit shift the index of the layer (8) to get a bit mask

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        int wallLayer = LayerMask.NameToLayer("Wall");
        layerMask = (1 << wallLayer) | (1 << playerLayer) | (1 << enemyLayer);


    }
	
	// Update is called once per frame
	void Update ()
    {
        GetAimedAt();
        TestingFunction();
    }


    void TestingFunction()
    {
        this.transform.Rotate(-(Vector3.forward * 5f * Time.deltaTime));
    }

    void GetAimedAt()
    {
        AimBounce(turret.transform.position, this.transform.up, 3);
    }

    RaycastHit AimBounce(Vector3 startPos, Vector3 normalizedDir, int numBounces, RaycastHit lastHit=new RaycastHit())
    {
        Debug.Log("Bounce number: " + numBounces + "Reflection Vector: " + normalizedDir);
        if(numBounces >= 1)
        {
            RaycastHit hit;
            // Does the ray intersect any Walls, Players, or Enemies
            if (Physics.Raycast(startPos, normalizedDir, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.layer == playerLayer)
                {
                    Debug.DrawLine(startPos, hit.point, Color.green);
                    return hit;
                }
                else if (hit.transform.gameObject.layer == enemyLayer)
                {
                    Debug.DrawLine(startPos, hit.point, Color.red);
                    return hit;
                }
                else
                {
                    Debug.DrawLine(startPos, hit.point, Color.white);
                    // Find the line from the gun/wall to the point that was clicked.
                    Vector3 incomingVec = hit.point - startPos;

                    // Use the point's normal to calculate the reflection vector.
                    Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
                    //Debug.Log("Bounce number: " + numBounces + "Reflection Vector: " + reflectVec);
                    return AimBounce(hit.point + (reflectVec.normalized * 0.1f), reflectVec.normalized, numBounces - 1, hit);
                }
            }
            
        }
        else
        {
            RaycastHit hit;
            // Does the ray intersect any Walls, Players, or Enemies
            if (Physics.Raycast(startPos, normalizedDir, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.layer == playerLayer)
                    Debug.DrawLine(startPos, hit.point, Color.green);
                else if (hit.transform.gameObject.layer == enemyLayer)
                    Debug.DrawLine(startPos, hit.point, Color.red);
                else
                    Debug.DrawLine(startPos, hit.point, Color.black);
                return hit;
            }
        }
        return lastHit;
    }


}
