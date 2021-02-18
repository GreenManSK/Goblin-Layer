using System;
using Events;
using Events.UI;
using UnityEngine;

namespace Controllers.Date.Behaviours
{
    [Serializable]
    public class DateAwaitDialog : ADateBehaviour
    {
        public override void OnTransitionIn(DateController context)
        {
            base.OnTransitionIn(context);
            // context.started = true;
            Context.dialogBox.SetActive(true);
        }

        public override bool ProcessEvent(IEvent @event)
        {
            if (!(@event is DialogConfirmationEvent)) return false;
            Context.StateController.ChangeState(Context.AvailableActions <= 0 || Context.goblins.Count <= 0 || !Context.started
                ? DateState.NonActive
                : DateState.Active);
            return true;
        }

        public override bool IsState(DateState state)
        {
            return state == DateState.AwaitDialog;
        }
    }
}