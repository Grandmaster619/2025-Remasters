using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry
{
    private const int MIN_ITEM_STACK_AMOUNT = 1;
    private const int MAX_ITEM_STACK_AMOUNT = 99;

    public ItemSO itemSO;
    [Range(MIN_ITEM_STACK_AMOUNT, MAX_ITEM_STACK_AMOUNT)] public int amt;

    public InventoryEntry(ItemSO itemSO, int amt)
    {
        this.itemSO = itemSO;
        this.amt = amt;
    }

    public ItemSO GetItemSO() { return itemSO; }

    public int GetAmount() { return amt; }

    public void SetAmount(int amt) { this.amt = amt; }
}

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<InventoryEntry> inventoryEntryList;

    public void Start()
    {
        if(InventoryLoader.GetInstance().GetInventoryEntries() != null) {
            inventoryEntryList = InventoryLoader.GetInstance().GetInventoryEntries();
            InventoryLoader.GetInstance().Clear();
        }
    }

    public void AddItem(ItemSO itemSO, int amt)
    {
        for(int i = 0; i < inventoryEntryList.Count; i++) {
            if (inventoryEntryList[i].itemSO == itemSO) {
                Debug.Log("Existing item stack found in player inventory");
                inventoryEntryList[i].amt += amt;
                return;
            }
        }

        Debug.Log("New Item Stack added to player inventory");
        inventoryEntryList.Add(new InventoryEntry(itemSO, amt));
    }

    public void RemoveItem(ItemSO itemSO)
    {
        for (int i = inventoryEntryList.Count - 1; i >= 0; i--) {
            if (inventoryEntryList[i].GetItemSO().GetItemName() == itemSO.GetItemName()) {
                if (inventoryEntryList[i].GetAmount() > 1)
                    inventoryEntryList[i].SetAmount(inventoryEntryList[i].GetAmount() - 1);
                else
                    inventoryEntryList.RemoveAt(i);
            }
        }
    }

    public bool Contains(ItemSO itemSO)
    {
        Debug.Log("Contains entered");
        for(int i = 0; i < inventoryEntryList.Count; i++) {
            if (inventoryEntryList[i].GetItemSO().GetItemName().Equals(itemSO.GetItemName()))
                return true;
        }

        return false;
    }

    public int ContainsAmount(ItemSO itemSO)
    {
        for (int i = 0; i < inventoryEntryList.Count; i++)
        {
            if (inventoryEntryList[i].GetItemSO().GetItemName().Equals(itemSO.GetItemName()))
                return inventoryEntryList[i].GetAmount();
        }

        return 0;
    }

    public List<InventoryEntry> GetInventoryEntryList() { return inventoryEntryList; }
}
