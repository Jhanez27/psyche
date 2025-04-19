using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //List of all Quests
    private Dictionary<string, Quest> questMap;

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GamesEventManager.Instance.questEvents.ChangeQuestState(quest);
        }
    }
    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.questEvents.OnStartQuest += StartQuest;
        GamesEventManager.Instance.questEvents.OnAdvanceQuest += AdvanceQuest;
        GamesEventManager.Instance.questEvents.OnFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.questEvents.OnStartQuest -= StartQuest;
        GamesEventManager.Instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
        GamesEventManager.Instance.questEvents.OnFinishQuest -= FinishQuest;
    }

    public void StartQuest(string id)
    {
        Debug.Log("Quest Started " + id);

        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.questInfo.ID, QuestState.IN_PROGRESS);
    }

    public void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.questInfo.ID, QuestState.CAN_FINISH);
        }
        
    }

    public void FinishQuest(string id)
    {
        Debug.Log("Quest Finished");

        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.questInfo.ID, QuestState.FINISHED);
    }

    public void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;

        GamesEventManager.Instance.questEvents.ChangeQuestState(quest);
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
