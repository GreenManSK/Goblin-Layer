using System.Collections;
using Controllers;
using Events;
using Services;
using UnityEngine;

namespace UI
{
    public class AttackBar : MonoBehaviour, IEventListener
    {
        public RectTransform bar;
        public float sizeUpdates = 10;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(AttackBarEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(AttackBarEvent), this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is AttackBarEvent attackBarEvent)
            {
                UpdateScaleX(0);
                StartCoroutine(UpdateSize(sizeUpdates, attackBarEvent.WaitTimeInS));
            }
        }

        private IEnumerator UpdateSize(float updates, float waitTimeInS)
        {
            var timeDelta = waitTimeInS / updates;
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