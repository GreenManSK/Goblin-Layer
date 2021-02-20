using System.Collections;
using Controllers;
using Events;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Bars
{
    public class DatingBar : AUIBar, IEventListener
    {
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
                StartCoroutine(UpdateSize(sizeUpdates, GameController.Instance.datingRestartTimeInS));
            }
        }
    }
}