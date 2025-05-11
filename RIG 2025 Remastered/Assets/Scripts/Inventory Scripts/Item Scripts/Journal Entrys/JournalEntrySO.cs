using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalEntry", menuName = "Item/Journal Entry", order = 200)]
public class JournalEntrySO : ItemSO
{
    [SerializeField, TextArea] private string journalEntry;

    public override void Use(GameObject playerObject)
    {
        InventoryMenuController.GetInstance().OpenJournal(this);
        PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
        Debug.Log("Journal Entry " + itemName + " opened.");
    }

    public string GetJournalEntryText() { return journalEntry; }
}
