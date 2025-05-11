using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    public Light lightObj;
    public GameObject solidObj;
    private Material lightMat;
    public float interval = 1;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        lightMat = solidObj.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            lightObj.enabled = !lightObj.enabled;
            if (!lightObj.enabled)
                lightMat.DisableKeyword("_EMISSION");
            else
                lightMat.EnableKeyword("_EMISSION");
            timer -= interval;
        }
    }
}