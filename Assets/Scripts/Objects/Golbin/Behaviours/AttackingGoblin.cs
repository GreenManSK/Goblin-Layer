using Events;
using Services;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class AttackingGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            context.Animator.SetTrigger(Animation);
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