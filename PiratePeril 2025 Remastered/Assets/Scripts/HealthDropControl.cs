using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.BroadcastMessage("AddHealth", 1, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
