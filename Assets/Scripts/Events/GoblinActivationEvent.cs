using UnityEngine;

namespace Events
{
    public class GoblinActivationEvent : IEvent
    {
        public GameObject Object { get; private set; }

        public GoblinActivationEvent(GameObject o)
        {
            Object = o;
        }
    }
}