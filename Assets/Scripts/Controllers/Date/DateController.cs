using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Constants;
using Data;
using Events;
using Events.Game;
using Events.Goblin;
using Events.Input;
using Events.UI;
using Objects.Golbin;
using Services;
using UI.Components.Date;
using UI.Controllers.Date;
using UnityEngine;

namespace Controllers.Date
{
    [RequireComponent(typeof(DateStateController))]
    public class DateController : MonoBehaviour, IEventListener
    {
        private const int MaxActions = Game.MaxActions; // TODO: Take from player stats

        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(GoblinActivationEvent),
            typeof(GoblinDeathEvent),
            typeof(DateActionEvent),
            typeof(ChangeActiveGoblin),
            typeof(DialogEvent),
            typeof(DialogConfirmationEvent),
            typeof(DateButtonEvent),
            typeof(CooldownUpdateEvent)
        }.AsReadOnly();

        public int ActiveIndex => _activeIndex;
        public int AvailableActions => _availableActions;
        public DateStateController StateController => _stateController;
        public GoblinController ActiveGoblin => goblins[_activeIndex];

        public bool canDate = true;
        public bool started = false;
        public GameObject arrowPrefab;
        public List<GoblinController> goblins = new List<GoblinController>();
        public DateUiController dateUi;
        public DialogBoxController dialogBox;

        private GameObject _arrow;
        private int _activeIndex;
        private int _availableActions = 0;

        private DateStateController _stateController;

        private void Awake()
        {
            FindDependencies();
        }

        private void FindDependencies()
        {
            if (dateUi == null)
            {
                dateUi = FindObjectOfType<DateUiController>();
            }

            if (dialogBox == null)
            {
                dialogBox = FindObjectOfType<DialogBoxController>();
            }
        }

        private void Start()
        {
            _stateController = GetComponent<DateStateController>();
            _stateController.ChangeState(DateState.NonActive);
        }

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
        }

        public void OnEvent(IEvent @event)
        {
            switch (@event)
            {
                case DialogEvent dialog:
                    OnDialogEvent(dialog);
                    break;
                case DateActionEvent dateAction:
                    OnDateActionEvent(dateAction);
                    break;
                case CooldownUpdateEvent cooldownUpdateEvent:
                    OnCooldownUpdateEvent(cooldownUpdateEvent);
                    break;
                default:
                    _stateController.ProcessEvent(@event);
                    break;
            }
        }

        private void OnDialogEvent(DialogEvent @event)
        {
            if (@event.Confirmational)
            {
                StateController.ChangeState(DateState.AwaitDialog);
            }
        }

        private void OnDateActionEvent(DateActionEvent @event)
        {
            _availableActions--;
            dateUi.SetActions(_availableActions, MaxActions);
        }

        private void OnCooldownUpdateEvent(CooldownUpdateEvent @event)
        {
            if (@event.Type == CooldownType.Date && @event.Charged)
            {
                canDate = true;
            }
        }

        public void StartDate()
        {
            if (started)
                return;
            canDate = false;
            started = true;
            _availableActions = MaxActions;
            dateUi.SetActions(_availableActions, MaxActions);
            goblins.Sort(GoblinSorter);
            _arrow = Instantiate(arrowPrefab, transform);
            dateUi.SetData(goblins);
            dateUi.gameObject.SetActive(true);
            dialogBox.SetActive(true);
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

            dialogBox.SetActive(false);
            dateUi.gameObject.SetActive(false);
            GameController.Instance.SetCameraTarget();
            EnableDating();
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
            GameEventSystem.Send(new DialogEvent("Goblin",
                GoblinTypesConfig.GetDefinition(goblin.type).RandomDateStartText(), false, DialogColor.Goblin));
            UpdateGoblin();
        }

        private void UpdateGoblin()
        {
            if (dateUi.gameObject.activeSelf)
            {
                dateUi.SetGoblin(goblins[_activeIndex]);
            }
        }

        private int GoblinSorter(GoblinController a, GoblinController b)
        {
            var aPos = a.gameObject.transform.position;
            var bPos = b.gameObject.transform.position;
            return Mathf.Approximately(aPos.x, bPos.x) ? aPos.y.CompareTo(bPos.y) : aPos.x.CompareTo(bPos.x);
        }

        public void EnableDating()
        {
            if (canDate)
                return;
            GameEventSystem.Send(
                new CooldownResetEvent(CooldownType.Date, GameController.Instance.datingRestartTimeInS));
        }
    }
}