using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Quest
{
    public QuestInfoSO questInfo;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] stepStates;
    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
        stepStates = new QuestStepState[this.questInfo.questSteps.Length];
        for (int i = 0; i < stepStates.Length; i++)
        {
            stepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestStepInde, QuestStepState[] questStepStates)
    {
        this.questInfo = questInfo;
        this.state = questState;
        this.currentQuestStepIndex = currentQuestStepInde;
        this.stepStates = questStepStates;

        if (this.stepStates.Length != this.questInfo.questSteps.Length)
        {
            Debug.Log("Data is not in sync. Step States and Quest Steps are not of the same length");
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < questInfo.questSteps.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parent)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parent).GetComponent<QuestStep>(); //Open for object pooling
            questStep.InitializeQuestStep(questInfo.ID, currentQuestStepIndex, stepStates[currentQuestStepIndex].state);        
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if(CurrentStepExists())
        {
            questStepPrefab = questInfo.questSteps[currentQuestStepIndex];
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState stepState, int stepIndex)
    {
        if(stepIndex < questInfo.questSteps.Length)
        {
            this.stepStates[stepIndex].state = stepState.state;
        }
        else
        {
            Debug.Log("Index " + stepIndex + " is out of bounds.");
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, stepStates);
    }
}
