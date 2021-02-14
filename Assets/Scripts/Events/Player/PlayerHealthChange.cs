namespace Events.Player
{
    public class PlayerHealthChange : IEvent
    {
        public float Health { get; }

        public PlayerHealthChange(float health)
        {
            Health = health;
        }
    }
}