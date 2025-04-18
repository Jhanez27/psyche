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

    //Event Driven Architecture for Dialogue System
    public event Action OnDialogueStart;
    public event Action<string> OnDialogueUpdate;
    public event Action<Story> OnDialogueChoicesUpdate;
    public event Action OnDialogueLineSkip;
    public event Action OnDialogueEnd;
    
    //Event Driven Architecture for Dialogue UI
    public event Action<string> OnDialogueSpeakerUpdate;
    public event Action<string> OnDialogueEmotionUpdate;
    public event Action<string> OnDialoguePortraitUpdate;
    public event Action<string> OnDialogueLayoutUpdate;

    public bool DialogueIsActive { get; private set; } //Property to check if the dialogue is active
    public bool ShowVisualCue { get; private set; } //Property to check if the visual cue is active
    public bool IsTyping { get; set; } //Property to check if the dialogue is being typed out
    public bool TimelineIsActive { get; set; } //Property to check if the timeline is active
    private Story currentStory; //Current story object 

    //Ink JSON Tags
    private const string SPEAKER_TAG = "Speaker";
    private const string EMOTION_TAG = "Emotion";
    private const string PORTRAIT_TAG = "Portrait";

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
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if(DialogueIsActive) return; //If dialogue is already active, do nothing

        Debug.Log("Starting dialogue with JSON: " + inkJSON.name); //Log the name of the JSON file
        
        currentStory = new Story(inkJSON.text);
        DialogueIsActive = true;
        OnDialogueStart?.Invoke(); 
        ContinueStory();
    }

    public void EndDialogue()
    {
        if(!DialogueIsActive) return; //If dialogue is not active, do nothing

        StartCoroutine(DelayDialogueEnd()); //Start the coroutine to delay the end of the dialogue
    }

    public void ContinueStory()
    {
        if(currentStory != null && currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            //Debug.Log($"Continuing story: {nextLine}");

            OnDialogueUpdate?.Invoke(nextLine);
            OnDialogueChoicesUpdate?.Invoke(currentStory);

            HandleTags(currentStory.currentTags);
        }
        else if(currentStory.currentChoices.Count > 0)
        {
            Debug.Log("Waiting for player choice...");
        }
        else
        {
            Debug.Log("End of dialogue reached.");
            EndDialogue();
        }
    }

    public void SkipTyping()
    {
        if (IsTyping)
        {
            Debug.Log("Skipping typing...");
            OnDialogueLineSkip?.Invoke(); //Invoke the line skip event
            IsTyping = false; //Set typing state to false
        }
    }

    public IEnumerator DelayDialogueEnd()
    {
        yield return new WaitForSeconds(0.2f); //Wait for 1 second before ending the dialogue
        
        DialogueIsActive = false; //Set the dialogue state to inactive
        OnDialogueEnd?.Invoke(); //Invoke the dialogue end event
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
                        OnDialogueSpeakerUpdate?.Invoke(tagValue); //Invoke the speaker update event with the tag value
                        OnDialoguePortraitUpdate?.Invoke(tagValue); //Invoke the emotion update event with the tag value
                        break;
                    case EMOTION_TAG:
                        //Invoke the emotion update event with the tag value
                        break;
                    case PORTRAIT_TAG:
                        OnDialogueLayoutUpdate?.Invoke(tagValue); //Invoke the portrait update event with the tag value
                        break;
                    default:
                        Debug.LogWarning("Unknown tag: " + tagName); //Log a warning for unknown tags
                        break;
                }
            }
        }
    }

    public void ChooseChoiceIndex(int index)
    {
        if (currentStory != null && currentStory.currentChoices != null && 
            index >= 0 && index < currentStory.currentChoices.Count)
        {
            Debug.Log($"Making choice: {index} - {currentStory.currentChoices[index].text}");
            currentStory.ChooseChoiceIndex(index);
            ContinueStory(); // This should progress the story
        }
        else
        {
            Debug.LogWarning($"Invalid choice index: {index}. Valid choices: {currentStory?.currentChoices?.Count ?? 0}");
        }
    }
}
