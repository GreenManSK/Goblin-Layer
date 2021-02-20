using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using Events;
using Events.Game;
using UnityEngine;

namespace Services
{
    public enum CooldownManagerState
    {
        Running,
        Date,
        Stopped
    }

    public class CooldownManager : MonoBehaviour, IEventListener
    {
        private const float PauseWaitInS = 0.1f;

        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(CooldownResetEvent),
            typeof(DateEvent),
            typeof(StopEvent),
            typeof(ResumeEvent)
        }.AsReadOnly();

        public float updateSpeed = 10;

        private Dictionary<CooldownType, IEnumerator> coroutines = new Dictionary<CooldownType, IEnumerator>();

        private CooldownManagerState _state = CooldownManagerState.Running;
        private CooldownManagerState _previousState = CooldownManagerState.Running;

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
            switch (@event)
            {
                case CooldownResetEvent cooldownResetEvent:
                    OnResetEvent(cooldownResetEvent);
                    break;
                case DateEvent dateEvent:
                    OnDateEvent(dateEvent);
                    break;
                case StopEvent _:
                    OnStopEvent();
                    break;
                case ResumeEvent _:
                    OnResumeEvent();
                    break;
            }
        }

        private void OnResumeEvent()
        {
            SetState(_previousState);
        }

        private void OnStopEvent()
        {
            SetState(CooldownManagerState.Stopped);
        }

        private void OnDateEvent(DateEvent dateEvent)
        {
            SetState(dateEvent.Start ? CooldownManagerState.Date : CooldownManagerState.Running);
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
                while (_state != CooldownManagerState.Running)
                {
                    yield return new WaitForSeconds(PauseWaitInS);
                }
            } while (value < 1f);

            GameEventSystem.Send(new CooldownUpdateEvent(type, 1f, true));
        }

        private void SetState(CooldownManagerState state)
        {
            _previousState = _state;
            _state = state;
        }
    }
}