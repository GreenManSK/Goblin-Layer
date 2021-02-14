using System.Collections;
using System.Collections.Generic;
using Events;
using Events.Player;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AttackBar : MonoBehaviour, IEventListener
    {
        public RectTransform bar;
        public float sizeUpdates = 10;

        public GameObject icon;
        public Image barSprite;
        
        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(AttackBarEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(AttackBarEvent), this);
        }

        public void SetVisibility(bool visible)
        {
            var color = barSprite.color;
            color.a = visible ? 1 : 0;
            barSprite.color = color;
            icon.SetActive(visible);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is AttackBarEvent attackBarEvent)
            {
                UpdateScale(0);
                StartCoroutine(UpdateSize(sizeUpdates, attackBarEvent.WaitTimeInS));
            }
        }

        private IEnumerator UpdateSize(float updates, float waitTimeInS)
        {
            var timeDelta = waitTimeInS / updates;
            var sizeDelta = 1 / updates;
            while (!Mathf.Approximately(bar.localScale.y, 1))
            {
                UpdateScale(bar.localScale.y + sizeDelta);
                yield return new WaitForSeconds(timeDelta);
            }
        }

        private void UpdateScale(float y)
        {
            var localScale = bar.localScale;
            bar.localScale = new Vector3(localScale.x, y, localScale.z);
        }
    }
}