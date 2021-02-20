using Data;

namespace Events.Game
{
    public class CooldownUpdateEvent : IEvent
    {
        public CooldownType Type { get; }
        public float Value { get; }
        public bool Charged { get; }

        public CooldownUpdateEvent(CooldownType type, float value, bool charged = false)
        {
            Type = type;
            Charged = charged;
            Value = value;
        }
    }
}