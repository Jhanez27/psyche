using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUIItem : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
    {
        //Item Properties
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text quantityText;
        [SerializeField]
        private Image borderImage;

        //Item Events
        public event Action<InventoryUIItem> OnItemClicked;
        public event Action<InventoryUIItem> OnItemDroppedOn;
        public event Action<InventoryUIItem> OnItemBeginDrag;
        public event Action<InventoryUIItem> OnItemEndDrag;
        public event Action<InventoryUIItem> OnItemRightMouseButtonClick;

        private bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void SetData(Sprite sprite, int quantity)
        {
            if (sprite != null && quantity > 0)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = sprite;
                quantityText.text = quantity.ToString();

                empty = false;
            }
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnItemRightMouseButtonClick?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!empty) { OnItemBeginDrag?.Invoke(this); }
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }
    }
}