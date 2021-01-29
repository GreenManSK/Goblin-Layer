using System;
using Objects.Player.Behaviours;
using Objects.StateMachine;
using UnityEngine;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateController : AStateController<PlayerController, PlayerState>
    {
        protected override PlayerController GetContext()
        {
            return GetComponent<PlayerController>();
        }

        protected override IBehaviour<PlayerController, PlayerState> CreateBehaviour(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Idle:
                    return new IdlePlayer();
                    ;
                case PlayerState.Moving:
                    return new MovingPlayer();
                case PlayerState.Dating:
                    return new DatingPlayer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}