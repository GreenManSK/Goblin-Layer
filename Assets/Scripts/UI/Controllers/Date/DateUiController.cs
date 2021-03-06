using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Constants;
using Controllers;
using Controllers.Goblin;
using Events;
using Events.Date;
using Events.Player;
using Events.UI;
using Objects.Golbin;
using Services;
using UI.Bars;
using UI.Components.Date;
using UI.Components.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Controllers.Date
{
    [RequireComponent(typeof(DateUiStateController))]
    public class DateUiController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(DialogEvent),
            typeof(DialogConfirmationEvent),
            typeof(PresentSelectEvent),
            typeof(AbilityChangeEvent)
        }.AsReadOnly();

        private static readonly Vector2 PrevVector = new Vector2(-1, 1);
        private static readonly Vector2 NextVector = new Vector2(1, -1);

        public List<AUIBar> bars;
        public GoblinAvatarController avatar;
        public EncounterBarController encounterBar;
        public ActionBarController actionBar;
        public TypeBarController typeBar;
        public GameObject presentsUi;
        public GameObject compendiumUi;
        public InventoryGridController inventoryGrid;
        public bool canMove = true;

        public GameObject compendiumButton;
        public List<GameObject> talkButtons = new List<GameObject>();
        public List<GameObject> actionButtons = new List<GameObject>();

        private GoblinController _target;
        private DateUiStateController _stateController;
        private List<GoblinController> _goblins;

        private void Awake()
        {
            _stateController = GetComponent<DateUiStateController>();
        }

        private void Start()
        {
            presentsUi.SetActive(false);
            compendiumUi.SetActive(false);
        }

        private void OnEnable()
        {
            _stateController.ChangeState(DateUiState.Base);
            GameEventSystem.Subscribe(ListenEvents, this);
            GameController.Instance.Input.Player.Move.performed += ChangeActive;
            EnableAbilities();
            bars.ForEach(b => b.SetVisibility(false));
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
            GameController.Instance.Input.Player.Move.performed -= ChangeActive;
            bars.ForEach(b => b.SetVisibility(true));
        }

        public void SetData(List<GoblinController> goblins)
        {
            _goblins = goblins;
            encounterBar.SetData(goblins);
        }

        public void SetActions(int available, int maximum)
        {
            actionBar.SetActions(available, maximum);
        }

        public void SetGoblin(GoblinController goblin)
        {
            _target = goblin;
            encounterBar.SetActive(goblin);
            typeBar.SetData(goblin, actionBar.fontSize);
            avatar.data = goblin.data;
            avatar.UpdateDesign();
        }

        private void ChangeActive(InputAction.CallbackContext ctx)
        {
            if (!GameController.PlayerAbilities.changeActive || !canMove || _goblins.Count <= 1)
                return;
            var input = ctx.ReadValue<Vector2>();
            if (input.magnitude < 1)
                return;
            var prevAngle = Vector2.Angle(input, PrevVector);
            var nextAngle = Vector2.Angle(input, NextVector);
            GameEventSystem.Send(new ChangeActiveGoblin(prevAngle > nextAngle));
        }

        public void Talk(bool talk = true)
        {
            _stateController.ChangeState(talk ? DateUiState.Talking : DateUiState.Base);
        }

        public void Seduce(SeductionDataMonoBehaviour obj)
        {
            // TODO: Use player stats
            GameEventSystem.Send(new SeductionEvent(_target, obj.type, Game.BaseSeduction));
            GameEventSystem.Send(new DateActionEvent());
        }

        public void TogglePresents(bool open)
        {
            _stateController.ChangeState(open ? DateUiState.Presents : DateUiState.Base);
        }

        public void ToggleCompendium(bool open)
        {
            _stateController.ChangeState(open ? DateUiState.Compendium : DateUiState.Base);
        }

        private void EnableAbilities()
        {
            compendiumButton.SetActive(GameController.PlayerAbilities.compendium);

            talkButtons[0].GetComponent<Button>().interactable = GameController.PlayerAbilities.compliment;
            talkButtons[1].GetComponent<Button>().interactable = GameController.PlayerAbilities.flirt;
            talkButtons[2].GetComponent<Button>().interactable = GameController.PlayerAbilities.insult;

            actionButtons[0].GetComponent<Button>().interactable = GameController.PlayerAbilities.compliment ||
                                                                    GameController.PlayerAbilities.flirt ||
                                                                    GameController.PlayerAbilities.insult;
            actionButtons[1].GetComponent<Button>().interactable = GameController.PlayerAbilities.present;
            actionButtons[2].GetComponent<Button>().interactable = GameController.PlayerAbilities.ask;
        }

        public void OnEvent(IEvent @event)
        {
            switch (@event)
            {
                case DialogEvent dialogEvent:
                    OnDialogEvent(dialogEvent);
                    break;
                case DialogConfirmationEvent dialogConfirmationEvent:
                    OnDialogConfirmationEvent(dialogConfirmationEvent);
                    break;
                case PresentSelectEvent presentSelectEvent:
                    OnPresentSelectEvent(presentSelectEvent);
                    break;
                case AbilityChangeEvent _:
                    EnableAbilities();
                    break;
            }
        }

        private void OnDialogEvent(DialogEvent @event)
        {
            if (@event.Confirmational)
            {
                _stateController.ChangeState(DateUiState.Dialog);
            }
        }

        private void OnDialogConfirmationEvent(DialogConfirmationEvent @event)
        {
            _stateController.ChangeState(DateUiState.Base);
        }

        private void OnPresentSelectEvent(PresentSelectEvent presentSelectEvent)
        {
            TogglePresents(false);
            GameEventSystem.Send(new PresentEvent(presentSelectEvent.Present, _target));
            GameEventSystem.Send(new DateActionEvent());
        }
    }
}