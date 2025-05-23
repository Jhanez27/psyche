using Inventory.UI;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    //MouseFollower Properties
    private Canvas canvas; // Where the MouseFollower will be displayed
    private InventoryUIItem item; // The item which the MouseFollower will display

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); // Get the canvas component from the root transform
        item = GetComponentInChildren<InventoryUIItem>(); // Get the InventoryItem component from the children
    }

    public void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
        ); // Convert the mouse position to local point in the canvas    
        transform.position = canvas.transform.TransformPoint(position); // Set the position of the object to the mouse position
    }

    public void Toggle(bool active)
    {
        gameObject.SetActive(active); // Set the active state of the object
    }
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity); // Set the data for the inventory item
    }
}
