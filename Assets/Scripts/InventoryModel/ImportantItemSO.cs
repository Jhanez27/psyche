using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "ImportantItemSO", menuName = "Scriptable Objects/ImportantItemSO")]
    public class ImportantItemSO : ItemSO, IItemAction
    {
        [SerializeField]
        List<Action> usages = new List<Action>();
        public string ActionName => "Use";
        [field: SerializeField]
        public AudioClip ActionSFX { get; private set; }
        public bool PerformAction(GameObject character)
        {
            foreach (Action action in usages)
            {
                action?.Invoke();
            }

            return true;
        }

    }
}