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
    private TMP_Text objectiveName;

    private int currentQuestID;

    internal void ResetDescription()
    {
        this.questName.text = "No Selected Quest";
        this.questDescription.text = string.Empty;
        this.objectiveName.text = "Objective: None";

        this.currentQuestID = -1;
    }
}
