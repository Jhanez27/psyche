using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler
{
    // Components of the Dialogue Button
    [Header("Components")]
    [SerializeField]
    private Button choiceButton;
    [SerializeField]
    private TextMeshProUGUI choiceButtonText;

    private int choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        choiceButtonText.text = choiceTextString;
    }

    public void SetChoiceIndex(int index)
    {
        choiceIndex = index;
    }

    public void SelectButton()
    {
        choiceButton.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GamesEventManager.Instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }
}
