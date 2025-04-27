using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        public void AddButton(string name, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponent<Button>().onClick.AddListener(() => onClickAction());
            button.GetComponentInChildren<TMP_Text>().text = name;
        }

        public void Toggle(bool val)
        {
            if(val)
            {
                RemoveOldButtons();
            }
            gameObject.SetActive(val);
        }

        private void RemoveOldButtons()
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}