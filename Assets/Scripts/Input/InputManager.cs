using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public void MovePressed(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {
            Debug.Log("Move Pressed");
            Vector2 movement = context.ReadValue<Vector2>();
            GamesEventManager.Instance.inputEvents.MovePressed(movement);
        }
    }

    public void DashPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Dash Pressed");
            GamesEventManager.Instance.inputEvents.DashPressed();
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Interact Pressed");
            GamesEventManager.Instance.inputEvents.InteractPressed();
        }
    }

    public void NextPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Next Pressed");
            GamesEventManager.Instance.inputEvents.NextPressed();
        }
    }

    public void InventoryTogglePressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Inventory Toggle Pressed");
            GamesEventManager.Instance.inputEvents.InventoryTogglePressed();
        }
    }

    public void QuestLogTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("QuestLogToggle Pressed");
            GamesEventManager.Instance.inputEvents.QuestLogTogglePressed();
        }
    }
}
