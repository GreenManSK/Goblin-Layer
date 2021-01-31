using Objects.Golbin;
using UnityEngine;

namespace Events
{
    public class GoblinActivationEvent : IEvent
    {
        public GoblinController Object { get; private set; }

        public GoblinActivationEvent(GoblinController o)
        {
            Object = o;
        }
    }
}