using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Controllers;
using Controllers.Weapon;
using Events;
using Events.Game;
using Events.Goblin;
using Events.Input;
using Events.Player;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(AttackEvent),
            typeof(DateEvent),
            typeof(StopEvent),
            typeof(ResumeEvent),
            typeof(DateButtonEvent),
            typeof(AttackButtonEvent)
        }.AsReadOnly();

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;

        public Vector2 movement = Vector2.zero;
        public Vector2 direction = Vector2.zero;
        public float moveSpeed = 1f;
        public float attackWait = 10f;
        public float health = 100f;
        public bool canDate = true;
        public PlayerWeaponController weapon;
        public InventoryController inventory;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sprite;
        private Animator _animator;

        private PlayerStateController _playerStateController;

        private bool _canAttack = true;

        private void Start()
        {
            _playerStateController = GetComponent<PlayerStateController>();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            inventory = GetComponent<InventoryController>();

            _playerStateController.ChangeState(PlayerState.Idle);
        }

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
            GameController.Instance.Input.Player.Move.performed += Move;
            GameController.Instance.Input.Player.Move.canceled += Stop;
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
            GameController.Instance.Input.Player.Move.performed -= Move;
            GameController.Instance.Input.Player.Move.canceled -= Stop;
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

        private void ToggleDate()
        {
            if (!canDate || !GameController.PlayerAbilities.startDate)
                return;
            var start = !_playerStateController.IsState(PlayerState.Dating);
            GameEventSystem.Send(new DateEvent(start));
        }

        private void Attack()
        {
            if (!_canAttack || !CanMove() || _playerStateController.IsState(PlayerState.Attacking) ||
                !GameController.PlayerAbilities.attack)
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

        public void OnEvent(IEvent @event)
        {
            switch (@event)
            {
                case DateEvent dateEvent:
                    OnDateEvent(dateEvent);
                    break;
                case AttackEvent attackEvent:
                    OnAttackEvent(attackEvent);
                    break;
                case StopEvent _:
                    OnStopEvent();
                    break;
                case ResumeEvent _:
                    OnResumeEvent();
                    break;
                case DateButtonEvent _:
                    ToggleDate();
                    break;
                case AttackButtonEvent _:
                    Attack();
                    break;
            }
        }

        private void OnAttackEvent(AttackEvent @event)
        {
            if (@event.Target != gameObject)
                return;
            health -= @event.Damage;
            GameEventSystem.Send(new PlayerHealthChange(health));
            _sprite.color = Color.Lerp(Color.red, Color.white, Mathf.Max(0, health / 100));
            if (health < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void OnDateEvent(DateEvent @event)
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

        private void OnStopEvent()
        {
            if (!_playerStateController.IsState(PlayerState.Stopped))
                _playerStateController.ChangeState(PlayerState.Stopped);
        }

        private void OnResumeEvent()
        {
            if (_playerStateController.IsState(PlayerState.Stopped))
                _playerStateController.ChangeState(_playerStateController.LastState == PlayerState.Moving
                    ? PlayerState.Idle
                    : _playerStateController.LastState);
        }

        private bool CanMove()
        {
            return !_playerStateController.IsState(PlayerState.Dating) &&
                   !_playerStateController.IsState(PlayerState.Stopped);
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