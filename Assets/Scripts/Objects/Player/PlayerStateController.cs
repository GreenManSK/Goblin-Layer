using System;
using System.Collections.Generic;
using Objects.Player.Behaviours;
using UnityEngine;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateController : MonoBehaviour
    {
        private readonly Dictionary<PlayerState, IPlayerBehaviour> _behaviours =
            new Dictionary<PlayerState, IPlayerBehaviour>();

        public IPlayerBehaviour Behaviour { get; private set; }

        private PlayerController _player;

        private void Start()
        {
            _player = GetComponent<PlayerController>();
        }

        private void Update()
        {
            Behaviour?.OnUpdate();
        }

        private void FixedUpdate()
        {
            Behaviour?.OnFixedUpdate();
        }

        public bool IsState(PlayerState state)
        {
            return Behaviour != null && Behaviour.IsState(state);
        }

        public void ChangeState(PlayerState state)
        {
            if (Behaviour != null && Behaviour.IsState(state))
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

            Behaviour?.OnTransitionOut();
            Behaviour = _behaviours[state];
            Behaviour.OnTransitionIn(_player);
        }
    }
}