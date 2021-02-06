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
                UpdateScaleX(0);
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
            while (!Mathf.Approximately(bar.localScale.x, 1))
            {
                UpdateScaleX(bar.localScale.x + sizeDelta);
                yield return new WaitForSeconds(timeDelta);
            }
        }

        private void UpdateScaleX(float x)
        {
            var localScale = bar.localScale;
            bar.localScale = new Vector3(x, localScale.y, localScale.z);
        }
    }
}