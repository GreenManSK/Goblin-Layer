using System;
using System.Collections.Generic;
using Constants;
using Controllers;
using Controllers.Goblin;
using Events;
using Objects.Golbin;
using Services;
using UI.Components.Date;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Controllers.Date
{
    [RequireComponent(typeof(DateUiStateController))]
    public class DateUiController : MonoBehaviour, IEventListener<DialogEvent>, IEventListener<DialogConfirmationEvent>
    {
        private static readonly Vector2 PrevVector = new Vector2(-1, 1);
        private static readonly Vector2 NextVector = new Vector2(1, -1);
        
        public GoblinAvatarController avatar;
        public EncounterBarController encounterBar;
        public ActionBarController actionBar;
        public TypeBarController typeBar;
        public DialogBoxController dialogBox;
        public bool CanMove = true;

        public List<GameObject> talkButtons = new List<GameObject>();
        public List<GameObject> actionButtons = new List<GameObject>();

        private GoblinController _target;
        private DateUiStateController _stateController;

        private void Awake()
        {
            _stateController = GetComponent<DateUiStateController>();
        }

        private void OnEnable()
        {
            _stateController.ChangeState(DateUiState.Base);
            GameEventSystem.Subscribe<DialogEvent>(this);
            GameEventSystem.Subscribe<DialogConfirmationEvent>(this);
            GameController.Instance.Input.Player.Move.performed += ChangeActive;
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe<DialogEvent>(this);
            GameEventSystem.Unsubscribe<DialogConfirmationEvent>(this);
            GameController.Instance.Input.Player.Move.performed -= ChangeActive;
        }

        public void SetData(List<GoblinController> goblins)
        {
            encounterBar.SetData(goblins);
        }

        public void SetActions(int available, int maximum)
        {
            actionBar.SetActions(available, maximum);
        }

        public void SetGoblin(GoblinController goblin)
        {
            // TODO: Disable when dialog
            _target = goblin;
            encounterBar.SetActive(goblin);
            typeBar.SetData(goblin);
            avatar.data = goblin.data;
            avatar.UpdateDesign();
        }

        private void ChangeActive(InputAction.CallbackContext ctx)
        {
            if (!CanMove)
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

        public void OpenCompendium()
        {
            // TODO: Open
            Debug.Log("Opening compendium");
        }

        public void OnEvent(DialogEvent @event)
        {
            if (@event.Confirmational)
            {
                _stateController.ChangeState(DateUiState.Dialog);
            }
        }

        public void OnEvent(DialogConfirmationEvent @event)
        {
            _stateController.ChangeState(DateUiState.Base);
        }
    }
}