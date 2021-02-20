using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using Events;
using Events.Game;
using UnityEngine;

namespace Services.Cooldown
{
    public class CooldownManager : MonoBehaviour, IEventListener
    {
        private const float PauseWaitInS = 0.1f;

        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(CooldownResetEvent)
        }.AsReadOnly();

        public float updateSpeed = 10;

        private Dictionary<CooldownType, IEnumerator> coroutines = new Dictionary<CooldownType, IEnumerator>();
        private bool _paused = false;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        private void OnDisable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is CooldownResetEvent cooldownResetEvent)
            {
                OnResetEvent(cooldownResetEvent);
            }
        }

        private void OnResetEvent(CooldownResetEvent @event)
        {
            if (coroutines.ContainsKey(@event.Type))
            {
                StopCoroutine(coroutines[@event.Type]);
            }
            coroutines[@event.Type] = CreateCooldown(@event.Type, @event.TimeInS);
            StartCoroutine(coroutines[@event.Type]);
        }

        private IEnumerator CreateCooldown(CooldownType type, float timeInMs)
        {
            var timeDelta = timeInMs / updateSpeed;
            var sizeDelta = 1 / updateSpeed;
            var value = 0f;
            do
            {
                GameEventSystem.Send(new CooldownUpdateEvent(type, value, false));
                yield return new WaitForSeconds(timeDelta);
                value += sizeDelta;
                while (_paused)
                {
                    yield return new WaitForSeconds(PauseWaitInS);
                }
            } while (value < 1f);

            GameEventSystem.Send(new CooldownUpdateEvent(type, 1f, true));
        }
    }
}