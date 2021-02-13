using UnityEngine;

namespace Events.Goblin
{
    public class AttackEvent : IEvent
    {
        public GameObject Target;
        public float Damage;

        public AttackEvent(GameObject target, float damage)
        {
            Target = target;
            Damage = damage;
        }
    }
}