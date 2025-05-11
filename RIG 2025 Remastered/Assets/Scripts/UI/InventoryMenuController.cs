using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum InventoryState
{
    None,
    Inventory,
    Journal
}

public class InventoryMenuController : MonoBehaviour
{
    private static InventoryMenuController instance;

    //Open and close inventory events
    public EventHandler<CancelableEventArgs> OnInventoryOpening;
    public EventHandler<EventArgs> OnInventoryOpened;
    public EventHandler<EventArgs> OnInventoryClosed;

    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private PlayerInventoryUI playerInventoryUI;
    [SerializeField] private JournalEntryUI journalEntryUI;
    private InventoryState inventoryState;
    private bool canOpenInventoryMenu;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("An instance of InventoryMenuController already exists");
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        inventoryState = InventoryState.None;
        canOpenInventoryMenu = true;
        gameObject.SetActive(true);
        PauseMenu.GetInstance().OnPauseMenuOpened += CloseInventoryOnPauseMenuOpened;
    }

    public void Update()
    {
        if (!canOpenInventoryMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Tab)) {
            switch (inventoryState) {
                case InventoryState.None:
                    OpenPlayerInventory(playerInventory);
                    break;
                case InventoryState.Inventory:
                    ClosePlayerInventory();
                    break;
                case InventoryState.Journal:
                    CloseJournal();
                    break;
            }
        }
    }

    public void OpenPlayerInventory(PlayerInventory playerInventory)
    {
        //*** Inventory Opening Event ***
        CancelableEventArgs eventArgs = new CancelableEventArgs();
        OnInventoryOpening.Invoke(this, eventArgs);

        if (eventArgs.IsCanceled())
            return;

        playerInventoryUI.OpenPlayerInventory(playerInventory);
        inventoryState = InventoryState.Inventory;

        // *** Inventory Opened Event ***
        OnInventoryOpened.Invoke(this, new EventArgs());
    }

    public void ClosePlayerInventory()
    {
        // *** Inventory Closed Event ***
        OnInventoryClosed.Invoke(this, new EventArgs());

        playerInventoryUI.ClosePlayerInventory();
        inventoryState = InventoryState.None;
    }

    public void OpenJournal(JournalEntrySO journalEntry)
    {
        playerInventoryUI.ClosePlayerInventory();
        journalEntryUI.OpenJournalEntryUI(journalEntry);
        inventoryState = InventoryState.Journal;
    }

    public void CloseJournal()
    {
        journalEntryUI.CloseJournalEntryUI();
        playerInventoryUI.OpenPlayerInventory(playerInventory);
        inventoryState = InventoryState.Inventory;
    }

    public void ForceCloseAllMenus()
    {
        if (inventoryState == InventoryState.None)
            return;

        if (inventoryState == InventoryState.Inventory)
            ClosePlayerInventory();
        else {
            CloseJournal();
            ClosePlayerInventory();
        }
    }

    private void CloseInventoryOnPauseMenuOpened(object sender, EventArgs eventArgs)
    {
        ForceCloseAllMenus();
    }

    public InventoryState GetInventoryState() { return inventoryState; }

    public static InventoryMenuController GetInstance() {  return instance; }
}
