using Objects.Golbin;

namespace Events.Goblin
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