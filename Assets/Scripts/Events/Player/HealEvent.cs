namespace Events.Player
{
    public class HealEvent : IEvent
    {
        public float Health { get; }

        public HealEvent(float health)
        {
            Health = health;
        }
    }
}