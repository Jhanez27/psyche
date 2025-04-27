using UnityEngine;
using System;

public class PlayerEvents
{
    // Initialization of Player Events
    public event Action OnMovementEnabled; // Action for when the Player is allowed to move
    public event Action OnMovementDisabled; // Action for when the Player is not allowed to move

    // Functions for Invoking Player Events
    public void MovementEnabled()
    {
        OnMovementEnabled?.Invoke();
    }

    public void MovementDisabled()
    {
        OnMovementDisabled?.Invoke();
    }
}
