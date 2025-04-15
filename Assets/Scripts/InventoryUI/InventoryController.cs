using Ink.Parsed;
using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private InventoryUIPage inventoryPage; // Reference to the inventory page UI
        [SerializeField]
        private InventorySO inventoryData; // Reference to the inventory data

        public List<InventoryItem> initialItems = new List<InventoryItem>();
        public bool InventoryIsActive => inventoryPage.isActiveAndEnabled; // Property to check if the inventory is active

        private InputSystem_Actions inputSystem; // Reference to the input system

        private void Awake()
        {
            inputSystem = new InputSystem_Actions(); // Initialize the input system

            PrepareUI();
            PrepareInventoryData();
        }

        internal void OnEnable()
        {
            inputSystem.Enable(); // Enable the input system
        }

        internal void OnDisable()
        {
            inputSystem.Disable(); // Disable the input system
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize(); // Initialize the inventory data
            inventoryData.OnInventoryUpdated += GetUpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (!item.IsEmpty)
                {
                    inventoryData.AddItem(item);
                }
            }
        }

        private void GetUpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryPage.ResetAllItems();
            foreach(var item in inventoryState)
            {
                inventoryPage.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryPage.InitializeInventoryUI(inventoryData.Size); // Initialize the inventory UI with the specified size 
            
            // Subscribe to the item events
            inventoryPage.OnDescriptionRequested += HandleDescriptionRequested; // Subscribe to the description request event
            inventoryPage.OnItemActionRequested += HandleItemActionRequested; // Subscribe to the item action request event
            inventoryPage.OnStartDragging += HandleItemDragStart; // Subscribe to the item drag start event
            inventoryPage.OnItemSwapped += HandleItemSwap; // Subscribe to the item swap event
        }

        private void HandleDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex); // Get the item at the specified index
            if (!inventoryItem.IsEmpty)
            {
                ItemSO item = inventoryItem.item; // Get the item scriptable object
                inventoryPage.UpdateDescription(itemIndex, item.Image, item.name, item.Description); // Update the inventory UI with the item data
            }
            else
            {
                inventoryPage.ResetSelection(); // Clears the Inventory Description if item is empty
                return;
            }
        }

        private void HandleItemActionRequested(int itemIndex)
        {

        }

        private void HandleItemDragStart(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if(!inventoryItem.IsEmpty)
            {
                inventoryPage.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
            }
        }

        private void HandleItemSwap(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        public void Update() //Checks if OpenInventory key is triggered
        {
            if (inputSystem.Player.OpenInventory.triggered)
            {
                if (!inventoryPage.isActiveAndEnabled && !DialogueManager.Instance.DialogueIsActive)
                {
                    // If the inventory page is not active and dialogue is not active, show the inventory page
                    inventoryPage.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryPage.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity); // Update the inventory UI with the current inventory state
                    }

                    InventoryItem firstItem = inventoryData.GetItemAt(0); // Get the first item in the inventory
                    if (!firstItem.IsEmpty)
                    {
                        inventoryPage.UpdateDescription(0, firstItem.item.Image, firstItem.item.name, firstItem.item.Description); // Update the inventory UI with the first item data
                    }
                    else
                    {
                        inventoryPage.ResetSelection(); // Clears the Inventory Description if item is empty
                        return;
                    }
                }
                else if (inventoryPage.isActiveAndEnabled && !DialogueManager.Instance.DialogueIsActive)
                {
                    // If the inventory page is active and dialogue is not active, hide the inventory page
                    inventoryPage.Hide();
                }
            }
        }
    }
}