using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //Singleton Pattern 
    public static DialogueManager Instance {get; private set; } 

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText; 

    public bool DialogueIsActive { get; private set; }
    private Story currentStory; 
    private InputSystem_Actions inputSystem;


    private void Awake()
    {
        //Singleton pattern
        if(Instance == null) { Instance = this;}
        else { 
            //If there is already an instance of DialogueManager, destroy this one which is a duplicate
            Debug.LogWarning("DialogueManager instance already exists, destroying duplicate."); 
            Destroy(this.gameObject);
            return;
        }

        //Enable the input system
        this.inputSystem = new InputSystem_Actions();
        this.inputSystem.Enable();
    }

    private void Start()
    {
        //Set default values
        dialoguePanel.SetActive(false);
        DialogueIsActive = false;
    }

    private void Update()
    {
        //Check if the player is in dialogue mode and if the player has pressed the interact button
        if(DialogueIsActive && (inputSystem.Player.Interact.triggered || Input.GetKeyDown(KeyCode.X))){
            this.ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //Create a new story object with the inkJSON text
        this.currentStory = new Story(inkJSON.text);
        this.DialogueIsActive = true;
        this.dialoguePanel.SetActive(true);

        this.ContinueStory();
    }

    private void ExitDialogueMode()
    {
        //Set the dialogue mode to false and hide the dialogue panel
        this.DialogueIsActive = false;
        this.dialoguePanel.SetActive(false);
        this.dialogueText.text = "";
    }

    private void ContinueStory()
    {
        //Check if the story can continue and continue the story
        if (this.currentStory.canContinue){
            this.dialogueText.text = this.currentStory.Continue();
        }
        else
        {
            this.ExitDialogueMode();
        }
    }

    public void StartCutsceneDialogue(TextAsset inkJSON)
    {
        if(!this.DialogueIsActive)
        {
            this.EnterDialogueMode(inkJSON);
        }
    }
    private void OnDestroy()
    {
        //Disable the input system
        if(this.inputSystem != null)
        {
            this.inputSystem.Disable();
        }
    }
}
