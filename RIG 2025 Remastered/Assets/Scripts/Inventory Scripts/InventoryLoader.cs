using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoader : MonoBehaviour
{
    private static InventoryLoader instance;

    private List<InventoryEntry> inventoryEntries;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void Clear()
    {
        inventoryEntries = null;
    }

    public List<InventoryEntry> GetInventoryEntries() { return inventoryEntries; }

    public void SetInventoryEntries(List<InventoryEntry> inventoryEntries) { this.inventoryEntries = inventoryEntries; }

    public static  InventoryLoader GetInstance() { return instance; }
}
