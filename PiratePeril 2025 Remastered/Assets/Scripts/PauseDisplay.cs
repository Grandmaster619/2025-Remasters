using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDisplay : MonoBehaviour
{
    private PersistentControl persistentObject;

    private void Start()
    {
        persistentObject = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentControl>();
    }

    private void Update()
    {
        if (persistentObject.IsPaused())
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
