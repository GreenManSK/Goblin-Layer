using System;
using System.Collections.Generic;
using Controllers.Date.Behaviours;
using Events;
using Objects.StateMachine;
using UnityEngine;

namespace Controllers.Date
{
    [RequireComponent(typeof(DateController))]
    public class DateStateController : AStateController<DateController, DateState>
    {
        private List<IEvent> _events = new List<IEvent>();
        
        public override void ChangeState(DateState state)
        {
            base.ChangeState(state);
            _events = _events.FindAll(e => !(Behaviour as ADateBehaviour)?.ProcessEvent(e) ?? true);
        }

        public void ProcessEvent(IEvent @event)
        {
            if ((Behaviour as ADateBehaviour)?.ProcessEvent(@event) ?? false)
                return;
            _events.Add(@event);
        }

        protected override DateController GetContext()
        {
            return GetComponent<DateController>();
        }
        
        protected override IBehaviour<DateController, DateState> CreateBehaviour(DateState state)
        {
            return state switch
            {
                DateState.Active => new DateActive(),
                DateState.NonActive => new DateNonActive(),
                DateState.AwaitDialog => new DateAwaitDialog(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}