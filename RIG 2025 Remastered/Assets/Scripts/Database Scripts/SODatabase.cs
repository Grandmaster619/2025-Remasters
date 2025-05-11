using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SODatabase : MonoBehaviour
{
    private static SODatabase instance;
    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public ItemSO GetItemByID(int id)
    {
        if (id >= itemDatabase.GetItemList().Count)
            return null;

        return itemDatabase.GetItemList()[id];
    }

    public ItemSO GetItemByName(string itemName)
    {
        Debug.Log(itemName + "ItemByName entered");
        ItemSO itemSO = null;
        Debug.Log(itemDatabase.GetItemList().Count);
        foreach (ItemSO item in itemDatabase.GetItemList())
        {
            if (itemName.Equals(item.GetItemName()))
            {
                itemSO = item;
                break;
            }
        }

        return itemSO;
    }

    public static SODatabase GetInstance() { return instance; }
}
