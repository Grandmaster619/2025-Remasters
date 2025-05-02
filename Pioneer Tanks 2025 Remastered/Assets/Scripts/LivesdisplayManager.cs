using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesdisplayManager : MonoBehaviour
{
    private PersistentController persistent;


    // Start is called before the first frame update
    void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "x" + persistent.CurLives.ToString();
    }
}
