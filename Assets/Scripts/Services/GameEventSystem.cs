using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Services
{
    public class GameEventSystem : MonoBehaviour
    {
        private static GameEventSystem _instance;

        public static GameEventSystem Instance
        {
            get
            {
                Init();
                return _instance;
            }
        }

        private readonly Dictionary<Type, List<IEventListener>> _listeners =
            new Dictionary<Type, List<IEventListener>>();

        private bool _isSending = false;
        private Queue<IEvent> _queue = new Queue<IEvent>();

        private static void Init()
        {
            if (_instance != null) return;
            var obj = new GameObject {name = "EventSystem"};
            _instance = obj.AddComponent<GameEventSystem>();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public static void Send<T>(T @event) where T : IEvent
        {
            if (Instance._isSending)
            {
                _instance._queue.Enqueue(@event);
            }
            else
            {
                _instance.SendEvent(@event);
                _instance.MoveQueue();
            }
        }

        private void SendEvent<T>(T @event) where T : IEvent
        {
            _isSending = true;
            var type = @event.GetType();
            if (_listeners.ContainsKey(type))
            {
                var listeners = _listeners[type];
                for (var i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEvent(@event);
                }
            }

            _isSending = false;
        }

        private void MoveQueue()
        {
            while (_queue.Count > 0 && !_isSending)
            {
                SendEvent(_queue.Dequeue());
            }
        }

        public static void Subscribe(IEnumerable<Type> types, IEventListener listener)
        {
            foreach (var type in types)
            {
                Subscribe(type, listener);
            }
        }

        public static void Unsubscribe(IEnumerable<Type> types, IEventListener listener)
        {
            foreach (var type in types)
            {
                Unsubscribe(type, listener);
            }
        }

        public static void Subscribe(Type type, IEventListener listener)
        {
            var instance = Instance;
            if (!instance._listeners.ContainsKey(type))
            {
                instance._listeners.Add(type, new List<IEventListener>());
            }

            instance._listeners[type].Add(listener);
        }

        public static void Unsubscribe(Type type, IEventListener listener)
        {
            if (Instance._listeners.ContainsKey(type))
                Instance._listeners[type]?.Remove(listener);
        }
    }
}