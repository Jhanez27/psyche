using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUIDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private TMP_Text itemDescription;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            Debug.Log("Item Image is null: " + (itemImage == null).ToString());                   // could be false
            Debug.Log("Item Image Game Object is null: " + (itemImage.gameObject == null).ToString());        // true if destroyed
            itemImage.gameObject.SetActive(false);
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
        }

        public void SetDescription(Sprite sprite, string name, string description)
        {
            if (sprite != null && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = sprite;
                itemName.text = name;
                itemDescription.text = description;
            }
        }
    }
}