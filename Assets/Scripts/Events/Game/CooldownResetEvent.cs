using Data;

namespace Events.Game
{
    public class CooldownResetEvent : IEvent
    {
        public CooldownType Type { get; }
        public float TimeInS { get; }

        public CooldownResetEvent(CooldownType type, float timeInS)
        {
            Type = type;
            TimeInS = timeInS;
        }
    }
}