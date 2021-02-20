using System;
using System.Collections.Generic;
using Constants;
using Controllers;
using TMPro;
using UnityEngine;

namespace UI.Components.Inventory
{
    public class InventoryGridController : MonoBehaviour
    {
        public GameObject rowPrefab;
        public GameObject itemSpotPrefab;

        public GameObject description;
        public TMP_Text descriptionText;
        
        public TMP_Text closeButtonText;

        private List<InventorySpotController> _items = new List<InventorySpotController>();
        private bool _descriptionFontSizeSet = false;
        private bool _closeButtonFontSizeSet = false;

        private void Start()
        {
            closeButtonText.fontSize = closeButtonText.fontSizeMin;
            closeButtonText.enableAutoSizing = true;
        }

        private void OnEnable()
        {
            if (_items.Count <= 0)
            {
                CreateItems();
            }
            OnPointer(null, false);
        }

        private void OnDestroy()
        {
            _items.ForEach(i => i.OnPointer -= OnPointer);
        }

        private void Update()
        {
            if (!_closeButtonFontSizeSet && !Mathf.Approximately(closeButtonText.fontSize, closeButtonText.fontSizeMin))
            {
                closeButtonText.enableAutoSizing = false;
                _closeButtonFontSizeSet = true;
            }
        }

        private void CreateItems()
        {
            for (var i = 0; i < Game.InventoryRows; i++)
            {
                var row = Instantiate(rowPrefab, transform);
                for (var j = 0; j < Game.InventoryColumns; j++)
                {
                    var item = Instantiate(itemSpotPrefab, row.transform);
                    var spot = item.GetComponent<InventorySpotController>();
                    spot.OnPointer += OnPointer;
                    _items.Add(spot);
                }
            }
        }

        public void SetItems(List<InventoryItem> inventoryItems)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].SetData(inventoryItems.Count > i ? inventoryItems[i] : null);
            }
        }

        private void OnPointer(InventoryItem item, bool enter)
        {
            if (item != null && enter)
            {
                SetDescriptionFontSize();
                descriptionText.text = $"<b>{item.present.name}</b>\n{item.present.description}";
            }
            description.SetActive(item != null && enter);
        }

        private void SetDescriptionFontSize()
        {
            if (_descriptionFontSizeSet)
                return;
            description.SetActive(true);
            descriptionText.enableAutoSizing = true;
            descriptionText.ForceMeshUpdate();
            descriptionText.enableAutoSizing = false;
            description.SetActive(false);
            _descriptionFontSizeSet = true;
        }
    }
}