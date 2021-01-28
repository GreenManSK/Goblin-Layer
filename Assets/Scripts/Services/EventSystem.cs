using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Services
{
    public class EventSystem : MonoBehaviour
    {
        private static EventSystem _instance;

        public static EventSystem Instance
        {
            get
            {
                Init();
                return _instance;
            }
        }

        private readonly Dictionary<Type, List<IEventListener>> _listeners =
            new Dictionary<Type, List<IEventListener>>();

        private static void Init()
        {
            if (_instance != null) return;
            var obj = new GameObject {name = "EventSystem"};
            _instance = obj.AddComponent<EventSystem>();
            DontDestroyOnLoad(obj);
        }

        public static void Send<T>(T @event) where T : IEvent
        {
            var type = @event.GetType();
            var instance = Instance;
            if (!instance._listeners.ContainsKey(type)) return;
            foreach (var listener in instance._listeners[type])
            {
                (listener as IEventListener<T>)?.OnEvent(@event);
            }
        }

        public static void Subscribe<T>(IEventListener<T> listener) where T : IEvent
        {
            var type = typeof(T);
            var instance = Instance;
            if (!instance._listeners.ContainsKey(type))
            {
                instance._listeners.Add(type, new List<IEventListener>());
            }

            instance._listeners[type].Add(listener);
        }

        public static void Unsubscribe<T>(IEventListener<T> listener) where T : IEvent
        {
            var type = typeof(T);
            if (Instance._listeners.ContainsKey(type))
                Instance._listeners[typeof(T)]?.Remove(listener);
        }
    }
}