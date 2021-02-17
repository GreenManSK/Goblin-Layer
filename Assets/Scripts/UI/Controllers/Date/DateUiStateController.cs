using System;
using Events.Date;
using Objects.StateMachine;
using Services;
using UI.Controllers.Date.Behaviours;
using UnityEngine;

namespace UI.Controllers.Date
{
    [RequireComponent(typeof(DateUiController))]
    public class DateUiStateController : AStateController<DateUiController, DateUiState>
    {
        protected override DateUiController GetContext()
        {
            return GetComponent<DateUiController>();
        }

        public override void ChangeState(DateUiState state)
        {
            base.ChangeState(state);
            GameEventSystem.Send(new DateUiStateChangeEvent(state));
        }

        protected override IBehaviour<DateUiController, DateUiState> CreateBehaviour(DateUiState state)
        {
            return state switch
            {
                DateUiState.Base => new DateUiBase(),
                DateUiState.Dialog => new DateUiDialog(),
                DateUiState.Talking => new DateUiTalking(),
                DateUiState.Presents => new DateUiPresents(),
                DateUiState.Compendium => new DateUiCompendium(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}