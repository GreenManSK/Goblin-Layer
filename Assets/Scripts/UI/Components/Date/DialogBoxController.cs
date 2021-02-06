using System;
using Controllers;
using Events;
using JetBrains.Annotations;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Components.Date
{
    public class DialogBoxController : MonoBehaviour, IEventListener<DialogEvent>
    {
        public TMP_Text text;
        public GameObject nextIndicator;

        private bool _needsConfirmation = false;

        private void Awake()
        {
            SetValues("");
            GameEventSystem.Subscribe(this);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(this);
        }

        private void OnEnable()
        {
            GameController.Instance.Input.Player.Fire.performed += Confirm;
        }

        private void OnDisable()
        {
            GameController.Instance.Input.Player.Fire.performed += Confirm;
        }

        public void OnEvent(DialogEvent @event)
        {
            SetValues(@event.Text, @event.Confirmational);
        }

        private void SetValues([CanBeNull] string dialogText = null, bool needConfirmation = false)
        {
            nextIndicator.SetActive(needConfirmation);
            if (dialogText != null)
            {
                text.text = dialogText;
            }

            _needsConfirmation = needConfirmation;
        }

        private void Confirm(InputAction.CallbackContext ctx)
        {
            if (_needsConfirmation)
            {
                SetValues(needConfirmation: false);
                GameEventSystem.Send(new DialogConfirmationEvent());
            }
        }
    }
}