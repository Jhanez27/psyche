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
    public event Action OnDialogueEnd;

    public bool DialogueIsActive { get; private set; } //Property to check if the dialogue is active
    private Story currentStory; //Current story object

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

        currentStory = new Story(inkJSON.text);
        DialogueIsActive = true;
        OnDialogueStart?.Invoke(); 
        ContinueStory();
    }

    public void EndDialogue()
    {
        if(!DialogueIsActive) return; //If dialogue is not active, do nothing

        DialogueIsActive = false; //Set dialogue to inactive
        OnDialogueEnd?.Invoke(); //Invoke the dialogue end event
    }

    public void ContinueStory()
    {
        if(currentStory != null && currentStory.canContinue)
        {
            string nextLine = currentStory.Continue(); //Continue the story and get the text
            OnDialogueUpdate?.Invoke(nextLine); //Invoke the dialogue update event with the text
        }
        else
        {
            EndDialogue(); //End the dialogue if there is no more text
        }
    }
}
