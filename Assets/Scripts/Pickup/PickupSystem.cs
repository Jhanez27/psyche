using Inventory.Model;
using System;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventory; // Reference to the player's inventory

    public event Action<ItemSO, int> OnItemAdded;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickupItem item = collision.GetComponent<PickupItem>(); // Get the PickupItem component from the collided object
        if(item != null)
        {
            int quantityRemaining = inventory.AddItem(item.Item, item.Quantity); // Add the item to the inventory

            OnItemAdded?.Invoke(item.Item, item.Quantity - quantityRemaining);

            if (quantityRemaining == 0)
            {
                item.DestroyItem(); // Destroy the pickup item if added successfully
            }
            else
            {
                item.Quantity = quantityRemaining; // Update the quantity of the pickup item

            }
        }
    }
}
