using UnityEngine;

namespace Objects.Player.Behaviours
{
    public class MovingPlayer : APlayerBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Move");

        public override void OnTransitionIn(PlayerController context)
        {
            base.OnTransitionIn(context);
            Debug.Log("move");
            Player.Animator.SetTrigger(Animation);
        }

        public override void OnFixedUpdate()
        {
            var change = Player.moveSpeed * Time.deltaTime * Player.movement;
            Player.Rigidbody2D.MovePosition(Player.Rigidbody2D.position + change);
        }

        public override void OnUpdate()
        {
            Player.FixFlip();
        }

        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Moving;
        }
    }
}