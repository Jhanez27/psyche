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

    void Update()
    {
        //Check if the player is in dialogue mode and if the player has pressed the interact button
        if(DialogueManager.Instance.DialogueIsActive)
        {
            if (inputSystem.Player.Interact.triggered && !DialogueManager.Instance.IsTyping)
            {
                //If the player is in dialogue mode and has pressed the interact button, continue the story
                DialogueManager.Instance.ContinueStory();
            }
            else if (inputSystem.Player.Next.triggered && DialogueManager.Instance.IsTyping)
            {
                //If the player is in dialogue mode and has pressed the interact button while typing, skip the typing
                DialogueManager.Instance.SkipTyping();
            }
        }
    }
}
