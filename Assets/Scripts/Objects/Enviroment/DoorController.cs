using System;
using Constants;
using Events;
using Events.UI;
using Services;
using UnityEngine;

namespace Objects.Enviroment
{
    public enum DoorState
    {
        Open,
        Close,
        NeedKey
    }

    [RequireComponent(typeof(Animator))]
    public class DoorController : MonoBehaviour
    {
        private static readonly int OpeningAnimation = Animator.StringToHash("Opening");
        private static readonly int OpenAnimation = Animator.StringToHash("Open");
        
        public DoorState state = DoorState.NeedKey;
        public Collider2D collider;

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                OnPlayerCollision();
            }
        }

        private void OnPlayerCollision()
        {
            switch (state)
            {
                case DoorState.Open:
                    break;
                case DoorState.Close:
                    ChangeState(DoorState.Open);
                    break;
                case DoorState.NeedKey:
                    GameEventSystem.Send(new DialogEvent("You","I need to find a key."));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnOpen()
        {
            Destroy(collider);
            Destroy(_rigidbody);
            _animator.SetTrigger(OpenAnimation);
        }

        public void ChangeState(DoorState newState)
        {
            if (state == DoorState.Open)
                return;
            if (newState == DoorState.Open)
            {
                _animator.SetTrigger(OpeningAnimation);
            }

            state = newState;
        }
    }
}