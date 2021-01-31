using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Entities;
using Events;
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
    public class GoblinController : MonoBehaviour, IEventListener<DateEvent>
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;
        public Seeker Seeker => _seeker;
        public Transform Target => _player.transform;
        public SpriteRenderer Sprite => _sprite;
        public HashSet<GameObject> goblinsNear = new HashSet<GameObject>();

        public Goblin data;
        
        public float moveSpeed = 1f;
        public float nextWaypointDistance = 3f;
        public float attackSpeedInS = 5f;
        public float pathUpdateTimeInS = 0.5f;
        public float nearUpdateTimeInS = 0.5f;
        public float lastAttack = 0; 

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

            data = GoblinGenerator.Generate();
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(this);
            if (!_goblinStateController.IsState(GoblinState.Idle))
            {
                GameEventSystem.Send(new GoblinDeathEvent(this));
            }
        }

        public void Activate(GameObject player)
        {
            _player = player;
            GameEventSystem.Send(new GoblinActivationEvent(this));
            _goblinStateController.ChangeState(GoblinState.Chasing);
            
            GameEventSystem.Subscribe(this);
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

        public void OnEvent(DateEvent @event)
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