using System.Collections;
using Events;
using Services;
using UnityEngine;

namespace UI
{
    public class DatingBar : MonoBehaviour, IEventListener<DateEvent>
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

        public void OnEvent(DateEvent @event)
        {
            if (@event.Start)
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