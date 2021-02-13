using System;
using UnityEngine;

namespace Objects.Enviroment
{
    public enum SpikesState
    {
        Up,
        Down
    }

    [RequireComponent(typeof(Animator))]
    public class SpikesController : MonoBehaviour
    {
        private static readonly int UpAnimation = Animator.StringToHash("GoUp");
        private static readonly int DownAnimation = Animator.StringToHash("GoDown");

        public SpikesState state = SpikesState.Down;
        public Collider2D collider;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (state != SpikesState.Down)
                ChangeState(state);
        }

        public void ChangeState(SpikesState spikesState)
        {
            state = spikesState;
            Debug.Log("test" + state);
            _animator.SetTrigger(spikesState == SpikesState.Down ? DownAnimation : UpAnimation);
        }

        public void OnAnimationEnd()
        {
            collider.enabled = state == SpikesState.Up;
        }
    }
}