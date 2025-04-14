using System.Threading;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID(); // Unique ID for the item

        //Item Properties
        [field: SerializeField]
        public bool IsStackable { get; set; }
        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1; // Maximum stack size for the item
        [field: SerializeField]
        public string Name { get; set; } // Name of the item
        [field: SerializeField]
        [TextArea]
        public string Description { get; set; } // Description of the item
        [field: SerializeField]
        public Sprite Image { get; set; } // Icon of the item
    }
}
