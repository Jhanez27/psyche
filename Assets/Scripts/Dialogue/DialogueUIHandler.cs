using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Ink.Runtime;
using System.Reflection;
using UnityEngine.Playables;

public class DialogueUIHandler : MonoBehaviour
{
    [Header("Dialogue UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText; //Text for the name of the character speaking
    [SerializeField] private Animator portraitAnimator; //Animator for the character portrait
    [SerializeField] private Animator layoutAnimator; //Animator for the layer of the character portrait

    [Header("Sound Elements")]
    private AudioSource audioSource; //Audio source for playing sound effects
    [SerializeField] private AudioClip typingSound; //Sound effect for typing
    
    [Header("Panel Elements")]
    [SerializeField] private Color textColor; //Color of the text
    [SerializeField] private TMP_FontAsset textFont; //Font of the text
    [SerializeField] private float typingSpeed = 30.0f; //Speed of typing effect
    [SerializeField] private float typingDelay = 0.05f; //Delay before typing starts

    [Header("Choice Elements")]
    [SerializeField] private GameObject choiceButtonPrefab; //Panel for displaying choices
    [SerializeField] private Transform choiceButtonContainer; //Container for the choice buttons

    private Coroutine typingCoroutine; //Coroutine for typing effect
    private string currentText; //Current text being displayed

    private void Awake()
    {
        dialoguePanel.SetActive(false); //Defaultly hides the dialogue panel
        typingCoroutine = null; //Initializes the typing coroutine to null
        audioSource = GetComponent<AudioSource>(); //Gets the audio source component

        ResetPanelContent(); //Resets the panel content
    }
    private void OnEnable()
    {
        // Subscribe Events to GamesEventManager
        GamesEventManager.Instance.dialogueEvents.OnDialogueStarted += ShowDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDisplayDialogue += DisplayDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDialogueSkipped += SkipTyping; //Subscribes to the skip typing event
        GamesEventManager.Instance.dialogueEvents.OnDialogueFinished += HideDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDialogueSpeakerUpdate += UpdateSpeaker; //Subscribes to the speaker update event
        GamesEventManager.Instance.dialogueEvents.OnDialoguePortraitUpdate += UpdatePortrait; //Subscribes to the portrait update event
        GamesEventManager.Instance.dialogueEvents.OnDialogueLayoutUpdate += UpdateLayout; //Subscribes to the layout update event
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueStarted -= ShowDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDisplayDialogue -= DisplayDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDialogueSkipped -= SkipTyping; //Unsubscribes from the skip typing event
        GamesEventManager.Instance.dialogueEvents.OnDialogueFinished -= HideDialogue;
        GamesEventManager.Instance.dialogueEvents.OnDialogueSpeakerUpdate -= UpdateSpeaker; //Unsubscribes from the speaker update event
        GamesEventManager.Instance.dialogueEvents.OnDialoguePortraitUpdate -= UpdatePortrait; //Unsubscribes from the portrait update event
        GamesEventManager.Instance.dialogueEvents.OnDialogueLayoutUpdate -= UpdateLayout; //Unsubscribes from the layout update event
    }
    
    //For Displaying the DialoguePanel
    private void ShowDialogue() //Shows the dialogue panel when the dialogue starts
    {
        dialoguePanel.SetActive(true); // Sets the dialogue panel to active
    }
    private void HideDialogue()//Hides the dialogue panel when the dialogue ends
    {
        dialoguePanel.SetActive(false); //Sets the dialogue panel to inactive
        ResetPanelContent(); //Resets the panel content
    }
    private void ResetPanelContent() // Resets the panel content when the dialogue ends
    {
        dialogueText.text = string.Empty; // Clears the dialogue text
    }

    // Functions for Updating the Panel Content
    private void DisplayDialogue(string dialogueLine, List<Choice> choices) // Displays the dialogue line and choices
    {
        dialogueText.text = ""; //Updates the dialogue text with the new text
        dialogueText.color = textColor; //Sets the text color
        dialogueText.font = textFont; //Sets the text font

        GamesEventManager.Instance.dialogueEvents.ToggleChoices(DisplayChoices(choices)); //Toggles the choices displayed state

        currentText = dialogueLine; //Updates the current text

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); //Stops the previous typing coroutine if it exists
            typingCoroutine = null;
        }

        typingCoroutine = StartCoroutine(TypeText(dialogueLine)); //Starts the typing coroutine with the new text
    }
    private IEnumerator TypeText(string text)
    {
        GamesEventManager.Instance.dialogueEvents.PerformTyping(true); //Sets the typing state to true
        ResetPanelContent(); //Resets the panel content
        yield return new WaitForSeconds(typingDelay); //Waits for the specified delay before starting to type


        if (typingSound != null && audioSource != null) //Checks if the typing sound and audio source are set
        {
            audioSource.clip = typingSound;
            audioSource.Play();//Plays the typing sound effect
        }

        int ctr = 0;
        foreach (char letter in text)
        {
            dialogueText.text += letter; //Adds each letter to the dialogue text
            ctr++; //Increments the counter for the number of letters typed

            yield return new WaitForSeconds(1 / typingSpeed); //Waits for the specified typing speed before adding the next letter
        }

        GamesEventManager.Instance.dialogueEvents.PerformTyping(false); //Sets the typing state to false after typing is complete

        audioSource.Pause();
        typingCoroutine = null; //Resets the typing coroutine to null
    }
    public void SkipTyping()
    {
        if (typingCoroutine != null) //Checks if the typing coroutine is running
        {
            StopCoroutine(typingCoroutine); //Stops the typing coroutine
            audioSource.Pause();
            typingCoroutine = null;
        }

        dialogueText.text = currentText; //Clears the dialogue text
        GamesEventManager.Instance.dialogueEvents.PerformTyping(false); //Sets the typing state to false
    }

    //Functions for Updating the Choices
    private bool DisplayChoices(List<Choice> choices) // Displays the choices in the choice button container
    {
        if(choices.Count == 0)
        {
            return false; //Returns if there are no choices to display
        }

        foreach (Choice choice in choices) //Creates new choice buttons for each choice
        {
            GameObject choiceGameObject = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            TextMeshProUGUI choiceText = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>();
            choiceGameObject.GetComponent<DialogueChoiceButton>().SetChoiceText(choice.text); //Sets the choice text for the button
            choiceGameObject.GetComponent<DialogueChoiceButton>().SetChoiceIndex(choices.IndexOf(choice)); //Sets the choice text for the button

            Button choiceButton = choiceGameObject.GetComponent<Button>();
            choiceButton.onClick.AddListener(() =>
            {
                foreach (Transform child in choiceButtonContainer) //Destroys all previous choice buttons
                {
                    Destroy(child.gameObject);
                }
            }); 
        }
        
        return true;
    }

    //Functions for Updating UI with Ink Tags
    private void UpdateSpeaker(string speakerName)
    {
        nameText.text = speakerName; //Updates the name text with the speaker's name
    }
    private void UpdatePortrait(string animClipName)
    {
        //Updates the character portrait based on the speaker's name
        if (portraitAnimator != null)
        {
            portraitAnimator.Play(animClipName); //Sets the trigger for the animator to show the correct portrait
        }
    }
    private void UpdateLayout(string animClipName)
    {
        //Updates the layout based on the speaker's name
        if (layoutAnimator != null)
        {
            layoutAnimator.Play(animClipName); //Sets the trigger for the animator to show the correct layout
        }
    }
}
