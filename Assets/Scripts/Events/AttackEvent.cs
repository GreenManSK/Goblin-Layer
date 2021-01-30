namespace Events
{
    public class AttackEvent : IEvent
    {
        public float Damage;

        public AttackEvent(float damage)
        {
            Damage = damage;
        }
    }
}