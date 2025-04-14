using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryPage inventoryPage; // Reference to the inventory page UI

    private InputSystem_Actions inputSystem; // Reference to the input system

    public int inventorySize = 18; // Size of the inventory

    private void Awake()
    {
        inputSystem = new InputSystem_Actions(); // Initialize the input system
        inputSystem.Enable(); // Enable the input system
        inventoryPage.InitializeInventoryUI(inventorySize); // Initialize the inventory UI with the specified size 
    }

    public void Update()
    {
        if(inputSystem.Player.OpenInventory.triggered)
        {
            if(!inventoryPage.isActiveAndEnabled && !DialogueManager.Instance.DialogueIsActive)
            {
                // If the inventory page is not active and dialogue is not active, show the inventory page
                inventoryPage.Show();
            }
            else if(inventoryPage.isActiveAndEnabled && !DialogueManager.Instance.DialogueIsActive)
            {
                // If the inventory page is active and dialogue is not active, hide the inventory page
                inventoryPage.Hide();
            }
        }
    }
}
