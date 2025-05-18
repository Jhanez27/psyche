using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents
{
    /// Initialization of Dialogue Events
    public event Action OnDialogueStarted; // Event for when the Dialogue is Started

    public event Action<string, DialogueSource> OnDialogueEntered; // Event for when the Dialogue is Entered
    public event Action OnDialogueFinished; // Event for when the Dialogue is Finished
    public event Action<string, List<Choice>> OnDisplayDialogue; // Event for when the Dialogue is Displayed
    public event Action OnDialogueSkipped; // Event for when the Dialogue is Skipped

    public event Action<int> OnUpdateChoiceIndex; // Event for when the Choice Index is Updated

    public event Action<bool> OnTypingPerformed; // Event for Toggling the IsTyping Boolean
    public event Action OnNextEnabled;
    public event Action OnNextDisabled;
    public event Action<bool> OnChoicesToggled; // Event for when the Choices are Displayed

    public event Action<string, Ink.Runtime.Object> OnInkVariableChanged; // Event for when a Variable is Changed

    public event Action<string> OnDialogueSpeakerUpdate; // Event for when the Dialogue Speaker is Updated
    public event Action<string> OnDialoguePortraitUpdate; // Event for when the Dialogue Portrait is Updated
    public event Action<string> OnDialogueLayoutUpdate; // Event for when the Dialogue Layout is Updated

    // Functions for Invoking Events
    // Functions for Dialogue Progression
    public void EnterDialogue(string knotName, DialogueSource source)
    {
        OnDialogueEntered?.Invoke(knotName, source);
    }
    public void StartDialogue()
    {
        OnDialogueStarted?.Invoke();
    }
    public void FinishDialogue()
    {
        OnDialogueFinished?.Invoke();
    }
    public void SkipDialogue()
    {
        OnDialogueSkipped?.Invoke();
    }
    
    // Functions for Dialogue Display
    public void DisplayDialogue(string dialogueLine, List<Choice> choices)
    {
        OnDisplayDialogue?.Invoke(dialogueLine, choices);
    }

    // Functions for Choice Updates
    public void UpdateChoiceIndex(int choiceIndex)
    {
        OnUpdateChoiceIndex?.Invoke(choiceIndex);
    }

    // Functions for Boolean Toggling
    public void PerformTyping(bool isTyping)
    {
        OnTypingPerformed?.Invoke(isTyping);
    }
    public void EnableNext()
    {
        OnNextEnabled?.Invoke();
    }
    public void DisableNext()
    {
        OnNextDisabled?.Invoke();
    }
    public void ToggleChoices(bool choicesDisplayed)
    {
        OnChoicesToggled?.Invoke(choicesDisplayed);
    }


    // Functions for Variable Changes
    public void ChangeInkVariables(string variableName, Ink.Runtime.Object value)
    {
        OnInkVariableChanged?.Invoke(variableName, value);
    }

    // Functions for Dialogue UI Updates
    public void UpdateDialogueSpeaker(string speakerName)
    {
        OnDialogueSpeakerUpdate?.Invoke(speakerName);
    }
    public void UpdateDialoguePortrait(string portraitName)
    {
        OnDialoguePortraitUpdate?.Invoke(portraitName);
    }
    public void UpdateDialogueLayout(string layoutName)
    {
        OnDialogueLayoutUpdate?.Invoke(layoutName);
    }
}
