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

    [Header("Tracked Inventory")]
    [SerializeField]
    private PickupSystem pickupSystem;

    private int currentAmount = 0;

    private void OnEnable()
    {
        if (pickupSystem != null)
        {
            pickupSystem.OnItemAdded += GetItemAdded;
        }
        else
        {
            Debug.Log("Whatafakkk");
        }
    }

    private void OnDisable()
    {
        pickupSystem.OnItemAdded -= GetItemAdded;
    }

    public void Init(PickupSystem pickupSystem)
    {
        this.pickupSystem = pickupSystem;
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
