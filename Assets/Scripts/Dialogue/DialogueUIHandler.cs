using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Ink.Runtime;
using System.Reflection;

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
    [SerializeField] private float typingSpeed = 1.0f; //Speed of typing effect
    [SerializeField] private float typingDelay = 0.05f; //Delay before typing starts

    [Header("Choice Elements")]
    [SerializeField] private GameObject choiceButtonPrefab; //Panel for displaying choices
    [SerializeField] private Transform choiceButtonContainer; //Container for the choice buttons

    private Coroutine typingCoroutine; //Coroutine for typing effect
    public bool IsTyping {get; private set;} //Flag to check if typing is in progress
    private string currentText; //Current text being displayed

    private void Awake()
    {
        dialoguePanel.SetActive(false); //Defaultly hides the dialogue panel
        IsTyping = false; //Sets typing state to false
        typingCoroutine = null; //Initializes the typing coroutine to null
        audioSource = GetComponent<AudioSource>(); //Gets the audio source component
    }

    private void OnEnable()
    {
        //Subscribes to the DialogueManager events
        DialogueManager.Instance.OnDialogueStart += ShowDialogue; 
        DialogueManager.Instance.OnDialogueUpdate += UpdateDialogue;
        DialogueManager.Instance.OnDialogueChoicesUpdate += ShowChoices; //Subscribes to the choices update event
        DialogueManager.Instance.OnDialogueEnd += HideDialogue;
        DialogueManager.Instance.OnDialogueSpeakerUpdate += UpdateSpeaker; //Subscribes to the speaker update event
        DialogueManager.Instance.OnDialoguePortraitUpdate += UpdatePortrait; //Subscribes to the portrait update event
        DialogueManager.Instance.OnDialogueLayoutUpdate += UpdateLayout; //Subscribes to the layout update event
    }

    private void OnDisable()
    {
        //Unsubscribes from the DialogueManager events
        DialogueManager.Instance.OnDialogueStart -= ShowDialogue;
        DialogueManager.Instance.OnDialogueUpdate -= UpdateDialogue;
        DialogueManager.Instance.OnDialogueChoicesUpdate -= ShowChoices; //Unsubscribes from the choices update event
        DialogueManager.Instance.OnDialogueEnd -= HideDialogue;
        DialogueManager.Instance.OnDialogueSpeakerUpdate -= UpdateSpeaker; //Unsubscribes from the speaker update event
        DialogueManager.Instance.OnDialoguePortraitUpdate -= UpdatePortrait; //Unsubscribes from the portrait update event
        DialogueManager.Instance.OnDialogueLayoutUpdate -= UpdateLayout; //Unsubscribes from the layout update event
    }

    private void ShowDialogue()
    {
        dialoguePanel.SetActive(true); //Shows the dialogue panel when the dialogue starts
    }

    private void UpdateDialogue(string newText)
    {
        dialogueText.text = ""; //Updates the dialogue text with the new text
        dialogueText.color = textColor; //Sets the text color
        dialogueText.font = textFont; //Sets the text font

        currentText = newText; //Updates the current text

        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); //Stops the previous typing coroutine if it exists
        }

        typingCoroutine = StartCoroutine(TypeText(newText)); //Starts the new typing coroutine
    }

    private void ShowChoices(Story s)
    {
        List<Choice> choices = s.currentChoices; //Gets the current choices from the story

        if (choices.Count == 0) //If there are no choices, return
        {
            return;
        }
        
        Debug.Log($"Number of choices: {choices.Count}");
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log($"Choice {i}: {choices[i].text}");
        }

        for (int i = 0; i < choices.Count; i++)
        {
            int index = i; //Stores the current index
            Choice choice = choices[index]; //Gets the current choice
            GameObject choiceGameObject = Instantiate(choiceButtonPrefab, choiceButtonContainer); //Instantiates a new choice button
            
            TextMeshProUGUI choiceText = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>(); //Gets the text component of the choice button
            choiceText.text = choice.text; //Sets the text of the choice button

            // Modify the button click listener in ShowChoices():
            choiceGameObject.GetComponent<Button>().onClick.AddListener(() => 
            {
                // Clear ALL choices first
                foreach (Transform child in choiceButtonContainer)
                {
                    Destroy(child.gameObject);
                }
                
                // Notify DialogueManager to make the choice
                DialogueManager.Instance.ChooseChoiceIndex(index);
                
                // No need to call ContinueStory() here - it's already handled in ChooseChoiceIndex
            });
            
        }
    }

    private void HideDialogue()
    {
        dialoguePanel.SetActive(false); //Hides the dialogue panel when the dialogue ends
        dialogueText.text = ""; //Clears the dialogue text
    }

    private IEnumerator TypeText(string text)
    {
        IsTyping = true; //Sets the typing state to true
        dialogueText.text = ""; //Clears the dialogue text before typing 

        yield return new WaitForSeconds(typingDelay); //Waits for the specified delay before starting to type

        int ctr = 0;
        foreach (char letter in text)
        {
            if(Input.GetKeyDown(KeyCode.C)) //Checks if the space key is pressed
            {
                SkipTyping(); //Skips the typing effect
                yield break; //Exits the coroutine
            }

            dialogueText.text += letter; //Adds each letter to the dialogue text
            ctr++; //Increments the counter for the number of letters typed

            if(typingSound != null && audioSource != null && ctr % 4 == 0) //Checks if the typing sound and audio source are set
            {
                audioSource.PlayOneShot(typingSound); //Plays the typing sound effect
            }

            yield return new WaitForSeconds(typingSpeed); //Waits for the specified typing speed before adding the next letter
        }

        IsTyping = false; //Sets the typing state to false after typing is complete
        typingCoroutine = null; //Resets the typing coroutine to null
    }

    public void SkipTyping()
    {
        if (IsTyping) //If typing is in progress
        {
            StopCoroutine(typingCoroutine); //Stops the typing coroutine
            dialogueText.text = currentText; //Clears the dialogue text
            IsTyping = false; //Sets the typing state to false
            typingCoroutine = null; //Resets the typing coroutine to null
        }
    }

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
