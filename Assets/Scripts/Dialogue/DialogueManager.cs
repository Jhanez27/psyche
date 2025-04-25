using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    //Singleton Pattern 
    public static DialogueManager Instance {get; private set; } 
    
    //Event Driven Architecture for Dialogue UI
    public event Action<string> OnDialogueSpeakerUpdate;
    public event Action<string> OnDialogueEmotionUpdate;
    public event Action<string> OnDialoguePortraitUpdate;
    public event Action<string> OnDialogueLayoutUpdate;

    public bool ShowVisualCue { get; private set; } //Property to check if the visual cue is active
    public bool IsTyping { get; set; } //Property to check if the dialogue is being typed out
    public bool TimelineIsActive { get; set; } //Property to check if the timeline is active

    //Ink JSON Tags
    private const string SPEAKER_TAG = "Speaker";
    private const string EMOTION_TAG = "Emotion";
    private const string LAYOUT_TAG = "Portrait";

    [Header("Story Configuration")]
    [SerializeField] private TextAsset inkJSON; //Ink JSON file to be used for the dialogue

    private Story story;
    private int currentChoiceIndex = -1; //Current choice index for the story
    private DialogueSource dialogueSource = DialogueSource.GAMEPLAY; //Source of the dialogue (Gameplay or Timeline)

    public bool DialogueIsActive { get; private set; } = false; //Property to check if the dialogue is active
    public bool CanInteract { get; private set; } = true; //Property to check if the player can interact with the dialogue
    public bool ChoicesDisplayed { get; private set; } = false; //Property to check if the choices are displayed
    public bool IsDialogueCooldown { get; private set; } = false; //Property to check if the dialogue is on cooldown

    private InkDialogueVariables inkDialogueVariables; //Ink dialogue variables to be used for the dialogue
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else
        {
            //If there is already an instance of DialogueManager, destroy this one which is a duplicate
            Debug.LogWarning("DialogueManager instance already exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        story = new Story(inkJSON.text); //Initialize the story with the ink JSON text
        inkDialogueVariables = new InkDialogueVariables(story); //Initialize the ink dialogue variables
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
    }

    // Input Functions
    private void NextDialoguePressed() // Function to handle the next button press
    {
        if (DialogueIsActive && CanInteract) // Only do somethjing when the Dialogue is Active
        {
            if(IsTyping) //If the dialogue is not being typed out
            {
                Debug.Log("Skipping Typing"); //Log the typing skip
                SkipTyping(); //Skip the typing
            }
            else if (!ChoicesDisplayed)
            {
                Debug.Log("Continuing Dialogue"); //Log the dialogue continuation
                ContinueOrExitStory(); //Continue or exit the story
            }
        }
    }

    // Functions for Dialogue Progression
    private void EnterDialogue(string knotName, DialogueSource source)
    {
        if(DialogueIsActive || IsDialogueCooldown) return; //If dialogue is already active, do nothing

        DialogueIsActive = true; //Set the dialogue state to inactive
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

        DialogueIsActive = false; //Set the dialogue state to inactive
        
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


        story.ResetState(); //Reset the story state
        StartCoroutine(DialogueCooldown()); //Start the dialogue cooldown coroutine
    }
    private IEnumerator DialogueCooldown()
    {
        IsDialogueCooldown = true; //Set the dialogue cooldown state to true
        yield return new WaitForSeconds(0.2f); //Wait for 0.5 seconds
        IsDialogueCooldown = false; //Set the dialogue cooldown state to false
    }
    public void SkipTyping()
    {
        if (IsTyping)
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
        Debug.Log("Sent IsTyping Value: " + isTyping); //Log the typing status
        this.IsTyping = isTyping; //Set the typing status
    }
    private void EnableInteraction()
    {
        CanInteract = true; //Set the interaction status
    }
    private void DisableInteraction()
    {
        CanInteract = false; //Set the interaction status
    }
    private void ToggleChoiceStatus(bool choiceStatus)
    {
        ChoicesDisplayed = choiceStatus; //Set the choice status
    }









    public void StartDialogue(TextAsset inkJSON)
    {
    }

    public void EndDialogue()
    {
    }

    public void ContinueStory()
    {
    }



    private void HandleTags(List<string> storyTags)
    {
        foreach (string tag in storyTags)
        {
            string[] tagParts = tag.Split(':'); //Split the tag into parts
            if(tagParts.Length == 2)
            {
                string tagName = tagParts[0].Trim(); //Get the tag name
                string tagValue = tagParts[1].Trim(); //Get the tag value

                switch (tagName)
                {
                    case SPEAKER_TAG:
                        GamesEventManager.Instance.dialogueEvents.UpdateDialogueSpeaker(tagValue); //Update the dialogue speaker with the tag value
                        GamesEventManager.Instance.dialogueEvents.UpdateDialoguePortrait(tagValue); //Update the dialogue emotion with the tag value
                        break;
                    case EMOTION_TAG:
                        //Invoke the emotion update event with the tag value
                        break;
                    case LAYOUT_TAG:
                        GamesEventManager.Instance.dialogueEvents.UpdateDialoguePortrait(tagValue); //Update the dialogue portrait with the tag value
                        break;
                    default:
                        Debug.LogWarning("Unknown tag: " + tagName); //Log a warning for unknown tags
                        break;
                }
            }
        }
    }
}
