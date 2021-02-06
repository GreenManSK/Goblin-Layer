using System.Collections;
using Controllers;
using Controllers.Weapon;
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
        public Vector2 direction = Vector2.zero;
        public float moveSpeed = 1f;
        public float attackWait = 10f;
        public float health = 100f;
        public bool canDate = true;
        public PlayerWeaponController weapon;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sprite;
        private Animator _animator;

        private PlayerStateController _playerStateController;
        private PlayerInputController _playerInputController;

        private bool _canAttack = true;

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
            _playerInputController.AddAction(_playerInputController.Actions.Fire, ActionType.Performed, Attack);
        }

        public void FixFlip()
        {
            if (!Mathf.Approximately(movement.x, 0))
            {
                _sprite.flipX = direction.x < 0;
            }
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            if (!CanMove())
                return;
            movement = direction = ctx.ReadValue<Vector2>();
            if (_playerStateController.IsState(PlayerState.Idle))
            {
                _playerStateController.ChangeState(PlayerState.Moving);
            }
        }

        private void Stop(InputAction.CallbackContext ctx)
        {
            if (!CanMove())
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

        private void Attack(InputAction.CallbackContext ctx)
        {
            if (!_canAttack || !CanMove() || _playerStateController.IsState(PlayerState.Attacking))
                return;
            _canAttack = false;
            _playerStateController.ChangeState(PlayerState.Attacking);
            StartCoroutine(RestartAttack(attackWait));
        }

        public void FinishAttack()
        {
            if (!CanMove())
                return;
            _playerStateController.ChangeState(movement != Vector2.zero ? PlayerState.Moving : PlayerState.Idle);
        }
        
        public void OnEvent(AttackEvent @event)
        {
            if (@event.Target != gameObject)
                return;
            health -= @event.Damage;
            _sprite.color = Color.Lerp(Color.red, Color.white, Mathf.Max(0, health / 100));
            if (health < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void OnEvent(DateEvent @event)
        {
            canDate = false;
            if (@event.Start)
            {
                _playerStateController.ChangeState(PlayerState.Dating);
            }
            else
            {
                _playerStateController.ChangeState(PlayerState.Idle);
                StartCoroutine(RestartDating(GameController.Instance.datingRestartTimeInS));
            }
        }

        public bool CanMove()
        {
            return !_playerStateController.IsState(PlayerState.Dating);
        }
        
        private IEnumerator RestartDating(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            canDate = true;
        }

        private IEnumerator RestartAttack(float waitTime)
        {
            GameEventSystem.Send(new AttackBarEvent(waitTime));
            yield return new WaitForSeconds(waitTime);
            _canAttack = true;
        }
    }
}