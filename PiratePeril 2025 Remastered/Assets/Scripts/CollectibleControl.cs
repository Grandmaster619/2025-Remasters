using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleControl : MonoBehaviour
{
    public Collectibles type;
    public AudioSource collectedSound;

    private PersistentControl persistentObject;

    private void Awake()
    {
        persistentObject = GameObject.FindWithTag("Persistent").GetComponent<PersistentControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            persistentObject.Collected.Add(type);
            collectedSound.PlayOneShot(collectedSound.clip);
            collision.gameObject.BroadcastMessage("UpdateCollectibles", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
