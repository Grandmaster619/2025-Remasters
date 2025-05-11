using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemEntryPrefab;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Image itemDescriptionPanelImage;
    [SerializeField] private TextMeshProUGUI itemDescriptionPanelText;
    [SerializeField] private GameObject itemEntriesParentObject;

    public void Start()
    {
        itemDescriptionPanelText.SetText("");
        gameObject.SetActive(false);
    }

    public void OpenPlayerInventory(PlayerInventory playerInventory)
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

        for(int i = 0; i < playerInventory.GetInventoryEntryList().Count; i++) {
            GameObject itemEntryObject = Instantiate(itemEntryPrefab, itemEntriesParentObject.transform);
            ItemEntryUI itemEntryUI = itemEntryObject.GetComponent<ItemEntryUI>();
            itemEntryUI.SetItemEntryProperties(playerInventory.GetInventoryEntryList()[i], playerInventory.gameObject);
            itemEntryUI.OnPointerEnterItemEntry += ChangeDescriptionPanelProperties;
        }

        gameObject.SetActive(true);
    }

    public void ClosePlayerInventory()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        gameObject.SetActive(false);
        foreach (Transform childObject in itemEntriesParentObject.transform)
            Destroy(childObject.gameObject);
    }

    private void ChangeDescriptionPanelProperties(object sender, ItemEntryEventArgs eventArgs)
    {
        itemDescriptionPanelImage.sprite = eventArgs.GetItemEntryUI().GetItemSO().GetSprite();
        itemDescriptionPanelText.SetText(eventArgs.GetItemEntryUI().GetItemSO().GetDescription());
    }
}
