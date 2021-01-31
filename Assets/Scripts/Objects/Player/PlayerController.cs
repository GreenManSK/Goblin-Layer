using System.Collections;
using Controllers;
using Events;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IEventListener<AttackEvent>, IEventListener<DateEvent>
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;

        public Vector2 movement = Vector2.zero;
        public float moveSpeed = 1f;
        public float health = 100f;
        public bool canDate = true;

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
            GameEventSystem.Subscribe<AttackEvent>(this);
            GameEventSystem.Subscribe<DateEvent>(this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe<AttackEvent>(this);
            GameEventSystem.Unsubscribe<DateEvent>(this);
        }

        private void RegisterInputs()
        {
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Performed, Move);
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Canceled, Stop);
            _playerInputController.AddAction(_playerInputController.Actions.Date, ActionType.Performed, ToggleDate);
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
            if (_playerStateController.IsState(PlayerState.Dating))
                return;
            movement = ctx.ReadValue<Vector2>();
            if (_playerStateController.IsState(PlayerState.Idle))
            {
                _playerStateController.ChangeState(PlayerState.Moving);
            }
        }

        private void Stop(InputAction.CallbackContext ctx)
        {
            if (_playerStateController.IsState(PlayerState.Dating))
                return;
            movement = Vector2.zero;
            if (_playerStateController.IsState(PlayerState.Moving))
            {
                _playerStateController.ChangeState(PlayerState.Idle);
            }
        }

        private void ToggleDate(InputAction.CallbackContext ctx)
        {
            if (!canDate)
                return;
            var start = !_playerStateController.IsState(PlayerState.Dating);
            GameEventSystem.Send(new DateEvent(start));
        }

        private IEnumerator RestartDating(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            canDate = true;
        }
        
        public void OnEvent(AttackEvent @event)
        {
            health -= @event.Damage;
            _sprite.color = Color.Lerp(Color.red, Color.white, Mathf.Max(0, health / 100));
            if (health < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void OnEvent(DateEvent @event)
        {
            if (@event.Start)
            {
                _playerStateController.ChangeState(PlayerState.Dating);
            }
            else
            {
                canDate = false;
                _playerStateController.ChangeState(PlayerState.Idle);
                StartCoroutine(RestartDating(GameController.Instance.datingRestartTimeInS));
            }
        }
    }
}