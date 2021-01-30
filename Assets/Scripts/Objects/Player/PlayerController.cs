using System;
using Events;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IEventListener<AttackEvent>
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;

        public Vector2 movement = Vector2.zero;
        public float moveSpeed = 1f;
        public float health = 100f;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sprite;
        private Animator _animator;

        private PlayerStateController _playerStateController;
        private PlayerInputController _playerInputController;

        private void Start()
        {
            _playerStateController = GetComponent<PlayerStateController>();
            _playerInputController = GetComponent<PlayerInputController>();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            RegisterInputs();
            _playerStateController.ChangeState(PlayerState.Idle);
        }

        private void OnEnable()
        {
            GameEventSystem.Subscribe(this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(this);
        }

        private void RegisterInputs()
        {
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Performed, Move);
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Canceled, Stop);
        }

        public void FixFlip()
        {
            if (!Mathf.Approximately(movement.x, 0))
            {
                _sprite.flipX = movement.x < 0;
            }
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            movement = ctx.ReadValue<Vector2>();
            if (_playerStateController.IsState(PlayerState.Idle))
            {
                _playerStateController.ChangeState(PlayerState.Moving);
            }
        }

        private void Stop(InputAction.CallbackContext ctx)
        {
            movement = Vector2.zero;
            if (_playerStateController.IsState(PlayerState.Moving))
            {
                _playerStateController.ChangeState(PlayerState.Idle);
            }
        }

        public void OnEvent(AttackEvent @event)
        {
            health -= @event.Damage;
            _sprite.color = Color.Lerp(Color.red, Color.white, Mathf.Max(0, health / 100));
            if (health < 0)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}