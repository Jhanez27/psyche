using UnityEngine;

public class ApproachHeinrich : QuestStep
{
    private bool hasInteractedWithHeinrich = false;

    private void Start()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.questEvents.OnInteractInCollision += CheckCollision;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.questEvents.OnInteractInCollision -= CheckCollision;
    }

    private void CheckCollision(string tag)
    {
        if (tag.Equals("Heinrich") && !hasInteractedWithHeinrich)
        {
            hasInteractedWithHeinrich = true; // Set the flag to true after interacting with the pipes
            UpdateState(); // Update the quest step state
            CheckFinalCondition(); // Check if the quest step is complete
        }
    }

    private void CheckFinalCondition()
    {
        if (hasInteractedWithHeinrich)
        {
            FinishQuestStep();
        }
    }


    private void UpdateState()
    {
        string state = hasInteractedWithHeinrich.ToString();
        string status = "Go to Heinrich and claim your paper.";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        /*
        this.currentAmount = System.Int32.Parse(state);
        this.hasInteractedWithPipes = System.Boolean.Parse(state);
        UpdateState();
        */
        string[] parts = state.Split(',');
        if (parts.Length == 1)
        {
            this.hasInteractedWithHeinrich = bool.Parse(parts[0]);
        }
        UpdateState();
    }
}
