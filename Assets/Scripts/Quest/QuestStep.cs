using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questID;
    private int stepIndex;

    public void InitializeQuestStep(string questID, int stepIndex, string questStepState)
    {
        this.questID = questID;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != string.Empty)
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            GamesEventManager.Instance.questEvents.AdvanceQuest(questID);
            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState)
    {
        GamesEventManager.Instance.questEvents.ChangeQuestStepState(questID, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string newState);
}
