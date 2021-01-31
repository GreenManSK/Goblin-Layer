using Entities.Types;
using Objects.Golbin;

namespace Events
{
    public class SeductionEvent : IEvent
    {
        public GoblinController Target { get; }
        public SeductionType Type { get; }
        public float Strength { get; }
        public bool ByPlayer { get; }

        public SeductionEvent(GoblinController target, SeductionType type, float strength, bool byPlayer = true)
        {
            Target = target;
            Type = type;
            Strength = strength;
            ByPlayer = byPlayer;
        }
    }
}