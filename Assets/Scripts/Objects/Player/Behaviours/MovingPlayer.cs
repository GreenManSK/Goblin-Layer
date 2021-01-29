using UnityEngine;

namespace Objects.Player.Behaviours
{
    public class MovingPlayer : APlayerBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Move");

        public override void OnTransitionIn(PlayerController context)
        {
            base.OnTransitionIn(context);
            Context.Animator.SetTrigger(Animation);
        }

        public override void OnFixedUpdate()
        {
            var change = Context.moveSpeed * Time.deltaTime * Context.movement;
            Context.Rigidbody2D.MovePosition(Context.Rigidbody2D.position + change);
        }

        public override void OnUpdate()
        {
            Context.FixFlip();
        }

        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Moving;
        }
    }
}