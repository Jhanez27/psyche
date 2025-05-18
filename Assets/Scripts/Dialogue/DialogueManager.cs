using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Story Configuration")]
    [SerializeField] private TextAsset inkJSON; //Ink JSON file to be used for the dialogue

    private Story story;
    private int currentChoiceIndex = -1; //Current choice index for the story
    private DialogueSource dialogueSource = DialogueSource.GAMEPLAY; //Source of the dialogue (Gameplay or Timeline)

    // Boolean Properties for Dialogue Manager
    private bool dialogueIsActive = false; //Property to check if the dialogue is active
    private bool canInteract = true; //Property to check if the player can interact with the dialogue
    private bool choicesDisplayed = false; //Property to check if the choices are displayed
    public bool isDialogueCooldown { get; private set; } = false; //Property to check if the dialogue is on cooldown
    private bool isTyping = false; //Property to check if the dialogue is being typed out
    private bool ShowVisualCue = true; //Property to check if the visual cue is active

    //Ink JSON Tags
    private const string SPEAKER_TAG = "Speaker";
    private const string LAYOUT_TAG = "Layout";

    // Ink Dialogue Integration
    private InkExternalFunctions inkExternalFunctions; //Ink external functions to be used for the dialogue
    private InkDialogueVariables inkDialogueVariables; //Ink dialogue variables to be used for the dialogue

    protected override void Awake()
    {
        base.Awake(); //Call the base Awake method

        story = new Story(inkJSON.text); //Initialize the story with the ink JSON text

        inkExternalFunctions = new InkExternalFunctions(); //Initialize the ink external functions
        inkExternalFunctions.Bind(story); //Bind the external functions to the story

        inkDialogueVariables = new InkDialogueVariables(story); //Initialize the ink dialogue variables
    }
    private void OnDestroy()
    {
        inkExternalFunctions.Unbind(story); //Unbind the external functions from the story
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += EnterDialogue; 
        GamesEventManager.Instance.inputEvents.OnNextPressed += NextDialoguePressed; //Subscribe to the continue event
        GamesEventManager.Instance.dialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex; //Subscribe to the choice index update event
        GamesEventManager.Instance.dialogueEvents.OnTypingPerformed += ToggleTypingStatus; //Subscribe to the typing status event
        GamesEventManager.Instance.dialogueEvents.OnNextEnabled += EnableInteraction; //Subscribe to the enable interaction event
        GamesEventManager.Instance.dialogueEvents.OnNextDisabled += DisableInteraction; //Subscribe to the disable interaction event
        GamesEventManager.Instance.dialogueEvents.OnChoicesToggled += ToggleChoiceStatus; //Subscribe to the choice status event
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged += UpdateInkVariable; //Subscribe to the ink variable changed event
        GamesEventManager.Instance.questEvents.OnChangeQuestState += QuestStateChange; //Subscribe to the quest state change event
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered -= EnterDialogue;
        GamesEventManager.Instance.inputEvents.OnNextPressed -= NextDialoguePressed; //Unsubscribe to the continue event
        GamesEventManager.Instance.dialogueEvents.OnUpdateChoiceIndex -= UpdateChoiceIndex; //Unsubscribe from the choice index update event
        GamesEventManager.Instance.dialogueEvents.OnTypingPerformed -= ToggleTypingStatus; //Unsubscribe from the typing status event
        GamesEventManager.Instance.dialogueEvents.OnNextEnabled -= EnableInteraction; //Subscribe to the enable interaction event
        GamesEventManager.Instance.dialogueEvents.OnNextDisabled -= DisableInteraction; //Subscribe to the disable interaction event
        GamesEventManager.Instance.dialogueEvents.OnChoicesToggled -= ToggleChoiceStatus; //Unsubscribe from the choice status event
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged -= UpdateInkVariable; //Unsubscribe from the ink variable changed event
        GamesEventManager.Instance.questEvents.OnChangeQuestState -= QuestStateChange; //Unsubscribe from the quest state change event
    }

    // Input Functions
    private void NextDialoguePressed() // Function to handle the next button press
    {
        if (dialogueIsActive && canInteract && ActiveUIManager.Instance.ActiveUIType.Equals(ActiveUIType.Dialogue)) // Only do somethjing when the Dialogue is Active
        {
            if(isTyping) //If the dialogue is not being typed out
            {
                Debug.Log("Skipping Typing"); //Log the typing skip
                SkipTyping(); //Skip the typing
            }
            else if (!choicesDisplayed)
            {
                Debug.Log("Continuing Dialogue"); //Log the dialogue continuation
                ContinueOrExitStory(); //Continue or exit the story
            }
        }
    }

    // Functions for Dialogue Progression
    private void EnterDialogue(string knotName, DialogueSource source)
    {
        if(dialogueIsActive || isDialogueCooldown) return; //If dialogue is already active, do nothing

        dialogueIsActive = true; //Set the dialogue state to inactive
        dialogueSource = source; //Set the dialogue source

        ActiveUIManager.Instance.OpenUI(ActiveUIType.Dialogue); //Open the dialogue UI

        GamesEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE); //Change the input event context to dialogue
        GamesEventManager.Instance.playerEvents.MovementDisabled(); //Disable player movement
        GamesEventManager.Instance.dialogueEvents.StartDialogue(); //Show the dialogue UI

        if (!knotName.Equals(string.Empty))
            {
            story.ChoosePathString(knotName); //Choose the path string for the story
        }
        else
        {
            Debug.LogWarning("Knot name is empty, using default path.");
        }

        inkDialogueVariables.SyncVariablesAndStartListening(story); //Sync the variables and start listening to the story

        Debug.Log("Active UI Type: " + ActiveUIManager.Instance.ActiveUIType); //Log the active UI type
        ContinueOrExitStory();
    }
    private void ContinueOrExitStory()
    {
        if (story.canContinue)
        {
            string dialogueLine = story.Continue(); //Continue the story and get the next line
            List<string> storyTags = story.currentTags; //Get the current tags from the story

            if(storyTags.Count > 0) //If there are tags present
            {
                HandleTags(storyTags); //Handle the tags
            }

            while (IsLineBlank(dialogueLine) && story.canContinue) //Check if the line is blank
            {
                dialogueLine = story.Continue(); //Continue the story until a non-blank line is found
            }

            if (IsLineBlank(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                GamesEventManager.Instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            }
        }

        else 
        {
            ExitDialogue(); //Exit the dialogue if there are no more lines
        }
    }
    private void ExitDialogue()
    {
        Debug.Log("Exiting Dialogue"); //Log the dialogue exit

        dialogueIsActive = false; //Set the dialogue state to inactive
        
        GamesEventManager.Instance.dialogueEvents.FinishDialogue(); //Exit the dialogue
        ActiveUIManager.Instance.CloseUI(ActiveUIType.Dialogue);
        if (dialogueSource == DialogueSource.GAMEPLAY)
        {
            GamesEventManager.Instance.playerEvents.MovementEnabled(); //Enable player movement
        }
        else
        {
            GamesEventManager.Instance.timelineEvents.StartTimeline(); //Finish the timeline

        }
        GamesEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT); //Change the input event context back to default

        inkDialogueVariables.StopListening(story); //Stop listening to the story variables

        Debug.Log("Active UI Type: " + ActiveUIManager.Instance.ActiveUIType); //Log the active UI type
        story.ResetState(); //Reset the story state
        StartCoroutine(DialogueCooldown()); //Start the dialogue cooldown coroutine
    }
    private IEnumerator DialogueCooldown()
    {
        isDialogueCooldown = true; //Set the dialogue cooldown state to true
        yield return new WaitForSeconds(0.5f); //Wait for 0.5 seconds
        isDialogueCooldown = false; //Set the dialogue cooldown state to false
    }
    public void SkipTyping()
    {
        if (isTyping)
        {
            GamesEventManager.Instance.dialogueEvents.SkipDialogue();
        }
        
    }
    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals(string.Empty) || dialogueLine.Trim().Equals("\n"); //Check if the dialogue line is blank
    }
    
    // Funtions for Choice Updates
    private void UpdateChoiceIndex(int index)
    {
        currentChoiceIndex = index; //Update the current choice index
        story.ChooseChoiceIndex(currentChoiceIndex); //Choose the choice index in the story
        ContinueOrExitStory(); //Continue or exit the story
        ToggleChoiceStatus(false);
    }

    // Function for Toggling Manager Booleans
    private void ToggleTypingStatus(bool isTyping)
    {
        this.isTyping = isTyping; //Set the typing status
    }
    private void EnableInteraction()
    {
        canInteract = true; //Set the interaction status
    }
    private void DisableInteraction()
    {
        canInteract = false; //Set the interaction status
    }
    private void ToggleChoiceStatus(bool choiceStatus)
    {
        choicesDisplayed = choiceStatus; //Set the choice status
    }

    // Function for Handling Tags
    private void HandleTags(List<string> storyTags)
    {
        foreach (string tag in storyTags)
        {
            string[] tagParts = tag.Split(':'); //Split the tag into parts
            if (tagParts.Length == 2)
            {
                string tagName = tagParts[0].Trim(); //Get the tag name
                string tagValue = tagParts[1].Trim(); //Get the tag value

                switch (tagName)
                {
                    case SPEAKER_TAG:
                        GamesEventManager.Instance.dialogueEvents.UpdateDialogueSpeaker(tagValue); //Update the dialogue speaker with the tag value
                        GamesEventManager.Instance.dialogueEvents.UpdateDialoguePortrait(tagValue); //Update the dialogue portrait with the tag value
                        break;
                    case LAYOUT_TAG:
                        GamesEventManager.Instance.dialogueEvents.UpdateDialogueLayout(tagValue); //Update the dialogue portrait with the tag value
                        break;
                    default:
                        Debug.LogWarning("Unknown tag: " + tagName); //Log a warning for unknown tags
                        break;
                }
            }
        }
    }

    // Function for Updating Ink Variables
    private void UpdateInkVariable(string name, Ink.Runtime.Object value)
    {
        inkDialogueVariables.UpdateVariableState(name, value); //Update the ink variable state
    }
    public bool GetBoolInkVariableValue(string name)
    {
        return inkDialogueVariables.GetBoolVariableState(name);
    }
    private void QuestStateChange(Quest quest)
    {
        GamesEventManager.Instance.dialogueEvents.ChangeInkVariables(
            quest.questInfo.ID + "State",
            new StringValue(quest.state.ToString())
            );
    }
}
