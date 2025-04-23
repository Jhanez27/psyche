using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using System;
using Inventory.Model;
using Inventory.UI;
using System.Linq;

public class QuestLogPage : MonoBehaviour
{
    [SerializeField]
    private QuestLogItem logItemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    [SerializeField]
    private QuestLogDescription descriptionPanel;

    private Dictionary<string, QuestLogItem> availableQuests = new Dictionary<string, QuestLogItem>();

    private void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        descriptionPanel.ResetDescription();
    }

    public void InitializeQuestLogPage(List<Quest> questList)
    {
        Debug.Log(questList.Count);
        foreach(Quest quest in questList)
        {
            LoadLogItems(quest);
        }

        if (availableQuests.Count > 0)
        {
            var firstQuest = questList.FirstOrDefault();
            if (firstQuest != null)
            {
                GamesEventManager.Instance.questUIEvents.DescriptionRequested(firstQuest.questInfo.ID);
            }
        }
    }

    private void DestroyLogItems()
    {
        foreach(QuestLogItem logItem in availableQuests.Values)
        {
            Destroy(logItem.gameObject);
        }
    }

    private void LoadLogItems(Quest quest)
    {
        QuestLogItem logItem = Instantiate(logItemPrefab, contentPanel.transform).GetComponent<QuestLogItem>();
        availableQuests[quest.questInfo.ID] = logItem;

        GamesEventManager.Instance.questUIEvents.OnLogItemClicked += HandleLogItemSelection;
    }

    public void UpdateData(string id, string name)
    {
        availableQuests[id].SetLogData(id, name);
    }



    private void HandleLogItemSelection(QuestLogItem logItem)
    {
        if(availableQuests.ContainsValue(logItem))
        {
            GamesEventManager.Instance.questUIEvents.DescriptionRequested(logItem.ID);
        }
    }

    public void UpdateDescription(string name, string description, string objective, string status)
    {
        this.descriptionPanel.UpdateDescription(name, description, objective, status);
    }
}
