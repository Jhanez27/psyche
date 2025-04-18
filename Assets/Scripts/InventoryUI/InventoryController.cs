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

        private CharactersInputHandler inputHandler;

        [SerializeField]
        private AudioClip dropClip;
        [SerializeField]
        private AudioSource audioSource;

        private void Awake()
        {
            inputHandler = GetComponent<CharactersInputHandler>(); // Get the input handler component

            PrepareUI();
            PrepareInventoryData();
        }

        private void OnEnable()
        {
            inputHandler.PlayerControls.Enable(); // Enable the input system
        }

        private void OnDisable()
        {
            inputHandler.PlayerControls.Disable(); // Disable the input system
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

        public void Update() //Checks if OpenInventory key is triggered
        {
            if (inputHandler.PlayerControls.Player.OpenInventory.triggered)
            {
                Debug.Log(!DialogueManager.Instance.DialogueIsActive + " " + !inventoryPage.isActiveAndEnabled);
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