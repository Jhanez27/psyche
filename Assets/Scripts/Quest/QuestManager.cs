using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    //List of all Quests
    private Dictionary<string, Quest> questMap;

    //Quest Events
    public event Action<string> OnStartQuest; //Event for Starting Quests
    public void StartQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);

        OnStartQuest?.Invoke(id);
    }

    public event Action<string> OnAdvanceQuest; //Event for Advancing Quests
    public void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);

        quest.MoveToNextStep();

        if (quest.CurrentQuestStepExists)
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(id, QuestState.CAN_FINISH);
        }

        OnAdvanceQuest?.Invoke(id);
    }

    public event Action<string> OnFinishQuest; //Event for Finishing Quests
    public void FinishQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(id, QuestState.FINISHED);
        OnFinishQuest?.Invoke(id);
    }

    public event Action<Quest> OnChangeQuestState;
    public void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        OnChangeQuestState?.Invoke(quest);
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            OnChangeQuestState(quest);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        questMap = CreateQuestMap();

    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.questInfo.ID, QuestState.CAN_START);
            }
        }
    }
    private void ClaimRewards(Quest quest)
    {
        Debug.Log("Rewards Claimed!");
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.questInfo.questPrerequisite)
        {
            if(GetQuestByID(prerequisiteQuestInfo.ID).state != QuestState.FINISHED)
                return false;
        }
        return true;
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        //Load all QuestInfoSO from the Resources folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quest");

        //Create the quest map
        Dictionary<string, Quest> returnQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (returnQuestMap.ContainsKey(questInfo.ID))
            { 
                Debug.LogWarning($"Quest with ID {questInfo.ID} already exists in the quest map. Skipping this quest.");
            }
            else
            {
                Quest quest = new Quest(questInfo);
                returnQuestMap.Add(questInfo.ID, quest);
            }
        }

        return returnQuestMap;
    }   
    
    private Quest GetQuestByID(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError($"Quest with ID {id} not found in the quest map.");
        }
        return quest;
    }
}
