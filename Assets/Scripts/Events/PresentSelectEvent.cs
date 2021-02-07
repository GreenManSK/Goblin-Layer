using Entities;

namespace Events
{
    public class PresentSelectEvent : IEvent
    {
        public Present Present { get; }

        public PresentSelectEvent(Present present)
        {
            Present = present;
        }
    }
}