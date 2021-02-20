using System;
using Controllers;
using Events;
using Events.UI;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Components.Inventory
{
    public class InventorySpotController : MonoBehaviour
    {
        public delegate void PointerDelegate(InventoryItem item, bool enter);

        private static float? iconSize = null;
        private static float? quantitySize = null;

        public event PointerDelegate OnPointer;
        
        public GameObject button;
        public TMP_Text icon;
        public TMP_Text quantity;

        private InventoryItem _item;

        private bool _sizesSet = false;

        public void Start()
        {
            if (iconSize != null && quantitySize != null)
            {
                _sizesSet = true;
                icon.fontSize = iconSize.Value;
                quantity.fontSize = quantitySize.Value;
                icon.enableAutoSizing = false;
                quantity.enableAutoSizing = false;
            }
            else
            {
                icon.fontSize = icon.fontSizeMin;
                quantity.fontSize = quantity.fontSizeMin;
                icon.enableAutoSizing = true;
                quantity.enableAutoSizing = true;
            }
        }

        private void Update()
        {
            if (!_sizesSet && !Mathf.Approximately(icon.fontSize, icon.fontSizeMin) &&
                !Mathf.Approximately(quantity.fontSize, quantity.fontSizeMin))
            {
                _sizesSet = true;
                iconSize = icon.fontSize;
                quantitySize = quantity.fontSize;
                icon.enableAutoSizing = false;
                quantity.enableAutoSizing = false;
            }
        }

        public void SetData(InventoryItem item)
        {
            _item = item;
            if (item != null)
            {
                icon.text = item.present.icon;
                quantity.text = item.quantity.ToString();
            }

            button.SetActive(item != null);
        }

        public void OnClick()
        {
            GameEventSystem.Send(new PresentSelectEvent(_item.present));
        }

        public void OnPointerEnter()
        {
            OnPointer?.Invoke(_item, true);
        }

        public void OnPointerExit()
        {
            OnPointer?.Invoke(_item, false);
        }
    }
}