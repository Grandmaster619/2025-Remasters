using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] private ItemSO item;
    [SerializeField][Range(1, 99)] private int amt;
    [SerializeField] private AudioSource src;

    protected override void Interact(GameObject sender)
    {
        Debug.Log("Interact for Item Interact called");
        if (sender.TryGetComponent(out PlayerInventory playerInventory))
        {
            Debug.Log("The player has picked up an item: " + item);
            playerInventory.AddItem(item, amt);
            Destroy(gameObject);
            if (src != null) { Instantiate(src); }
        }
    }
}
