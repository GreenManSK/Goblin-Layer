using System;
using Objects.StateMachine;
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

        protected override IBehaviour<DateUiController, DateUiState> CreateBehaviour(DateUiState state)
        {
            return state switch
            {
                DateUiState.Base => new DateUiBase(),
                DateUiState.Dialog => new DatingUiDialog(),
                DateUiState.Talking => new DateUiTalking(),
                DateUiState.Presents => new DateUiPresents(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}