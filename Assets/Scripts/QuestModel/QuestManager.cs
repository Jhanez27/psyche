using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Data Persistence Config")]
    [SerializeField] private bool loadQuestState = true;

    [SerializeField]
    private QuestLogPage logPage;

    //List of all Quests
    private Dictionary<string, Quest> questMap;
    private bool canInteract = true;

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }

            GamesEventManager.Instance.questEvents.ChangeQuestState(quest);
        }
    }
    private void Awake()
    {
        questMap = CreateQuestMap();

        List<Quest> quests = questMap.Values.ToList<Quest>();
        foreach (Quest quest in quests)
        {
            Debug.Log(quest.questInfo.name + " Quest Manager");
        }
        PrepareUI(quests);
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.questEvents.OnStartQuest += StartQuest;
        GamesEventManager.Instance.questEvents.OnAdvanceQuest += AdvanceQuest;
        GamesEventManager.Instance.questEvents.OnFinishQuest += FinishQuest;
        GamesEventManager.Instance.questEvents.OnQuestStepStateChange += QuestStepChange;
        GamesEventManager.Instance.inputEvents.OnQuestLogTogglePressed += QuestTogglePressed;
        GamesEventManager.Instance.questUIEvents.OnDescriptionRequested += HandleLogDescriptionRequest;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.questEvents.OnStartQuest -= StartQuest;
        GamesEventManager.Instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
        GamesEventManager.Instance.questEvents.OnFinishQuest -= FinishQuest;
        GamesEventManager.Instance.questEvents.OnQuestStepStateChange -= QuestStepChange;
        GamesEventManager.Instance.inputEvents.OnQuestLogTogglePressed -= QuestTogglePressed;
        GamesEventManager.Instance.questUIEvents.OnDescriptionRequested -= HandleLogDescriptionRequest;
    }
    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.questInfo.ID, QuestState.CAN_START);
            }
        }
    }
    private void OnApplicationQuit() // Saves quests whenever the game is exited
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    // UI Prepapration
    private void PrepareUI(List<Quest> quests) // Iniitializes the quest log page
    {
        logPage.InitializeQuestLogPage(quests);
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

            returnQuestMap.Add(questInfo.ID, LoadQuest(questInfo));
        }

        return returnQuestMap;
    }

    // Quest Progression
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

    // Quest Input 
    private void QuestTogglePressed()
    {
        if (!logPage.isActiveAndEnabled && ActiveUIManager.Instance.CanOpenUI(ActiveUIType.QuestLog) && canInteract)
        {
            logPage.Show();
            ActiveUIManager.Instance.OpenUI(ActiveUIType.QuestLog);

            bool isFirst = true;
            foreach (Quest quest in questMap.Values)
            {
                if (isFirst)
                {
                    isFirst = false;
                    GamesEventManager.Instance.questUIEvents.DescriptionRequested(quest.questInfo.ID);
                }

                logPage.UpdateData(quest.questInfo.ID, quest.questInfo.displayName);
            }
            GamesEventManager.Instance.playerEvents.MovementDisabled();
        }
        else
        {
            logPage.Hide();
            GamesEventManager.Instance.playerEvents.MovementEnabled();
            ActiveUIManager.Instance.CloseUI(ActiveUIType.QuestLog);
        }
    }

    // Quest Display
    private void HandleLogDescriptionRequest(string id)
    {
        Quest logItem = questMap[id];
        if (logItem != null)
        {
            Quest quest = questMap[id];
            GameObject questStepObject;

            if (quest.CurrentStepExists())
            {
                questStepObject = quest.questInfo.questSteps[quest.currentQuestStepIndex];
            }
            else
            {
                questStepObject = quest.questInfo.questSteps[quest.currentQuestStepIndex - 1];
            }

            if (questStepObject != null)
            {
                QuestStep questStep = questStepObject.GetComponent<QuestStep>();

                logPage.UpdateDescription(
                    quest.questInfo.name,
                    quest.questInfo.description,
                    questStep.QuestStepName,
                    quest.GetFullStatusText()
                );
            }
        }
    }

    // Quest Attribute Updates
    private void QuestStepChange(string id, int index, QuestStepState state)
    {
        Quest quest = GetQuestByID(id);
        quest.StoreQuestStepState(state, index);
        ChangeQuestState(quest.questInfo.ID, quest.state);
    }
    public void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;

        GamesEventManager.Instance.questEvents.ChangeQuestState(quest);
    }

    //Quest Boolean Updates
    private void EnableInteract()
    {
        canInteract = true;
    }
    private void DisableInteract()
    {
        canInteract = false;
    }

    // Quest Access and Checking
    private bool CheckRequirementsMet(Quest quest) // Checks whether each prerequisite quest is satisfied
    {
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.questInfo.questPrerequisite)
        {
            if (GetQuestByID(prerequisiteQuestInfo.ID).state != QuestState.FINISHED)
                return false;
        }
        return true;
    }
    private Quest GetQuestByID(string id) // Get corresponding Quest through ID
    {

        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError($"Quest with ID {id} not found in the quest map.");
        }
        return quest;
    }

    // Quest Rewards
    private void ClaimRewards(Quest quest)
    {
        Debug.Log("Rewards Claimed!");
    }

    // Quest Data Persistence
    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData data = quest.GetQuestData();
            string serialisedData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(quest.questInfo.ID, serialisedData);

            Debug.Log(serialisedData);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save quest with id: " + quest.questInfo.ID + " : " + e);
        }
    }
    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;

        try
        {
            if (PlayerPrefs.HasKey(questInfo.ID) && loadQuestState)
            {
                string serializeData = PlayerPrefs.GetString(questInfo.ID);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializeData);
                quest = new Quest(questInfo, questData.state, questData.stepIndex, questData.stepStates);
            }
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.questInfo.ID + " : " + e);
        }

        return quest;
    }
}
