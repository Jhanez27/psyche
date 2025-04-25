using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogueInputHandler : MonoBehaviour
{
    public InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        //Enable the input system when the object is enabled
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        //Disable the input system when the object is disabled
        inputSystem.Disable();
    }
}
