using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerControlls.PlayerActions Actions => _inupt.Player;

        private PlayerControlls _inupt;

        private Dictionary<(InputAction, ActionType), Action<InputAction.CallbackContext>> _actions =
            new Dictionary<(InputAction, ActionType), Action<InputAction.CallbackContext>>();

        private void Awake()
        {
            Debug.Log("created");
            _inupt = new PlayerControlls();
        }

        private void OnDestroy()
        {
            Debug.Log("deleted");
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
            _inupt.Player.Enable();
        }

        private void OnDisable()
        {
            _inupt.Player.Disable();
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