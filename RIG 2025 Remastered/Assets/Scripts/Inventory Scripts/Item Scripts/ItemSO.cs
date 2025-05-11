using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    CONSUMABLE,
    KEY,
    JOURNAL_ENTRY
}

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField, TextArea] protected string description;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private ItemType itemType;

    public abstract void Use(GameObject playerObject);

    public string GetItemName() { return itemName; }

    public string GetDescription() { return description; }

    public Sprite GetSprite() { return itemImage; }

    public ItemType GetItemType() {  return itemType; }
}
