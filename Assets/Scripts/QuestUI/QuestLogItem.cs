using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestLogItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text logItemName;

    public string ID { get; private set; }

    public void SetLogData(string id, string name)
    {
        logItemName.text = name;
        this.ID = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            GamesEventManager.Instance.questUIEvents.LogItemClicked(this);
        }
    }
}
