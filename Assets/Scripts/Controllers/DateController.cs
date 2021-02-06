using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using Events;
using Objects.Golbin;
using Services;
using UI.Components.Date;
using UI.Controllers.Date;
using UnityEngine;

namespace Controllers
{
    public class DateController : MonoBehaviour,
        IEventListener<GoblinActivationEvent>,
        IEventListener<GoblinDeathEvent>,
        IEventListener<DateEvent>,
        IEventListener<DateActionEvent>,
        IEventListener<ChangeActiveGoblin>, 
        IEventListener<DialogEvent>, 
        IEventListener<DialogConfirmationEvent>
    {
        private const int MaxActions = 2; // TODO: Take from player stats

        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(GoblinActivationEvent),
            typeof(GoblinDeathEvent),
            typeof(DateEvent),
            typeof(DateActionEvent),
            typeof(ChangeActiveGoblin),
            typeof(DialogEvent),
            typeof(DialogConfirmationEvent)
        }.AsReadOnly();

        public GameObject arrowPrefab;
        public List<GoblinController> goblins = new List<GoblinController>();
        public DateUiController dateUi;

        private bool _isDate = false;
        private GameObject _arrow;
        private int _activeIndex;
        private int _availableActions = 0;
        private DialogBoxController _dialogBox;
        private bool _canEnd = true;

        private void Start()
        {
            dateUi.gameObject.SetActive(false);
            _dialogBox = dateUi.dialogBox;
        }

        private void OnEnable()
        {
            foreach (var listenEvent in ListenEvents)
            {
                GameEventSystem.Subscribe(listenEvent, this);
            }
        }

        private void OnDisable()
        {
            foreach (var listenEvent in ListenEvents)
            {
                GameEventSystem.Unsubscribe(listenEvent, this);
            }
        }

        public void OnEvent(GoblinActivationEvent @event)
        {
            goblins.Add(@event.Object);
        }

        public void OnEvent(GoblinDeathEvent @event)
        {
            var index = goblins.IndexOf(@event.Object);
            if (index < 0)
                return;
            if (index <= _activeIndex)
            {
                _activeIndex = Mathf.Max(0, _activeIndex - 1);
            }

            goblins.RemoveAt(index);
            if (goblins.Count <= 0)
            {
                if (_isDate)
                {
                    GameEventSystem.Send(new DateEvent(false));
                }
            }
            else
            {
                SetActiveGoblin(_activeIndex);
            }
        }

        public void OnEvent(DateEvent @event)
        {
            if (@event.Start)
            {
                StartDate();
            }
            else
            {
                StopDate();
            }
        }

        public void OnEvent(DateActionEvent @event)
        {
            _availableActions--;
            dateUi.SetActions(_availableActions, MaxActions);
            // TODO: this feker not working because dialog is trigered after this
            TryToEnd();
        }

        public void OnEvent(ChangeActiveGoblin @event)
        {
            SetActiveGoblin((goblins.Count + _activeIndex + (@event.Next ? 1 : -1)) % goblins.Count);
        }

        public void OnEvent(DialogEvent @event)
        {
            if (@event.Confirmational)
            {
                _canEnd = false;
            }
        }

        public void OnEvent(DialogConfirmationEvent @event)
        {
            _canEnd = true;
            TryToEnd();
        }

        private void StartDate()
        {
            if (goblins.Count <= 0)
                return;
            _isDate = true;
            _canEnd = true;
            _availableActions = MaxActions;
            dateUi.SetActions(_availableActions, MaxActions);
            goblins.Sort(GoblinSorter);
            _arrow = Instantiate(arrowPrefab, transform);
            dateUi.SetData(goblins);
            dateUi.gameObject.SetActive(true);
            SetActiveGoblin(0);
        }

        private void StopDate()
        {
            Destroy(_arrow);
            _arrow = null;
            dateUi.gameObject.SetActive(false);
            GameController.Instance.SetCameraTarget();
            _isDate = false;
        }

        private void SetActiveGoblin(int index)
        {
            if (index < 0 || index >= goblins.Count || _arrow == null)
                return;
            goblins[_activeIndex].Updated -= UpdateGoblin;
            _activeIndex = index;
            var goblin = goblins[index];
            goblin.Updated += UpdateGoblin;
            _arrow.transform.position = goblins[_activeIndex].transform.position;
            GameController.Instance.SetCameraTarget(goblin.transform);
            GameEventSystem.Send(new DialogEvent(GoblinTypesConfig.GetDefinition(goblin.type).RandomDateStartText()));
            UpdateGoblin();
        }

        private void UpdateGoblin()
        {
            dateUi.SetGoblin(goblins[_activeIndex]);
        }

        private int GoblinSorter(GoblinController a, GoblinController b)
        {
            var aPos = a.gameObject.transform.position;
            var bPos = b.gameObject.transform.position;
            return Mathf.Approximately(aPos.x, bPos.x) ? aPos.y.CompareTo(bPos.y) : aPos.x.CompareTo(bPos.x);
        }

        private void TryToEnd()
        {
            if (_canEnd && _availableActions <= 0)
            {
                GameEventSystem.Send(new DateEvent(false));
            }
        }
    }
}