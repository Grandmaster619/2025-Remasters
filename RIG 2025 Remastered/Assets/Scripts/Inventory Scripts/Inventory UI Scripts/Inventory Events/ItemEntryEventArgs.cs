using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntryEventArgs : EventArgs
{
    ItemEntryUI itemEntryUI;

    public ItemEntryEventArgs(ItemEntryUI itemEntryUI)
    {
        this.itemEntryUI = itemEntryUI;
    }

    public ItemEntryUI GetItemEntryUI() { return itemEntryUI; }
}
