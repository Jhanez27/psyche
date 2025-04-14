using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField]
    private InventoryItem itemPrefab; // Prefab for the inventory item UI
    [SerializeField]
    private RectTransform contentPanel; // Panel to hold the inventory items
    [SerializeField]
    private InventoryDescription inventoryDescription; // Reference to the inventory description UI

    [SerializeField]
    private MouseFollower mouseFollower; // Reference to the mouse follower UI

    List<InventoryItem> inventoryItems = new List<InventoryItem>(); // List to hold the inventory items

    private int currentDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested; // Event for item description request
    public event Action<int> OnItemActionRequested; // Event for item action request
    public event Action<int> OnStartDragging; // Event for item drag start
    public event Action<int, int> OnItemSwapped; // Event for item swap

    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false); // Hide the mouse follower UI
        inventoryDescription.ResetDescription(); // Reset the inventory description UI
    }
    public void InitializeInventoryUI(int inventorySize)
    {
        for(int i = 0; i < inventorySize; i++)
        {
            InventoryItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); // Create a new inventory item
            newItem.transform.SetParent(contentPanel, false); // Set the parent to the content panel
            inventoryItems.Add(newItem); // Add the new item to the list

            //Subscribe to item events
            newItem.OnItemClicked += HandleItemSelection;
            newItem.OnItemBeginDrag += HandleItemBeginDrag;
            newItem.OnItemEndDrag += HandleItemEndDrag;
            newItem.OnItemDroppedOn += HandleItemSwap;
            newItem.OnItemRightMouseButtonClick += HandleShowItemActions;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if(inventoryItems.Count > itemIndex)
        {
            inventoryItems[itemIndex].SetData(itemImage, itemQuantity); // Set the data for the inventory item
        }
    }
    private void HandleItemSelection(InventoryItem item)
    {
        int index = inventoryItems.IndexOf(item); // Get the index of the clicked item
        if(index == -1)
            return;

        OnDescriptionRequested?.Invoke(index); // Invoke the item description request event
    }

    private void HandleItemBeginDrag(InventoryItem item)
    {
        int index = inventoryItems.IndexOf(item); // Get the index of the dragged item
        if(index == -1)
            return;

        currentDraggedItemIndex = index; // Set the current dragged item index
        HandleItemSelection(item); // Handle item selection
        OnStartDragging?.Invoke(index); // Invoke the item drag start event
    }

    private void HandleItemEndDrag(InventoryItem item)
    {
        ResetDraggedItem(); // Reset the dragged item
    }

    private void HandleItemSwap(InventoryItem item)
    {
        int index = inventoryItems.IndexOf(item); // Get the index of the dragged item
        if(index == -1)
        {
            return;
        }

        OnItemSwapped?.Invoke(currentDraggedItemIndex, index); // Invoke the item swap event
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false); // Hide the mouse follower UI
        currentDraggedItemIndex = -1; // Reset the current dragged item index
    }

    private void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true); // Show the mouse follower UI
        mouseFollower.SetData(sprite, quantity); // Set the data for the mouse follower UI
    }

    private void HandleShowItemActions(InventoryItem item)
    {

    }

    public void Show()
    {
        gameObject.SetActive(true); // Show the inventory UI
        ResetSelection(); // Reset the item selection
    }

    public void Hide()
    {
        gameObject.SetActive(false); // Hide the inventory UI
        ResetDraggedItem(); // Reset the dragged item
    }

    private void ResetSelection()
    {
        inventoryDescription.ResetDescription(); // Reset the inventory description UI
        DeselectAllItems(); // Deselect all items
    }

    private void DeselectAllItems()
    {
        foreach(InventoryItem item in inventoryItems)
        {
            item.Deselect(); // Deselect each item
        }
    }
}
