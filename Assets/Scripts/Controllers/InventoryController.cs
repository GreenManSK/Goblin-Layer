using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

namespace Controllers
{
    public class InventoryController : MonoBehaviour
    {
        public List<InventoryItem> items = new List<InventoryItem>();

        public void Add(Present present)
        {
            var item = GetItem(present);
            if (item == null)
            {
                items.Add(new InventoryItem(present));
            }
            else
            {
                item.quantity++;
            }
        }

        public void Remove(Present present)
        {
            var item = GetItem(present);
            if (item == null) return;
            item.quantity--;
            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
        }

        private InventoryItem GetItem(Present present)
        {
            return items.FirstOrDefault(i => i.present.Equals(present));
        }
    }

    [Serializable]
    public class InventoryItem
    {
        public Present present;
        public int quantity;

        public InventoryItem(Present present, int quantity = 1)
        {
            this.present = present;
            this.quantity = quantity;
        }
    }
}