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
            }
            
            if (currentAmount >= requiredAmount)
            {
                FinishQuestStep();
            }
        }
    }
}
