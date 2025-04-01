using UnityEngine;

public class DialogueInputHandler : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(inputSystem.Player.Interact.ReadValue<float>());
        
        //Check if the player is in dialogue mode and if the player has pressed the interact button
        if(DialogueManager.Instance.DialogueIsActive && inputSystem.Player.Interact.triggered)
        {
            DialogueManager.Instance.ContinueStory();
        }
    }

    private void OnDestroy()
    {
        //Disable the input system when the object is destroyed
        if (inputSystem != null)
        {
            inputSystem.Disable();
        }        
    }
}
