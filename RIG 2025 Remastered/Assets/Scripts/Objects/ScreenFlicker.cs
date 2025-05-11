using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlicker : MonoBehaviour
{
    public GameObject solidObj;
    public int materialIndex = 0; // Index of the material you want to modify
    private Material[] materials;
    private Material lightMat;
    public float interval = 1;
    float timer;
    bool state = true;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = solidObj.GetComponent<Renderer>();
        materials = renderer.materials;

        if (materialIndex >= 0 && materialIndex < materials.Length)
        {
            // Make sure the specified material index is valid
            lightMat = materials[materialIndex];
        }
        else
        {
            Debug.LogError("Invalid material index specified!");
            enabled = false; // Disable the script to prevent errors
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            if (state == false)
            {
                lightMat.DisableKeyword("_EMISSION");
                state = true;
            }
            else
            {
                lightMat.EnableKeyword("_EMISSION");
                state = false;
            }

            interval = Random.Range(0f, 1f);
            timer = 0;
        }
    }
}
