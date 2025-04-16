using Ink.Parsed;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "Scriptable Objects/ConsumableItemSO")]
    public class ConsumableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiers = new List<ModifierData>();
        public string ActionName => "Consume";

        [field: SerializeField]
        public AudioClip ActionSFX { get; private set; }

        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifiers)
            {
                data.StatModifier.AffectCharacter(character, data.Value);
            }
            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip ActionSFX { get; }
        bool PerformAction(GameObject character);
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO StatModifier;
        public float Value;
    }
}

