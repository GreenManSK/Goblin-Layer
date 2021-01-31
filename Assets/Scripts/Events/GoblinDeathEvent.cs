using Objects.Golbin;
using UnityEngine;

namespace Events
{
    public class GoblinDeathEvent : IEvent
    {
        public GoblinController Object { get; private set; }

        public GoblinDeathEvent(GoblinController o)
        {
            Object = o;
        }
    }
}