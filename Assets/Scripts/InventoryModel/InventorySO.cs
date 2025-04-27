using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 18; // Default inventory size

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>(Size); // Initthisialize the inventory items list with the specified size
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem()); // Fill the inventory with empty items
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            if(!item.IsStackable)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddNonStackableItem(item, 1);
                }
                UpdateInventory();
                return quantity;
            }
            quantity = AddStackableItem(item, quantity); // Try to add to an existing stackable item
            UpdateInventory();
            return quantity;
        }

        private int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem; // Add the new item to the first empty slot
                    return quantity; // Return 0 if the item was added successfully
                }
            }

            return 0;
        }

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false; // Check if the inventory is full

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].IsEmpty) // Filling out nonfully stacked similar items
                {
                    if (inventoryItems[i].item.ID == item.ID)
                    {
                        int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                        if(quantity > amountPossibleToTake)
                        {
                            inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize); // Fill the stack to max
                            quantity -= amountPossibleToTake; // Decrease the quantity by the amount added
                        }
                        else
                        {
                            inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity); // Add the quantity to the existing stack
                            return 0; // Return 0 if the item was added successfully
                        }
                    }
                }
            }

            while(quantity > 0 && !IsInventoryFull()) //Filling out empty slots
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize); // Clamp the quantity to the maximum stack size
                quantity -= newQuantity; // Decrease the quantity by the amount added

                AddItemToFirstFreeSlot(item, newQuantity); // Add the item to the first empty slot
            }
            return quantity; // Return the remaining quantity
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity
            };

            for(int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem; // Add the new item to the first empty slot
                    return quantity;
                }
            }
            return 0; // Return 0 if the item was added successfully
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].IsEmpty)
                {
                    returnValue.Add(i, inventoryItems[i]); // Add non-empty items to the dictionary
                }
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int index)
        {
            return inventoryItems[index]; // Get the item at the specified index
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            if (itemIndex1 < inventoryItems.Count && itemIndex1 >= 0 && itemIndex2 < inventoryItems.Count && itemIndex2 >= 0)
            {
                InventoryItem itemTemp = inventoryItems[itemIndex1];
                inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
                inventoryItems[itemIndex2] = itemTemp;
                UpdateInventory();
            }
        }

        private void UpdateInventory()
        {
            //Changes the inventory look whenever there is a change in invetory states
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        internal void RemoveItem(int itemIndex, int amount)
        {
            if (inventoryItems.Count > itemIndex)
            {
                InventoryItem item = inventoryItems[itemIndex];
                if (item.IsEmpty)
                    return;

                int quantityRemaining = item.quantity - amount;
                if (quantityRemaining <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem(); // Remove the item if quantity is 0 or less
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(quantityRemaining); // Update the quantity of the item
                }

                UpdateInventory();
            }
        }
    }

    [Serializable]
    public struct InventoryItem //uses struct because it is a value type and we want to avoid reference type issues
    {
        public ItemSO item; // Reference to the item scriptable object
        public int quantity; // Quantity of the item in the inventory
        public bool IsEmpty => item == null; // Check if the item is empty

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity > 0 ? newQuantity : 0, // Set quantity to 0 if new quantity is less than or equal to 0
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0, // Create an empty item with null item and quantity 0
        };
    }
    
}


