using System.Collections;
using Controllers;
using Events;
using Services;
using UnityEngine;

namespace UI
{
    public class AttackBar : MonoBehaviour, IEventListener<AttackBarEvent>
    {
        public RectTransform bar;
        public float sizeUpdates = 10;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(this);
        }

        public void OnEvent(AttackBarEvent @event)
        {
            UpdateScaleX(0);
            StartCoroutine(UpdateSize(sizeUpdates, @event.WaitTimeInS));
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