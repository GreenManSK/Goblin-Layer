using System;
using Controllers;
using Events;
using Events.Game;
using Events.UI;
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

        private void EnableControls()
        {
            GameController.Instance.Input.Player.Fire.started += Confirm;
            GameController.Instance.Input.Player.Date.started += Confirm;
        }

        private void DisableControls()
        {
            GameController.Instance.Input.Player.Fire.started -= Confirm;
            GameController.Instance.Input.Player.Date.started -= Confirm;
        }

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
                EnableControls();
                GameEventSystem.Send(new StopEvent());
                nextIndicator.SetActive(needConfirmation);
                _needsConfirmation = needConfirmation;
            }
            nextIndicator.SetActive(_needsConfirmation);
        }

        private void Confirm(InputAction.CallbackContext ctx)
        {
            if (gameObject.activeSelf && _needsConfirmation)
            {
                _needsConfirmation = false;
                SetValues(needConfirmation: false);
                GameEventSystem.Send(new DialogConfirmationEvent());
                DisableControls();
                GameEventSystem.Send(new ResumeEvent());
            }
        }
    }
}