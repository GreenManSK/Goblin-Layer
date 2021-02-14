using System;
using Objects.Player.Behaviours;
using Objects.StateMachine;
using UnityEngine;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateController : AStateController<PlayerController, PlayerState>
    {
        public PlayerState LastState => _lastState;
        private PlayerState _lastState;

        protected override PlayerController GetContext()
        {
            return GetComponent<PlayerController>();
        }

        public override void ChangeState(PlayerState state)
        {
            _lastState = CurrentState;
            base.ChangeState(state);
        }

        protected override IBehaviour<PlayerController, PlayerState> CreateBehaviour(PlayerState state)
        {
            return state switch
            {
                PlayerState.Idle => new IdlePlayer(),
                PlayerState.Moving => new MovingPlayer(),
                PlayerState.Dating => new DatingPlayer(),
                PlayerState.Attacking => new AttackingPlayer(),
                PlayerState.Stopped => new StoppedPlayer(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}