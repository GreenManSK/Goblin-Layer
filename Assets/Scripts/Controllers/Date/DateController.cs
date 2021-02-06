using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Constants;
using Data;
using Events;
using Objects.Golbin;
using Services;
using UI.Components.Date;
using UI.Controllers.Date;
using UnityEngine;

namespace Controllers.Date
{
    [RequireComponent(typeof(DateStateController))]
    public class DateController : MonoBehaviour,
        IEventListener<GoblinActivationEvent>,
        IEventListener<GoblinDeathEvent>,
        IEventListener<DateEvent>,
        IEventListener<DateActionEvent>,
        IEventListener<ChangeActiveGoblin>,
        IEventListener<DialogEvent>,
        IEventListener<DialogConfirmationEvent>
    {
        private const int MaxActions = Game.MaxActions; // TODO: Take from player stats

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

        public int ActiveIndex => _activeIndex;
        public int AvailableActions => _availableActions;
        public DateStateController StateController => _stateController;

        public bool started = false;
        public GameObject arrowPrefab;
        public List<GoblinController> goblins = new List<GoblinController>();
        public DateUiController dateUi;
        public DialogBoxController dialogBox;

        private GameObject _arrow;
        private int _activeIndex;
        private int _availableActions = 0;

        private DateStateController _stateController;

        private void Start()
        {
            _stateController = GetComponent<DateStateController>();
            _stateController.ChangeState(DateState.NonActive);
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
            _stateController.ProcessEvent(@event);
        }

        public void OnEvent(GoblinDeathEvent @event)
        {
            _stateController.ProcessEvent(@event);
        }

        public void OnEvent(DateEvent @event)
        {
            _stateController.ProcessEvent(@event);
        }

        public void OnEvent(ChangeActiveGoblin @event)
        {
            _stateController.ProcessEvent(@event);
        }

        public void OnEvent(DialogEvent @event)
        {
            if (@event.Confirmational)
            {
                StateController.ChangeState(DateState.AwaitDialog);
            }
        }

        public void OnEvent(DialogConfirmationEvent @event)
        {
            _stateController.ProcessEvent(@event);
        }

        public void OnEvent(DateActionEvent @event)
        {
            _availableActions--;
            dateUi.SetActions(_availableActions, MaxActions);
        }

        public void StartDate()
        {
            if (started)
                return;
            started = true;
            _availableActions = MaxActions;
            dateUi.SetActions(_availableActions, MaxActions);
            goblins.Sort(GoblinSorter);
            _arrow = Instantiate(arrowPrefab, transform);
            dateUi.SetData(goblins);
            dateUi.gameObject.SetActive(true);
            dialogBox.gameObject.SetActive(true);
            SetActiveGoblin(0);
        }

        public void StopDate()
        {
            started = false;
            if (_arrow != null)
            {
                Destroy(_arrow);
                _arrow = null;
            }

            dialogBox.gameObject.SetActive(false);
            dateUi.gameObject.SetActive(false);
            GameController.Instance.SetCameraTarget();
        }

        public void SetActiveGoblin(int index)
        {
            if (index < 0 || index >= goblins.Count || _arrow == null)
                return;
            if (_activeIndex < goblins.Count)
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
    }
}