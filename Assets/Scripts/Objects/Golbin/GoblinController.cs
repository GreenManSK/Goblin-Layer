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
    public class GoblinController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;
        public Seeker Seeker => _seeker;
        public Transform Target => _player.transform;
        public SpriteRenderer Sprite => _sprite;

        public float moveSpeed = 1f;
        public float nextWaypointDistance = 3f;
        public float pathUpdateTimeInS = 0.5f;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sprite;
        private Animator _animator;
        private Seeker _seeker;

        private GameObject _player;

        private GoblinStateController _goblinStateController;
        
        private void Start()
        {
            _goblinStateController = GetComponent<GoblinStateController>();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _seeker = GetComponent<Seeker>();
            
            _goblinStateController.ChangeState(GoblinState.Idle);
        }

        public void Activate(GameObject player)
        {
            // TODO: Register for stuff
            _player = player;
            GameEventSystem.Send(new GoblinActivationEvent(gameObject));
            _goblinStateController.ChangeState(GoblinState.Chasing);
        }
    }
}