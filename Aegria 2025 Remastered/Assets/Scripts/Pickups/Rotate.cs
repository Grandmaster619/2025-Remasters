using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField] private float speed = 25;
    void Update()
    {
        this.transform.Rotate(new Vector3(35, -30, 65) * Time.deltaTime);
        
        if(this.tag != "newShield")
            this.transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
    }
}
