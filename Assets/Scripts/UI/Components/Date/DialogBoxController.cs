using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private bool _controlsEnabled = false;
        private bool _needsConfirmation = false;
        private Queue<DialogEvent> dialogs = new Queue<DialogEvent>();

        private void Awake()
        {
            GameEventSystem.Subscribe(typeof(DialogEvent), this);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(typeof(DialogEvent), this);
        }

        private void EnableControls()
        {
            if (_controlsEnabled)
                return;
            _controlsEnabled = true;
            GameEventSystem.Subscribe(ConfirmationInputEvents, this);
        }

        private void DisableControls()
        {
            GameEventSystem.Unsubscribe(ConfirmationInputEvents, this);
            _controlsEnabled = false;
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is DialogEvent dialogEvent)
            {
                if (dialogEvent.Confirmational)
                {
                    dialogs.Enqueue(dialogEvent);
                    DisplayDialog(dialogs.Peek());
                }
                else
                {
                    DisplayDialog(dialogEvent);
                }
            } else if (ConfirmationInputEvents.Contains(@event.GetType()))
            {
                Confirm();
            }
        }

        private void DisplayDialog(DialogEvent dialog)
        {
            text.text = $"<b>{dialog.Name.ToUpper()}</b>\n{dialog.Text}";

            if (dialog.Confirmational)
            {
                EnableControls();
                GameEventSystem.Send(new StopEvent());
                _needsConfirmation = true;
            }
            nextIndicator.SetActive(_needsConfirmation);
        }

        private void Confirm()
        {
            if (!_needsConfirmation)
                return;
            dialogs.Dequeue();
            if (dialogs.Count > 0)
            {
                DisplayDialog(dialogs.Peek());
            }
            else
            {
                _needsConfirmation = false;
                DisableControls();
                GameEventSystem.Send(new DialogConfirmationEvent());
                GameEventSystem.Send(new ResumeEvent());
            }
        }
    }
}