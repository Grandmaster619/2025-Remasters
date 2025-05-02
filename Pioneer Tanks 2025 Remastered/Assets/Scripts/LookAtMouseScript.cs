using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouseScript : MonoBehaviour
{
    public Transform target;
    public LayerMask mouseLayer;

    private PersistentController persistent;

    void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();
    }

    void Update()
    {
        if (!persistent.PAUSED)
        {
            Vector3 v3T = Input.mousePosition;
            v3T.y = 0.3f;
            v3T = Camera.main.ScreenToWorldPoint(v3T);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 100000, mouseLayer))
            {
                v3T = hitData.point;
                // Debug.Log("Ray Found" + v3T.ToString());
            }
            transform.LookAt(v3T);
            transform.localEulerAngles = new Vector3(-90f, transform.localEulerAngles.y, transform.localEulerAngles.z + 90);
        }
    }

}
