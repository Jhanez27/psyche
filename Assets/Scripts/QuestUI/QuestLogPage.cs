using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestLogPage : MonoBehaviour
{
    [SerializeField]
    private QuestLogItem logItemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    [SerializeField]
    private QuestLogDescription descriptionPrefab;

    private List<QuestLogItem> availableQuests = new List<QuestLogItem>();

    private void Awake()
    {
        Hide();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
        descriptionPrefab.ResetDescription();
    }

    public void InitializeQuestLogPage()
    {

    }
}
