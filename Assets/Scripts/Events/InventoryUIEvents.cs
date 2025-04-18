using Inventory.UI;
using System;
using UnityEngine;

public class InventoryUIEvents
{
    // Initialization of InventoryUI Events

    // InventoryPage Items Events
    public event Action<int> OnDescriptionRequested; // Event for Item Description UI Updates
    public event Action<int> OnItemActionRequested; // Event for Handling Actions Done by Items
    public event Action<int> OnStartDragging; // Event for Handling Dragging Items in Inventory
    public event Action<int, int> OnItemSwapped; // Event for Handling Swapping Items or Slots in Inventory


    //InventoryItems Events
    public event Action<InventoryUIItem> OnItemClicked; // Event for Clicking the Item
    public event Action<InventoryUIItem> OnItemDroppedOn; // Event for When the Mouse is Released on the Item
    public event Action<InventoryUIItem> OnItemBeginDrag; // Event for When the Mouse is Dragging the Item
    public event Action<InventoryUIItem> OnItemEndDrag; // Event for When the Mouse Stops Dragging the Item
    public event Action<InventoryUIItem> OnItemRightMouseButtonClicked; //Event for Right-Clicking the Item

    // Functions for Invoking Input Events
    // InventoryPage Items
    public void DescriptionRequested(int itemIndex)
    {
        OnDescriptionRequested?.Invoke(itemIndex);
    }

    public void ItemActionRequested(int itemIndex)
    {
        OnItemActionRequested?.Invoke(itemIndex);
    }

    public void StartDragging(int itemIndex)
    {
        OnStartDragging?.Invoke(itemIndex);
    }

    public void ItemSwapped(int itemIndex1, int itemIndex2)
    {
        OnItemSwapped?.Invoke(itemIndex1, itemIndex2);
    }

    //InventoryItems
    public void ItemClicked(InventoryUIItem item)
    {
        OnItemClicked?.Invoke(item);
    }

    public void ItemDroppedOn(InventoryUIItem item)
    {
        OnItemDroppedOn?.Invoke(item);
    }

    public void ItemBeginDrag(InventoryUIItem item)
    {
        OnItemBeginDrag?.Invoke(item);
    }

    public void ItemEndDrag(InventoryUIItem item)
    {
        OnItemEndDrag?.Invoke(item);
    }

    public void ItemRightMouseButtonClicked(InventoryUIItem item)
    {
        OnItemRightMouseButtonClicked?.Invoke(item);
    }
}
