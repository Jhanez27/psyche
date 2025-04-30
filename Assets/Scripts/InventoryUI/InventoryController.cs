using Characters.Handlers;
using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;

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
        private bool canOpenInventory = true; // Flag to check if the inventory can be opened

        [SerializeField]
        private AudioClip dropClip;
        [SerializeField]
        private AudioSource audioSource;

        private void Awake()
        {
            PrepareUI();
            PrepareInventoryData();
        }
        private void OnEnable()
        {
            GamesEventManager.Instance.inputEvents.OnInventoryTogglePressed += InventoryTogglePressed;
            GamesEventManager.Instance.inventoryUIEvents.OnInventoryEnabled += EnableInventory; // Subscribe to the inventory enabled event
            GamesEventManager.Instance.inventoryUIEvents.OnInventoryDisabled += DisableInventory; // Subscribe to the inventory disabled event
        }
        private void OnDisable()
        {
            GamesEventManager.Instance.inputEvents.OnInventoryTogglePressed -= InventoryTogglePressed;
            GamesEventManager.Instance.inventoryUIEvents.OnInventoryEnabled -= EnableInventory; // Unsubscribe from the inventory enabled event
            GamesEventManager.Instance.inventoryUIEvents.OnInventoryDisabled -= DisableInventory; // Unsubscribe from the inventory disabled event
        }
        private void OnDestroy()
        {
            GamesEventManager.Instance.inventoryModelEvents.OnInventoryUpdated -= GetUpdateInventoryUI;
        }

        // Inventory Preparations
        private void PrepareInventoryData()
        {
            inventoryData.Initialize(); // Initialize the inventory data
            GamesEventManager.Instance.inventoryModelEvents.OnInventoryUpdated += GetUpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (!item.IsEmpty)
                {
                    inventoryData.AddItem(item.item, item.quantity);
                }
            }
        }
        private void PrepareUI()
        {
            inventoryPage.InitializeInventoryUI(inventoryData.Size); // Initialize the inventory UI with the specified size 

            // Subscribe to the item events
            GamesEventManager.Instance.inventoryUIEvents.OnDescriptionRequested += HandleDescriptionRequested; // Subscribe to the description request event
            GamesEventManager.Instance.inventoryUIEvents.OnItemActionRequested += HandleItemActionRequested; // Subscribe to the item action request event
            GamesEventManager.Instance.inventoryUIEvents.OnStartDragging += HandleItemDragStart; // Subscribe to the item drag start event
            GamesEventManager.Instance.inventoryUIEvents.OnItemSwapped += HandleItemSwap; // Subscribe to the item swap event
        }
        
        // InventoryModel Updates
        private void GetUpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryPage.ResetAllItems();
            foreach(var item in inventoryState)
            {
                inventoryPage.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
            }
        }

        // InventoryUI Updates
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

        //InventoryItem Actions
        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex); // Get the item at the specified index
            if (inventoryItem.IsEmpty) { return; }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem; // Cast the item to IDestroyableItem
            if(destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction; // Cast the item to IItemAction
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject); // Perform the item action
                audioSource.PlayOneShot(itemAction.ActionSFX); // Play the item action sound effect

                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryPage.ResetSelection(); // Reset the item selection if the item is empty
                }
            }
        }
        private void HandleItemActionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex); // Get the item at the specified index
            if (inventoryItem.IsEmpty) { return; }

            IItemAction itemAction = inventoryItem.item as IItemAction; // Cast the item to IItemAction
            if (itemAction != null)
            {
                inventoryPage.ShowItemAction(itemIndex); // Show the item action UI
                inventoryPage.AddAction(itemAction.ActionName, () => PerformAction(itemIndex)); // Add the item action to the UI
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem; // Cast the item to IDestroyableItem
            if (destroyableItem != null)
            {
                inventoryPage.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryPage.ResetSelection();
            }
        }
        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity); // Remove the item from the inventory
            inventoryPage.ResetSelection(); // Reset the item selection
            audioSource.PlayOneShot(dropClip); // Play the drop sound effect
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

        // Inventory UI Toggle
        private void InventoryTogglePressed()
        {
            if (ActiveUIManager.Instance.CanOpenUI(ActiveUIType.Inventory) && canOpenInventory)
            {
                // If the inventory page is not active and dialogue is not active, show the inventory page
                inventoryPage.Show();
                ActiveUIManager.Instance.OpenUI(ActiveUIType.Inventory); // Set the active UI type to inventory

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
            else if (ActiveUIManager.Instance.ActiveUIType.Equals(ActiveUIType.Inventory))
            {
                // If the inventory page is active and dialogue is not active, hide the inventory page
                inventoryPage.Hide();
                ActiveUIManager.Instance.CloseUI(ActiveUIType.Inventory); // Set the active UI type to none
            }

            Debug.Log(ActiveUIManager.Instance.ActiveUIType);
        }

        // Inventory Boolean Toggle
        private void EnableInventory()
        {
            canOpenInventory = true; // Enable the inventory
        }
        private void DisableInventory()
        {
            canOpenInventory = false; // Disable the inventory
        }
    }
}