using Events;
using Services;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class AttackingGoblin : AGoblinBehaviour
    {
        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            GameEventSystem.Send(new AttackEvent(10));
            Context.lastAttack = Time.time;
            Context.Chase();
        }
        
        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Attacking;
        }
    }
}