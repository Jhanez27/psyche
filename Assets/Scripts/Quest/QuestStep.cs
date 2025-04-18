using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questID;

    public void InitializeQuestStep(string questID)
    {
        this.questID = questID;
    }

    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            QuestManager.Instance.AdvanceQuest(questID);
            Destroy(gameObject);
        }
    }
}
