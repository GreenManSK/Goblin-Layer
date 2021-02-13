using Objects.Golbin;

namespace Events.Goblin
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