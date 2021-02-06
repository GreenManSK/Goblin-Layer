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
    public class DialogBoxController : MonoBehaviour, IEventListener
    {
        public TMP_Text text;
        public GameObject nextIndicator;

        private bool _needsConfirmation = false;

        private void Awake()
        {
            SetValues("");
            GameEventSystem.Subscribe(typeof(DialogEvent),this);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(typeof(DialogEvent),this);
        }

        // private void OnEnable()
        // {
        //     GameController.Instance.Input.Player.Fire.started += Confirm;
        //     GameController.Instance.Input.Player.Date.started += Confirm;
        // }
        //
        // private void OnDisable()
        // {
        //     GameController.Instance.Input.Player.Fire.started -= Confirm;
        //     GameController.Instance.Input.Player.Date.started -= Confirm;
        // }

        public void OnEvent(IEvent @event)
        {
            if (@event is DialogEvent dialogEvent)
            {
                SetValues(dialogEvent.Text, dialogEvent.Confirmational);
            }
        }

        private void SetValues([CanBeNull] string dialogText = null, bool needConfirmation = false)
        {
            if (dialogText != null)
            {
                text.text = dialogText;
            }

            if (needConfirmation)
            {
                GameController.Instance.Input.Player.Fire.started += Confirm;
                GameController.Instance.Input.Player.Date.started += Confirm;
                nextIndicator.SetActive(needConfirmation);
                _needsConfirmation = needConfirmation;
            }
        }

        private void Confirm(InputAction.CallbackContext ctx)
        {
            if (gameObject.activeSelf && _needsConfirmation)
            {
                SetValues(needConfirmation: false);
                GameEventSystem.Send(new DialogConfirmationEvent());
                GameController.Instance.Input.Player.Fire.started -= Confirm;
                GameController.Instance.Input.Player.Date.started -= Confirm;
            }
        }
    }
}