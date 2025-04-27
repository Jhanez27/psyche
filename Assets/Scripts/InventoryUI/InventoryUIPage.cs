using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class InventoryUIPage : MonoBehaviour
    {
        [SerializeField]
        private InventoryUIItem itemPrefab; // Prefab for the inventory item UI
        [SerializeField]
        private RectTransform contentPanel; // Panel to hold the inventory items
        [field: SerializeField]
        public InventoryUIDescription inventoryDescription { get; private set; } // Reference to the inventory description UI

        [SerializeField]
        private MouseFollower mouseFollower; // Reference to the mouse follower UI

        List<InventoryUIItem> inventoryItems = new List<InventoryUIItem>(); // List to hold the inventory items

        private int currentDraggedItemIndex = -1; // Index of the currently dragged item

        [SerializeField]
        private ItemActionPanel itemActionPanel; // Reference to the item action panel UI

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false); // Hide the mouse follower UI
            inventoryDescription.ResetDescription(); // Reset the inventory description UI
        }
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryUIItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); // Create a new inventory item
                newItem.transform.SetParent(contentPanel, false); // Set the parent to the content panel
                inventoryItems.Add(newItem); // Add the new item to the list

                //Subscribe to item events
                GamesEventManager.Instance.inventoryUIEvents.OnItemClicked += HandleItemSelection;
                GamesEventManager.Instance.inventoryUIEvents.OnItemBeginDrag += HandleItemBeginDrag;
                GamesEventManager.Instance.inventoryUIEvents.OnItemEndDrag += HandleItemEndDrag;
                GamesEventManager.Instance.inventoryUIEvents.OnItemDroppedOn += HandleItemSwap;
                GamesEventManager.Instance.inventoryUIEvents.OnItemRightMouseButtonClicked += HandleShowItemActions;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (inventoryItems.Count > itemIndex)
            {
                inventoryItems[itemIndex].SetData(itemImage, itemQuantity); // Set the data for the inventory item
            }
        }
        private void HandleItemSelection(InventoryUIItem item)
        {
            int index = inventoryItems.IndexOf(item); // Get the index of the clicked item
            if (index == -1)
                return;

            GamesEventManager.Instance.inventoryUIEvents.DescriptionRequested(index); // Invoke the item description request event
        }

        private void HandleItemBeginDrag(InventoryUIItem item)
        {
            int index = inventoryItems.IndexOf(item); // Get the index of the dragged item
            if (index == -1)
                return;

            currentDraggedItemIndex = index; // Set the current dragged item index
            HandleItemSelection(item); // Handle item selection
            GamesEventManager.Instance.inventoryUIEvents.StartDragging(index); // Invoke the item drag start event
        }

        private void HandleItemEndDrag(InventoryUIItem item)
        {
            ResetDraggedItem(); // Reset the dragged item
        }

        private void HandleItemSwap(InventoryUIItem item)
        {
            int index = inventoryItems.IndexOf(item); // Get the index of the dragged item
            if (index == -1)
            {
                return;
            }

            GamesEventManager.Instance.inventoryUIEvents.ItemSwapped(currentDraggedItemIndex, index); // Invoke the item swap event
            HandleItemSelection(item);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false); // Hide the mouse follower UI
            currentDraggedItemIndex = -1; // Reset the current dragged item index
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true); // Show the mouse follower UI
            mouseFollower.SetData(sprite, quantity); // Set the data for the mouse follower UI
        }

        private void HandleShowItemActions(InventoryUIItem item)
        {
            Debug.Log("Right Clicked");
            int index = inventoryItems.IndexOf(item); // Get the index of the right-clicked item
            if (index == -1)
                return;

            GamesEventManager.Instance.inventoryUIEvents.ItemActionRequested(index); // Invoke the item action request event
        }

        public void Show()
        {
            gameObject.SetActive(true); // Show the inventory UI
            ResetSelection(); // Reset the item selection
        }

        public void Hide()
        {
            itemActionPanel.Toggle(false); // Hide the item action panel
            gameObject.SetActive(false); // Hide the inventory UI
            ResetDraggedItem(); // Reset the dragged item
        }

        public void ResetSelection()
        {
            inventoryDescription.ResetDescription(); // Reset the inventory description UI
            DeselectAllItems(); // Deselect all items
        }

        public void AddAction(string actionName, Action performAction)
        {
            itemActionPanel.AddButton(actionName, performAction); // Add an action button to the item action panel
        }

        public void ShowItemAction(int itemIndex)
        {
            itemActionPanel.Toggle(true); // Show the item action panel
            itemActionPanel.transform.position = inventoryItems[itemIndex].transform.position; // Set the position of the item action panel
        }

        private void DeselectAllItems()
        {
            foreach (InventoryUIItem item in inventoryItems)
            {
                item.Deselect(); // Deselect each item
            }
            itemActionPanel.Toggle(false); // Hide the item action panel
        }

        public void UpdateDescription(int itemIndex, Sprite sprite, string name, string description)
        {
            inventoryDescription.SetDescription(sprite, name, description); // Set the item description
            DeselectAllItems(); // Deselect all items
            inventoryItems[itemIndex].Select();
        }

        public void ResetAllItems()
        {
            foreach(InventoryUIItem item in inventoryItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}