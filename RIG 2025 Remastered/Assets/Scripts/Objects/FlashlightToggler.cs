using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggler : MonoBehaviour
{
    public GameObject ltar;
    private bool state = false;
    public ItemSO flashlight;

    void Start()
    {
        ltar.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GetComponentInParent<PlayerInventory>().Contains(flashlight)) {
            //print("!!!");
            state=!state;
            ltar.SetActive(state);
        }
    }
}
