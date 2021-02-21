using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Constants;
using Controllers;
using Controllers.Weapon;
using Data;
using Entities;
using Entities.Types;
using Events;
using Events.Date;
using Events.Game;
using Events.Goblin;
using Events.UI;
using Pathfinding;
using Services;
using UnityEngine;

namespace Objects.Golbin
{
    [RequireComponent(typeof(GoblinStateController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Seeker))]
    public class GoblinController : MonoBehaviour, IEventListener
    {
        public delegate void UpdatedEvent();

        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(DateEvent),
            typeof(SeductionEvent),
            typeof(PresentEvent),
            typeof(StopEvent),
            typeof(ResumeEvent)
        }.AsReadOnly();

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;
        public Seeker Seeker => _seeker;
        public GameObject Player => _player;
        public SpriteRenderer Sprite => _sprite;
        public GoblinStateController StateController => _goblinStateController;
        public HashSet<GameObject> goblinsNear = new HashSet<GameObject>();
        public event UpdatedEvent Updated;

        public Goblin data;
        public GoblinType type;
        public float seduction = 0;

        public float moveSpeed = 1f;
        public float nextWaypointDistance = 3f;
        public float attackReach = 1f;
        public float attackSpeedInS = 5f;
        public float pathUpdateTimeInS = 0.5f;
        public float nearUpdateTimeInS = 0.5f;
        public float lastAttack = 0;
        public GoblinWeaponController weapon;
        public LastSeduction lastSeduction = new LastSeduction();

        public GameObject HearthsPrefab;
        public GameObject BoltsPrefab;

        public SpriteRenderer blush;
        public SpriteRenderer angryMark;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sprite;
        private Animator _animator;
        private Seeker _seeker;

        private GameObject _player;
        private Dictionary<GameObject, IEnumerator> _nearRemovingCoroutines = new Dictionary<GameObject, IEnumerator>();

        private GoblinStateController _goblinStateController;

        private void Start()
        {
            _goblinStateController = GetComponent<GoblinStateController>();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _seeker = GetComponent<Seeker>();

            _goblinStateController.ChangeState(GoblinState.Idle);

            var typeDefinition = GoblinTypesConfig.GetDefinition(type);
            var hide = data.hide;
            data = GoblinGenerator.Generate(
                blushes: GoblinGenerator.NoBlushes,
                glasses: typeDefinition.glasseses,
                accessories: typeDefinition.accessories,
                costumes: typeDefinition.costumes,
                hairstyles: typeDefinition.hairs,
                expressions: typeDefinition.expressions
            );
            data.hide = hide;
            data.expression = GoblinTypesConfig.GetSeductionExpression(seduction, data.expression);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
            if (_goblinStateController && !_goblinStateController.IsState(GoblinState.Idle))
            {
                GameEventSystem.Send(new GoblinDeathEvent(this));
            }
        }

        public void Activate(GameObject player)
        {
            _player = player;
            GameEventSystem.Send(new GoblinActivationEvent(this));
            _goblinStateController.ChangeState(GoblinState.Chasing);

            GameEventSystem.Subscribe(ListenEvents, this);
        }

        public bool CanAttack()
        {
            // TODO: do not use timestamp difference
            return (Time.time - lastAttack) > attackSpeedInS;
        }

        public void Attack()
        {
            _goblinStateController.ChangeState(GoblinState.Attacking);
        }

        public void Chase()
        {
            _goblinStateController.ChangeState(GoblinState.Chasing);
        }

        public void OnEvent(IEvent @event)
        {
            switch (@event)
            {
                case DateEvent dateEvent:
                    OnDateEvent(dateEvent);
                    break;
                case SeductionEvent seductionEvent:
                    OnSeductionEvent(seductionEvent);
                    break;
                case PresentEvent presentEvent:
                    OnPresentEvent(presentEvent);
                    break;
                case StopEvent _:
                    OnStopEvent();
                    break;
                case ResumeEvent _:
                    OnResumeEvent();
                    break;
            }
        }

        private void OnDateEvent(DateEvent @event)
        {
            if (@event.Start)
            {
                _goblinStateController.ChangeState(GoblinState.Dating);
            }
            else
            {
                _goblinStateController.ChangeState(GoblinState.Chasing);
            }
        }

        private void OnPresentEvent(PresentEvent @event)
        {
            OnSeductionEvent(new SeductionEvent(this, SeductionType.Present, @event.Present.strength));
        }

        private void OnSeductionEvent(SeductionEvent @event)
        {
            if (!GameController.Mechanics.seduction)
                return;
            var change = 0f;
            var sendDialog = false;
            if (@event.Target == this)
            {
                lastSeduction.Update(@event.Type);
                change += (lastSeduction.count <= 1 ? @event.Strength : 0) *
                          GoblinTypesConfig.GetMultiplier(type, @event.Type) *
                          (lastSeduction.count == 0 ? 1 : 0.5f);
                if (lastSeduction.count >= 2)
                {
                    change -= @event.Strength;
                }

                if (lastSeduction.count == 0 && GoblinTypesConfig.IsPositiveSeduction(type, @event.Type) &&
                    @event.Type != SeductionType.Present)
                {
                    change += @event.Strength * GoblinTypesConfig.GetMultiplier(type, SeductionType.BeforeOthers);
                }

                if (@event.ByPlayer && @event.Type != SeductionType.Attack)
                {
                    sendDialog = true;
                }
            }
            else if (@event.ByPlayer && GoblinTypesConfig.IsPositiveSeduction(type, @event.Type))
            {
                change += @event.Strength * GoblinTypesConfig.GetMultiplier(type, SeductionType.SeeOthers);
            }

            UpdateSeduction(change);
            if (sendDialog)
            {
                SendDialogReaction(change);
            }
        }

        private void OnStopEvent()
        {
            if (!_goblinStateController.IsState(GoblinState.Stopped))
                _goblinStateController.ChangeState(GoblinState.Stopped);
        }

        private void OnResumeEvent()
        {
            if (_goblinStateController.IsState(GoblinState.Stopped))
                _goblinStateController.ChangeState(_goblinStateController.LastState);
        }

        private void UpdateSeduction(float change)
        {
            seduction += change;
            data.blush = GetBlush();
            UpdateMiniEffect();
            if (change > 0)
            {
                Instantiate(HearthsPrefab, transform);
            }
            else if (!Mathf.Approximately(change, 0))
            {
                Instantiate(BoltsPrefab, transform);
            }

            data.expression = GoblinTypesConfig.GetSeductionExpression(seduction, data.expression);
            GameEventSystem.Send(new SeductionChangeEvent(this, change));
            Updated?.Invoke();
            if (seduction >= 100)
            {
                Destroy(gameObject);
            }
        }

        private void SendDialogReaction(float change)
        {
            var typeDefinition = GoblinTypesConfig.GetDefinition(type);
            string reactionText;
            var dialogColor = DialogColor.Default;
            var reaction = SeductionReaction.Neutral;
            if (Mathf.Approximately(change, 0))
            {
                reactionText = typeDefinition.RandomNeutralReactionText();
            }
            else if (change < 0)
            {
                reactionText = typeDefinition.RandomNegativeReactionText();
                dialogColor = DialogColor.Negative;
                reaction = SeductionReaction.Negative;
            }
            else
            {
                reactionText = typeDefinition.RandomPositiveReactionText();
                dialogColor = DialogColor.Positive;
                reaction = SeductionReaction.Positive;
            }

            if (lastSeduction.count > 0)
            {
                reactionText = lastSeduction.count switch
                {
                    1 => "Can't you come up with something new?",
                    2 => "It's getting boring.",
                    _ => "Again? Who do you think I am?!"
                };
            }

            var oldExpression = data.expression;
            data.expression = lastSeduction.count == 0 || lastSeduction.count > 2
                ? GoblinTypesConfig.GetReactionExpression(reaction)
                : Expression.Sleepy;

            Updated?.Invoke();
            GameEventSystem.Send(new DialogEvent("Goblin", reactionText, true, dialogColor,
                () =>
                {
                    data.expression = oldExpression;
                    Updated?.Invoke();
                }));
        }

        private Blush GetBlush()
        {
            if (seduction > 66)
            {
                return Blush.Strong;
            }

            if (seduction > 33)
            {
                return Blush.Weak;
            }

            return Blush.None;
        }

        private void UpdateMiniEffect()
        {
            blush.gameObject.SetActive(false);
            angryMark.gameObject.SetActive(false);
            if (seduction > 33)
            {
                blush.gameObject.SetActive(true);
                blush.color = Helpers.ChangeAlpha(blush.color, seduction > 66 ? 0.8f : 0.6f);
            } else if (seduction < 0)
            {
                angryMark.gameObject.SetActive(transform);
                angryMark.color = Helpers.ChangeAlpha(blush.color, seduction < 40 ? 0.8f : 0.6f);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Goblin))
            {
                if (_nearRemovingCoroutines.ContainsKey(other.gameObject))
                {
                    StopCoroutine(_nearRemovingCoroutines[other.gameObject]);
                    _nearRemovingCoroutines.Remove(other.gameObject);
                }

                goblinsNear.Add(other.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Goblin))
            {
                var coroutine = RemoveNear(other.gameObject, nearUpdateTimeInS);
                _nearRemovingCoroutines.Add(other.gameObject, coroutine);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator RemoveNear(GameObject near, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            goblinsNear.Remove(near);
        }
    }
}