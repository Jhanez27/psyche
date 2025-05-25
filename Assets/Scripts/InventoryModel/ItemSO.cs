using System.Threading;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public string ID { get; private set; } // Unique ID for the item

        //Item Properties
        [field: SerializeField]
        public bool IsStackable { get; set; }
        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1; // Maximum stack size for the item
        [field: SerializeField]
        public string Name { get; set; } // Name of the item
        [field: SerializeField, TextArea]
        public string Description { get; set; } // Description of the item
        [field: SerializeField]
        public Sprite Image { get; set; } // Icon of the item
    }
}
