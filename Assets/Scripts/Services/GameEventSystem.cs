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

        private static void Init()
        {
            if (_instance != null) return;
            var obj = new GameObject {name = "EventSystem"};
            _instance = obj.AddComponent<GameEventSystem>();
            DontDestroyOnLoad(obj);
        }

        public static void Send<T>(T @event) where T : IEvent
        {
            var type = @event.GetType();
            var instance = Instance;
            if (!instance._listeners.ContainsKey(type)) return;
            var listeners = instance._listeners[type];
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                (listeners[i] as IEventListener<T>)?.OnEvent(@event);
            }
        }

        public static void Subscribe<T>(IEventListener<T> listener) where T : IEvent
        {
            Subscribe(typeof(T), listener);
        }

        public static void Unsubscribe<T>(IEventListener<T> listener) where T : IEvent
        {
            Unsubscribe(typeof(T), listener);
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