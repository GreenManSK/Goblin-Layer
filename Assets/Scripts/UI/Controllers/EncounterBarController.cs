using System;
using System.Collections.Generic;
using Objects.Golbin;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class EncounterBarController : MonoBehaviour
    {
        public GameObject itemPrefab;

        private List<EncounterBarItemController> _items = new List<EncounterBarItemController>();

        private void OnEnable()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        public void SetData(List<GoblinController> goblins)
        {
            _items.ForEach(item => item.gameObject.SetActive(false));

            for (var i = 0; i < goblins.Count; i++)
            {
                if (_items.Count <= i)
                {
                    _items.Add(CreateItem());
                }

                _items[i].SetData(goblins[i]);
                _items[i].gameObject.SetActive(true);
            }
        }

        public void SetActive(GoblinController active)
        {
            foreach (var item in _items)
            {
                item.SetActive(item.goblin == active);
            }
        }
        
        private EncounterBarItemController CreateItem()
        {
            var go = Instantiate(itemPrefab, transform);
            return go.GetComponent<EncounterBarItemController>();
        }
    }
}