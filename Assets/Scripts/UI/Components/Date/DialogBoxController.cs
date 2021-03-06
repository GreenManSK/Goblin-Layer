using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Events;
using Events.Game;
using Events.Input;
using Events.UI;
using RotaryHeart.Lib.SerializableDictionary;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Date
{
    public class DialogBoxController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ConfirmationInputEvents = new List<Type>
        {
            typeof(AttackButtonEvent),
            typeof(DateButtonEvent)
        }.AsReadOnly();

        public GameObject box;
        public TMP_Text text;
        public GameObject nextIndicator;
        public Image background;
        public int displayLines = 3;
        public DialogColorDictionary colors;

        private bool _controlsEnabled = false;
        private bool _needsConfirmation = false;
        private readonly Queue<DialogEvent> _dialogs = new Queue<DialogEvent>();
        private DialogEvent _lastNonConfirmation = new DialogEvent("", "", false);

        private void Awake()
        {
            GameEventSystem.Subscribe(typeof(DialogEvent), this);
            ComputeFontSize();
        }

        private void Start()
        {
            var transparency = background.color.a;
            foreach (DialogColor color in Enum.GetValues(typeof(DialogColor)))
            {
                var temp = colors[color];
                temp.a = transparency;
                colors[color] = temp;
            }
        }

        private void ComputeFontSize()
        {
            var startedActive = box.activeSelf;
            box.SetActive(true);

            text.enableAutoSizing = true;
            text.text = string.Concat(Enumerable.Repeat("text\n", displayLines));
            text.ForceMeshUpdate();
            text.enableAutoSizing = false;
            nextIndicator.GetComponent<TMP_Text>().fontSize = text.fontSize;

            box.SetActive(startedActive);
        }

        private void OnDestroy()
        {
            GameEventSystem.Unsubscribe(typeof(DialogEvent), this);
        }

        public void SetActive(bool active)
        {
            box.SetActive(active);
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
                    _dialogs.Enqueue(dialogEvent);
                    DisplayDialog(_dialogs.Peek());
                }
                else
                {
                    _lastNonConfirmation = dialogEvent;
                    DisplayDialog(dialogEvent);
                }
            }
            else if (ConfirmationInputEvents.Contains(@event.GetType()))
            {
                Confirm();
            }
        }

        private void DisplayDialog(DialogEvent dialog)
        {
            text.text = $"<b>{dialog.Name.ToUpper()}</b>\n{dialog.Text}";
            background.color = colors[dialog.DialogColor];

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
            var dialog = _dialogs.Dequeue();
            dialog.OnConfirm?.Invoke();
            if (_dialogs.Count > 0)
            {
                DisplayDialog(_dialogs.Peek());
            }
            else
            {
                _needsConfirmation = false;
                DisplayDialog(_lastNonConfirmation);
                DisableControls();
                GameEventSystem.Send(new DialogConfirmationEvent());
                GameEventSystem.Send(new ResumeEvent());
            }
        }
    }

    [System.Serializable]
    public class DialogColorDictionary : SerializableDictionaryBase<DialogColor, Color>
    {
    }
}