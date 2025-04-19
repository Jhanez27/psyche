using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestLogItem : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI logItemName;

    public void Initialize(string id)
    {
        logItemName = this.GetComponent<TextMeshProUGUI>();
        logItemName.text = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {

        }
    }
}
