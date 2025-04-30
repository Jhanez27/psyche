using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
    }
    public void MovePressed(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {
            Debug.Log("HO1");
            Vector2 movement = context.ReadValue<Vector2>();
            GamesEventManager.Instance.inputEvents.MovePressed(movement);
        }
    }

    public void DashPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GamesEventManager.Instance.inputEvents.DashPressed();
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("HI");
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

    public void InventoryTogglePressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GamesEventManager.Instance.inputEvents.InventoryTogglePressed();
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
