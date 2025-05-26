using UnityEngine;

public class TalkingBeforeFallingInLine : QuestStep
{
    private bool hasInteractedWithChris = false;

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
        if (tag.Equals("Chris") && !hasInteractedWithChris)
        {
            hasInteractedWithChris = true; // Set the flag to true after interacting with the pipes
            UpdateState(); // Update the quest step state
            CheckFinalCondition(); // Check if the quest step is complete
        }
    }

    private void CheckFinalCondition()
    {
        if (hasInteractedWithChris)
        {
            FinishQuestStep();
        }
    }


    private void UpdateState()
    {
        string state = hasInteractedWithChris.ToString();
        string status = "Talk to Chris before falling in line.";
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
            this.hasInteractedWithChris = bool.Parse(parts[0]);
        }
        UpdateState();
    }
}
