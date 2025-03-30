using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsActive { get; private set; }
    private static DialogueManager instance;
    private InputSystem_Actions inputSystem;

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Awake()
    {
        //Singleton pattern
        if(instance == null) 
        {
            instance = this;
        }
        else 
        {
            Debug.LogWarning("Another instance of DialogueManager already exists. Destroying this instance.");
        }

        //Enable the input system
        this.inputSystem = new InputSystem_Actions();
        this.inputSystem.Enable();
    }

    private void Start()
    {
        //Set default values
        dialoguePanel.SetActive(false);
        dialogueIsActive = false;
    }

    private void Update()
    {
        //Check if the player is in dialogue mode and if the player has pressed the interact button
        if(dialogueIsActive && (inputSystem.Player.Interact.triggered || Input.GetKeyDown(KeyCode.X))){
            this.ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //Create a new story object with the inkJSON text
        this.currentStory = new Story(inkJSON.text);
        this.dialogueIsActive = true;
        this.dialoguePanel.SetActive(true);

        this.ContinueStory();
    }

    private void ExitDialogueMode()
    {
        //Set the dialogue mode to false and hide the dialogue panel
        this.dialogueIsActive = false;
        this.dialoguePanel.SetActive(false);
        this.dialogueText.text = "";
    }

    private void ContinueStory()
    {
        //Check if the story can continue and continue the story
        if (this.currentStory.canContinue){
            dialogueText.text = this.currentStory.Continue();
        }
        else
        {
            this.ExitDialogueMode();
        }
    }

    private void Oestroy()
    {
        //Disable the input system
        if(this.inputSystem != null)
        {
            this.inputSystem.Disable();
        }
    }
}
