using System;
using UnityEngine;

public class QuestEvents
{
    //Initialization of Quest Events
    public event Action<string> OnStartQuest; // Event for Starting a Quest
    public event Action<string> OnAdvanceQuest; // Event for Proceeding to the next Quest Step
    public event Action<string> OnFinishQuest; // Event for Finishing the Quest
    public event Action<Quest> OnChangeQuestState; // Event for changing the Quest State of a Quest
    public event Action<string, int, QuestStepState> OnQuestStepStateChange; // Event for changing the QuestStepState

    //Functions for Invoking Quest Events
    public void StartQuest(string id)
    {
        OnStartQuest?.Invoke(id);
    }

    public void AdvanceQuest(string id)
    {
        OnAdvanceQuest?.Invoke(id);
    }

    public void FinishQuest(string id)
    {
        OnFinishQuest?.Invoke(id);
    }

    public void ChangeQuestState(Quest quest)
    {
        OnChangeQuestState?.Invoke(quest);
    }

    public void ChangeQuestStepState(string id, int stepIndex, QuestStepState questStepState)
    {
        OnQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
    }
}
