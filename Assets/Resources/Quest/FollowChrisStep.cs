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

    private int currentAmount = 0;

    private void Start()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.inventoryModelEvents.OnItemAdded += GetItemAdded;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.inventoryModelEvents.OnItemAdded += GetItemAdded;
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
            
            if (currentAmount >= requiredAmount)
            {
                FinishQuestStep();
            }
        }
    }

    private void UpdateState()
    {
        string state = currentAmount.ToString();
        string status = "Collected " + currentAmount + "/" + requiredAmount + " " + item.name + ".";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        this.currentAmount = System.Int32.Parse(state);
        UpdateState();
    }
}
