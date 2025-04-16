using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "ImportantItemSO", menuName = "Scriptable Objects/ImportantItemSO")]
    public class ImportantItemSO : ItemSO, IItemAction
    {
        public string ActionName => "Use";
        [field: SerializeField]
        public AudioClip ActionSFX { get; private set; }
        public bool PerformAction(GameObject character)
        {
            // Implement the action logic for important items here
            Debug.Log($"Using important item: {name}");
            return true;
        }

    }
}