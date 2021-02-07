using Controllers;
using Events;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Components.Inventory
{
    public class InventorySpotController : MonoBehaviour
    {
        public delegate void PointerDelegate(InventoryItem item, bool enter);

        public event PointerDelegate OnPointer;
        
        public GameObject button;
        public TMP_Text icon;
        public TMP_Text quantity;

        private InventoryItem _item;
        
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