using UnityEngine;

namespace Objects.Player.Behaviours
{
    public class MovingPlayer: APlayerBehaviour
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var change = Player.moveSpeed * Time.deltaTime * Player.movement;
            Player.Rigidbody2D.MovePosition(Player.Rigidbody2D.position + change);
        }

        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Moving;
        }
    }
}