using UnityEngine;

namespace Objects.Player.Behaviours
{
    public class IdlePlayer : APlayerBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(PlayerController context)
        {
            base.OnTransitionIn(context);
            Player.Animator.SetTrigger(Animation);
        }

        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Idle;
        }
    }
}