using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUIHandler : MonoBehaviour
{
    [Header("Dialogue UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 1.0f; //Speed of typing effect
    [SerializeField] private float typingDelay = 0.05f; 

    private Coroutine typingCoroutine; //Coroutine for typing effect

    private void Awake()
    {
        dialoguePanel.SetActive(false); //Defaultly hides the dialogue panel
    }

    private void OnEnable()
    {
        //Subscribes to the DialogueManager events
        DialogueManager.Instance.OnDialogueStart += ShowDialogue; 
        DialogueManager.Instance.OnDialogueUpdate += UpdateDialogue;
        DialogueManager.Instance.OnDialogueEnd += HideDialogue;
    }

    private void OnDisable()
    {
        //Unsubscribes from the DialogueManager events
        DialogueManager.Instance.OnDialogueStart -= ShowDialogue;
        DialogueManager.Instance.OnDialogueUpdate -= UpdateDialogue;
        DialogueManager.Instance.OnDialogueEnd -= HideDialogue;
    }

    private void ShowDialogue()
    {
        dialoguePanel.SetActive(true); //Shows the dialogue panel when the dialogue starts
    }

    private void UpdateDialogue(string newText)
    {
        dialogueText.text = ""; //Updates the dialogue text with the new text

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
        dialogueText.text = ""; //Clears the dialogue text before typing

        yield return new WaitForSeconds(typingDelay); //Waits for the specified delay before starting to type

        foreach (char letter in text)
        {
            dialogueText.text += letter; //Adds each letter to the dialogue text
            yield return new WaitForSeconds(typingSpeed); //Waits for the specified typing speed before adding the next letter
        }

        typingCoroutine = null; //Resets the typing coroutine to null
    }
}
