using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEntryUI : MonoBehaviour, IPointerEnterHandler
{
    private const int DESCRIPTION_CHARACTER_LIMIT = 36;

    public EventHandler<ItemEntryEventArgs> OnPointerEnterItemEntry;

    private InventoryEntry inventoryEntry;
    [SerializeField] private UnityEngine.UI.Button itemButton;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmtText;

    public void SetItemEntryProperties(InventoryEntry inventoryEntry, GameObject playerObject)
    {
        this.inventoryEntry = inventoryEntry;

        itemNameText.SetText(inventoryEntry.GetItemSO().GetItemName());
        if(inventoryEntry.GetItemSO().GetDescription().Length <=  DESCRIPTION_CHARACTER_LIMIT)
            itemDescriptionText.SetText(inventoryEntry.GetItemSO().GetDescription());
        else {
            string breifDescription = "";
            for(int i = 0; i <  DESCRIPTION_CHARACTER_LIMIT; i++)
                breifDescription = breifDescription + inventoryEntry.GetItemSO().GetDescription()[i];
            itemDescriptionText.SetText(breifDescription + "...");
        }
        itemImage.sprite = inventoryEntry.GetItemSO().GetSprite();
        itemAmtText.text = inventoryEntry.GetAmount().ToString();

        itemButton.onClick.AddListener(() => inventoryEntry.GetItemSO().Use(playerObject));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //*** Pointer Enter Item Entry Event ***
        OnPointerEnterItemEntry?.Invoke(this, new ItemEntryEventArgs(this));
    }

    public ItemSO GetItemSO() { return inventoryEntry.GetItemSO(); }
}
