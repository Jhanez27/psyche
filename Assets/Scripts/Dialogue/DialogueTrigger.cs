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

    private bool playerInRange;
    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        //Set the attributes to default values
        this.playerInRange = false;
        this.visualCue.SetActive(false);
        this.inputSystem = new InputSystem_Actions();
        this.inputSystem.Enable();
    }

    private void Update()
    {
        if(this.playerInRange && !DialogueManager.GetInstance().dialogueIsActive){
            //Shows the visual cue when the player is in range
            this.visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X) || this.inputSystem.Player.Interact.triggered){
                DialogueManager.GetInstance().EnterDialogueMode(this.inkJSON);
            }
        }
        else
        {
            //Hides the visual cue when the player is not in range
            this.visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in range of dialogue trigger");
        //Checks if the player is in range
        if(collision.gameObject.CompareTag("Player"))
        {
            this.playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Checks if the player is not in range
        if(collision.gameObject.CompareTag("Player"))
        {
            this.playerInRange = false;
        }
    }

    private void OnDestroy()
    {
        if(this.inputSystem != null)
        {
            this.inputSystem.Disable();
        }
    }
}
