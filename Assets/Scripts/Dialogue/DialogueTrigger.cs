using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    //Visual Cue refers to the interactible notifying the player if it can be interacted with
    [SerializeField] private GameObject visualCue;
    
    
    [Header("Ink JSON")]
    //Ink JSON refers to the JSON file that contains the dialogue
    [SerializeField] private TextAsset inkJSON;

    private bool canInteract;
    private bool playerInRange;
    private bool visualCueAllowed;
    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        //Set the attributes to default values
        playerInRange = false;
        visualCue.SetActive(false);
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
        visualCueAllowed = true;
        canInteract = true;
    }

    private void Update()
    {
        if(playerInRange && !DialogueManager.Instance.DialogueIsActive && canInteract){
            //Shows the visual cue when the player is in range
            if(visualCueAllowed) { visualCue.SetActive(true);}
            if (inputSystem.Player.Interact.triggered){
                DialogueManager.Instance.StartDialogue(this.inkJSON);
            }
        }
        else
        {
            //Hides the visual cue when the player is not in range
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in range of dialogue trigger");
        //Checks if the player is in range
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Checks if the player is not in range
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnDestroy()
    {
        if(inputSystem != null)
        {
            inputSystem.Disable();
        }
    }

    public void SetVisualCueAllowed(bool allowed)
    {
        visualCueAllowed = allowed;
        if(!allowed) { visualCue.SetActive(false);}
    }

    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
        if(!canInteract) { visualCue.SetActive(false);}
    }
}
