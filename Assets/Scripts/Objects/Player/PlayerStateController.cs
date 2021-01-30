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
            return state switch
            {
                PlayerState.Idle => new IdlePlayer(),
                PlayerState.Moving => new MovingPlayer(),
                PlayerState.Dating => new DatingPlayer(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}