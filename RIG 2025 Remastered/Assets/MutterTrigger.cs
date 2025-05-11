using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MutterTrigger : MonoBehaviour
{
    private BoxCollider myHitbox;
    // Start is called before the first frame update
    void Start()
    {
        myHitbox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        myHitbox.enabled = false;
        GetComponent<Animator>().SetBool("mutter", true);
    }
}
