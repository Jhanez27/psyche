using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    //Visual Cue refers to the interactible notifying the player if it can be interacted with
    [SerializeField] private GameObject visualCue;
    
    [Header("Dialogue Configuration")]
    [SerializeField] private TextAsset inkJSON; //Ink JSON file for the dialogue
    [SerializeField] private bool isCutsceneTrigger = false; //Checks if the trigger is a cutscene trigger

    private bool canInteract = true;

    private bool playerInRange = false;
    private bool visualCueAllowed = true;
    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        HideVisualCue();

        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    public void HideVisualCue()
    {
        visualCue.SetActive(false);
    }

    public void SetVisualCueAllowed(bool allowed)
    {
        visualCueAllowed = allowed;
        if (!allowed) { visualCue.SetActive(false); }
    }

    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
        if (!canInteract) { visualCue.SetActive(false); }
    }

    public void ChangeInkJSON(TextAsset newText)
    {
        if(newText != null)
        {
            inkJSON = newText;
        }
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

    public void StartDialogueScene()
    {
        if(isCutsceneTrigger)
        {
            DialogueManager.Instance.StartDialogue(this.inkJSON);
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

}
