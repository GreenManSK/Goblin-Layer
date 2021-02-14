using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Controllers;
using Events;
using Events.Game;
using Events.Input;
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
        private static readonly ReadOnlyCollection<Type> ConfirmationInputEvents = new List<Type>
        {
            typeof(AttackButtonEvent),
            typeof(DateButtonEvent)
        }.AsReadOnly();

        public TMP_Text text;
        public GameObject nextIndicator;

        private bool _needsConfirmation = false;

        private void Awake()
        {
            SetValues("");
            GameEventSystem.Subscribe(typeof(DialogEvent), this);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(typeof(DialogEvent), this);
        }

        private void EnableControls()
        {
            GameEventSystem.Subscribe(ConfirmationInputEvents, this);
        }

        private void DisableControls()
        {
            GameEventSystem.Subscribe(ConfirmationInputEvents, this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is DialogEvent dialogEvent)
            {
                SetValues(dialogEvent.Text, dialogEvent.Confirmational);
            } else if (ConfirmationInputEvents.Contains(@event.GetType()))
            {
                Confirm();
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

        private void Confirm()
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