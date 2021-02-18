using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Components.Compendium
{
    public abstract class AppearanceSectionController<T> : MonoBehaviour
    {
        public GameObject itemPrefab;
        public TMP_Text title;

        private readonly List<GameObject> _items = new List<GameObject>();

        private void OnEnable()
        {
            title.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            title.gameObject.SetActive(false);
        }

        public void SetItems(IEnumerable<T> appearances)
        {
            _items.ForEach(Destroy);
            foreach (var appearance in appearances)
            {
                var go = Instantiate(itemPrefab, transform);
                SetData(appearance, go);
                _items.Add(go);
            }
        }

        protected abstract void SetData(T appearance, GameObject go);
    }
}