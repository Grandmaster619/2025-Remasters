using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    private PersistentController persistent;

    void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(persistent.PAUSED)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
