using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModelEvents 
{
    // Initialization of InventoryModel Events (These involve the Item Logics)
    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
    public event Action<ItemSO, int> OnItemAdded;

    // Functions for Invoking InventoryModel Events
    public void InventoryUpdated(Dictionary<int, InventoryItem> inventory)
    {
        OnInventoryUpdated?.Invoke(inventory);
    }

    public void ItemAdded(ItemSO item, int quantity)
    {
        OnItemAdded?.Invoke(item, quantity);
    }
}
