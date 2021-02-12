using System.Collections;
using Controllers;
using Events;
using Services;
using UnityEngine;

namespace UI
{
    public class DatingBar : MonoBehaviour, IEventListener
    {
        public RectTransform bar;
        public float sizeUpdates = 10;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(DateEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(DateEvent), this);
        }

        public void OnEvent(IEvent @event)
        {
            if (!(@event is DateEvent dateEvent)) return;
            if (dateEvent.Start)
            {
                UpdateScale(0);
            }
            else
            {
                StartCoroutine(UpdateSize(sizeUpdates));
            }
        }

        private IEnumerator UpdateSize(float updates)
        {
            var timeDelta = GameController.Instance.datingRestartTimeInS / updates;
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