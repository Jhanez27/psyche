using Inventory.Model;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventory; // Reference to the player's inventory

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickupItem item = collision.GetComponent<PickupItem>(); // Get the PickupItem component from the collided object
        if(item != null)
        {
            int quantityRemaining = inventory.AddItem(item.Item, item.Quantity); // Add the item to the inventory
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
