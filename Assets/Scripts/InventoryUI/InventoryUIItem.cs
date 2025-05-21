using System;
using System.Collections.Generic;
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

        private bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (var result in results)
                {
                    Debug.Log($"Raycast hit: {result.gameObject.name}");
                }
            }
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
            Debug.Log("Pointer Clicked on Item");
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                GamesEventManager.Instance.inventoryUIEvents.ItemRightMouseButtonClicked(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                GamesEventManager.Instance.inventoryUIEvents.ItemClicked(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            return;
            // No implementation here, only Begin and Drop PointerEvents are needed
            // The function is needed for the IDragHandler interface
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!empty) 
            { 
                GamesEventManager.Instance.inventoryUIEvents.ItemBeginDrag(this);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            GamesEventManager.Instance.inventoryUIEvents.ItemDroppedOn(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GamesEventManager.Instance.inventoryUIEvents.ItemEndDrag(this);
        }
    }
}