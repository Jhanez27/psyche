using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase 
{
    private static Dictionary<string, ItemSO> itemLookup;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        itemLookup = new Dictionary<string, ItemSO>();
        ItemSO[] allItems = Resources.LoadAll<ItemSO>("Items");

        foreach (ItemSO item in allItems)
        {
            if (!string.IsNullOrEmpty(item.ID))
            {
                itemLookup[item.ID] = item;
            }
            else
            {
                Debug.LogWarning($"ItemSO '{item.name}' has no ID assigned!");
            }
        }
    }

    public static ItemSO GetItemByID(string ID)
    {
        itemLookup.TryGetValue(ID, out ItemSO item);
        return item;
    }
}
