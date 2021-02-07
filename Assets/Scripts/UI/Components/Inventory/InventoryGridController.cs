using System;
using System.Collections.Generic;
using Constants;
using Controllers;
using Objects.Player;
using TMPro;
using UnityEngine;

namespace UI.Components.Inventory
{
    public class InventoryGridController : MonoBehaviour
    {
        public GameObject rowPrefab;
        public GameObject itemSpotPrefab;

        public GameObject description;
        public TMP_Text descriptionTitle;
        public TMP_Text descriptionText;

        private List<InventorySpotController> _items = new List<InventorySpotController>();

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
                descriptionTitle.text = item.present.name;
                descriptionText.text = item.present.description;
            }
            description.SetActive(item != null && enter);
        }
    }
}