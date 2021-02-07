using Entities;
using Objects.Golbin;

namespace Events
{
    public class PresentEvent : IEvent
    {
        public Present Present { get; }
        public GoblinController Target { get; }

        public PresentEvent(Present present, GoblinController target)
        {
            Present = present;
            Target = target;
        }
    }
}