using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Player
{
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public Vector2 movement = Vector2.zero;
        public float moveSpeed = 1f;

        private Rigidbody2D _rigidbody2D;

        private PlayerStateController _playerStateController;
        private PlayerInputController _playerInputController;

        private void Start()
        {
            _playerStateController = GetComponent<PlayerStateController>();
            _playerInputController = GetComponent<PlayerInputController>();

            _rigidbody2D = GetComponent<Rigidbody2D>();

            RegisterInputs();
            _playerStateController.ChangeState(PlayerState.Idle);
        }

        private void RegisterInputs()
        {
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Performed, Move);
            _playerInputController.AddAction(_playerInputController.Actions.Move, ActionType.Canceled, Stop);
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
    }
}