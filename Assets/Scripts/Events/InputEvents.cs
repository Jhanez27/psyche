using System;
using UnityEngine;

public class InputEvents
{
    // Identifier for when Interaction occurs, whether through Dialogue, Default, and many more
    public InputEventContext InputEventContext { get; private set; } = InputEventContext.DEFAULT;

    // Initialization of Input Events
    public event Action<Vector2> OnMovePressed; // Event for when the Move Buttons are Pressed
    public event Action OnDashPressed; // Event for when the Dash Button is Pressed
    public event Action<InputEventContext> OnInteractPressed; // Event for when the Interact Button is Pressed
    public event Action OnNextPressed; // Event for when the Next Button(s) are Pressed
    public event Action OnInventoryTogglePressed; // Event for when the InventoryToggle Button is Pressed
    public event Action OnQuestLogTogglePressed; // Event for when the QuestLogToggle Button is Pressed

    // Functions for Invoking Input Events
    public void MovePressed(Vector2 movement)
    {
        OnMovePressed?.Invoke(movement);
    }
    public void DashPressed()
    {
        OnDashPressed?.Invoke();
    }
    public void InteractPressed()
    {
        OnInteractPressed?.Invoke(this.InputEventContext);
    }
    public void NextPressed()
    {
        Debug.Log("Next Pressed!");
        OnNextPressed?.Invoke();
    }
    public void InventoryTogglePressed()
    {
        OnInventoryTogglePressed?.Invoke();
    }
    public void QuestLogTogglePressed()
    {
        OnQuestLogTogglePressed?.Invoke();
    }

    // Function for Changing Input Context
    public void ChangeInputEventContext(InputEventContext inputEventContext)
    {
        this.InputEventContext = inputEventContext;
    }
}
