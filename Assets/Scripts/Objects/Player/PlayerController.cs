using System;
using System.Collections.Generic;
using Objects.Player.Behaviours;
using UnityEngine;

namespace Objects.Player
{
    public class PlayerController : MonoBehaviour
    {
        private IPlayerBehaviour _behaviour;

        private readonly Dictionary<PlayerState, IPlayerBehaviour> _behaviours =
            new Dictionary<PlayerState, IPlayerBehaviour>();

        private void Start()
        {
            ChangeState(PlayerState.Idle);
        }

        private void Update()
        {
            _behaviour?.OnUpdate();
        }

        private void ChangeState(PlayerState state)
        {
            if (_behaviour != null && _behaviour.IsState(state))
            {
                return;
            }

            if (!_behaviours.ContainsKey(state))
            {
                switch (state)
                {
                    case PlayerState.Idle:
                        _behaviours.Add(PlayerState.Idle, new IdlePlayer());
                        break;
                    case PlayerState.Moving:
                        _behaviours.Add(PlayerState.Moving, new MovingPlayer());
                        break;
                    case PlayerState.Dating:
                        _behaviours.Add(PlayerState.Dating, new DatingPlayer());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            }

            _behaviour?.OnTransitionOut();
            _behaviour = _behaviours[state];
            _behaviour.OnTransitionIn(this);
        }
    }
}