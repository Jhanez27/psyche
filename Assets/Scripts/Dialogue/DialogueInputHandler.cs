using UnityEngine;

public class DialogueInputHandler : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
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
            else if (inputSystem.Player.Interact.triggered && DialogueManager.Instance.IsTyping)
            {
                //If the player is in dialogue mode and has pressed the interact button while typing, skip the typing
                DialogueManager.Instance.SkipTyping();
            }
        }
    }

    private void OnDestroy()
    {
        //Disable the input system when the object is destroyed
        inputSystem?.Disable();        
    }
}
