using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public void MovePressed(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            GamesEventManager.Instance.inputEvents.MovePressed(movement);
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GamesEventManager.Instance.inputEvents.InteractPressed();
        }
    }

    public void NextPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GamesEventManager.Instance.inputEvents.NextPressed();
        }
    }

    public void QuestLogTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GamesEventManager.Instance.inputEvents.QuestLogTogglePressed();
        }
    }
}
