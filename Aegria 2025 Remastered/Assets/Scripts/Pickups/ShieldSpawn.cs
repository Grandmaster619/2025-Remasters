using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawn : MonoBehaviour
{
    public GameObject shield;
    public float shieldTimer = 10.0f;     //How long the shield will be activated
    public bool shieldActivated = false;

    [SerializeField] private float remainingTime;

    private GameObject myShield;
    private float timeoutSpd, startTime;

    private void Update()
    {
        if (shieldActivated && remainingTime > 0)
        { 
            Color32 col = myShield.GetComponent<Renderer>().material.GetColor("_Color");
            col.a = (byte)Mathf.PingPong((Time.time - startTime) * timeoutSpd + 128, 192);
            myShield.GetComponent<Renderer>().material.SetColor("_Color", col);

            remainingTime -= Time.deltaTime;
            timeoutSpd = Mathf.Pow(100, (Time.time - startTime) / shieldTimer);
        }

        if (remainingTime <= 0)
        {
            base.transform.gameObject.tag = "Player";
            Destroy(myShield);
            GetComponent<ParticleReceiver>().enabled = true;
            shieldActivated = false;
        }
    }

    void OnTriggerEnter(Collider other)                    //Picking up the shield
    {
        if (other.gameObject.tag == "Shield")
        {
            Destroy(other.gameObject);

            if (myShield == null)
            {
                myShield = Instantiate(shield, transform.position, transform.rotation, transform);
                base.transform.gameObject.tag = "Shield";
                GetComponent<ParticleReceiver>().enabled = false;
            }

            remainingTime += shieldTimer;

            shieldActivated = true;

            timeoutSpd = 1;
            startTime = Time.time;
        }
    }
}