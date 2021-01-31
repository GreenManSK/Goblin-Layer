using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerControlls.PlayerActions Actions
        {
            get
            {
                _action ??= GameController.Instance.Input.Player;
                return _action.Value;
            }
        }

        private PlayerControlls.PlayerActions? _action = null;

        private Dictionary<(InputAction, ActionType), Action<InputAction.CallbackContext>> _actions =
            new Dictionary<(InputAction, ActionType), Action<InputAction.CallbackContext>>();

        private void OnDestroy()
        {
            foreach (var action in _actions)
            {
                var inputAction = action.Key.Item1;
                switch (action.Key.Item2)
                {
                    case ActionType.Performed:
                        inputAction.performed -= action.Value;
                        break;
                    case ActionType.Canceled:
                        inputAction.canceled -= action.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void OnEnable()
        {
            Actions.Enable();
        }

        private void OnDisable()
        {
            Actions.Disable();
        }

        public void AddAction(InputAction inputAction, ActionType type, Action<InputAction.CallbackContext> callback)
        {
            _actions.Add((inputAction, type), callback);
            switch (type)
            {
                case ActionType.Performed:
                    inputAction.performed += callback;
                    break;
                case ActionType.Canceled:
                    inputAction.canceled += callback;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ActionType
    {
        Performed,
        Canceled
    }
}