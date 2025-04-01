using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueUIHandler : MonoBehaviour
{
    [Header("Dialogue UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText; //Text for the name of the character speaking
    [SerializeField] private Animator portraitAnimator; //Animator for the character portrait
    [SerializeField] private Animator layoutAnimator; //Animator for the layer of the character portrait
    
    [Header("Panel Elements")]
    [SerializeField] private Color textColor; //Color of the text
    [SerializeField] private TMP_FontAsset textFont; //Font of the text
    [SerializeField] private float typingSpeed = 1.0f; //Speed of typing effect
    [SerializeField] private float typingDelay = 0.05f; //Delay before typing starts

    private Coroutine typingCoroutine; //Coroutine for typing effect
    public bool IsTyping {get; private set;} //Flag to check if typing is in progress
    private string currentText; //Current text being displayed

    private void Awake()
    {
        dialoguePanel.SetActive(false); //Defaultly hides the dialogue panel
        IsTyping = false; //Sets typing state to false
        typingCoroutine = null; //Initializes the typing coroutine to null
    }

    private void OnEnable()
    {
        //Subscribes to the DialogueManager events
        DialogueManager.Instance.OnDialogueStart += ShowDialogue; 
        DialogueManager.Instance.OnDialogueUpdate += UpdateDialogue;
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

        foreach (char letter in text)
        {
            if(Input.GetKeyDown(KeyCode.X)) //Checks if the space key is pressed
            {
                SkipTyping(); //Skips the typing effect
                yield break; //Exits the coroutine
            }

            dialogueText.text += letter; //Adds each letter to the dialogue text
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
