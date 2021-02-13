using Objects.Golbin;

namespace Events.Goblin
{
    public class SeductionChangeEvent : IEvent
    {
        public GoblinController Target { get; }
        public float Change { get; }

        public SeductionChangeEvent(GoblinController target, float change)
        {
            Target = target;
            Change = change;
        }
    }
}