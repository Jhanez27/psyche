using Inventory.Model;
using System;
using UnityEngine;

public class FollowChrisStep : QuestStep
{
    [Header("Requirement")]
    [SerializeField]
    private ItemSO item;
    [SerializeField]
    private int requiredAmount = 1;

    private bool hasInteractedWithPipes = false; 

    private int currentAmount = 0;



    private void Start()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.inventoryModelEvents.OnItemAdded += GetItemAdded;
        GamesEventManager.Instance.questEvents.OnInteractInCollision += CheckCollision;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.inventoryModelEvents.OnItemAdded -= GetItemAdded;
        GamesEventManager.Instance.questEvents.OnInteractInCollision -= CheckCollision;
    }

    private void CheckCollision(string tag)
    {
        if(tag.Equals("Pipes") && !hasInteractedWithPipes)
        {
            hasInteractedWithPipes = true; // Set the flag to true after interacting with the pipes
            UpdateState(); // Update the quest step state
            Debug.Log("Pipes Interacted with");
            CheckFinalCondition(); // Check if the quest step is complete
        }
    }

    private void CheckFinalCondition()
    {
        if (currentAmount >= requiredAmount && hasInteractedWithPipes)
        {
            FinishQuestStep();
        }
    }

    private void GetItemAdded(ItemSO item, int quantity)
    {
        if (item.ID.Equals(this.item.ID))
        {
            if (currentAmount < requiredAmount)
            {
                currentAmount += quantity;
                UpdateState();
            }

            Debug.Log("Item Added: " + item.name + " x" + quantity);
            CheckFinalCondition(); // Check if the quest step is complete
        }
    }
    

    private void UpdateState()
    {
        /*
        string state = currentAmount.ToString();
        string status = "Collected " + currentAmount + "/" + requiredAmount + " " + item.name + ".";
        ChangeState(state, status);
        */

        string state = currentAmount.ToString() + "," + hasInteractedWithPipes.ToString();
        string status = "Collected " + currentAmount + "/" + requiredAmount + " " + item.name + ".\nInteract With Pipes.";
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
        if (parts.Length == 2)
        {
            this.currentAmount = int.Parse(parts[0]);
            this.hasInteractedWithPipes = bool.Parse(parts[1]);
        }
        UpdateState();
    }
}
