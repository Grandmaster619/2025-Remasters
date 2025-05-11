using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalCollectable : MonoBehaviour
{
    private GameObject player;
    private Inventory playerInventory;
    private CollectableManager collectableManager;
    public int journalId;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            playerInventory = player.GetComponent<Inventory>();

            if (playerInventory == null)
            {
                Debug.LogError("Inventory component not found on player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found with tag 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory.JournalCollected(journalId);
            journalId++;
            //collectableManager.MarkCollectableAsCollected("Sample Entry 5");
            Debug.Log("Flashlight Collected");
            gameObject.SetActive(false);
        }   
    }
}
