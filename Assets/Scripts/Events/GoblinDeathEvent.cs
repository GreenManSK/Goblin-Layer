using UnityEngine;

namespace Events
{
    public class GoblinDeathEvent : IEvent
    {
        public GameObject Object { get; private set; }

        public GoblinDeathEvent(GameObject o)
        {
            Object = o;
        }
    }
}