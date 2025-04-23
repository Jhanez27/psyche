using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class QuestLogDescription : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questName;
    [SerializeField]
    private TMP_Text questDescription;
    [SerializeField]
    private TMP_Text questObjective;
    [SerializeField]
    private TMP_Text questStepStatus;

    private void Awake()
    {
        ResetDescription();
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.questEvents.OnChangeQuestState += QuestStateChange;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.questEvents.OnChangeQuestState -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest == null)
            return;
    }
    public void ResetDescription()
    {
        this.questName.text = "No Selected Quest";
        this.questDescription.text = string.Empty;
        this.questObjective.text = "Objective: None";
        this.questStepStatus.text = "Status: None";
    }

    public void UpdateDescription(string name, string description, string objective, string status)
    {
        this.questName.text = name;
        this.questDescription.text = description;
        this.questObjective.text = "Objective: " + objective;
        this.questStepStatus.text = status;
    }
}
