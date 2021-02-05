using UnityEngine;

namespace UI.Controllers.Date.Behaviours
{
    public class DateUiBase : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            context.talkButtons.ForEach(b => b.SetActive(false));
            context.actionButtons.ForEach(b => b.SetActive(true));
        }

        public override void OnTransitionOut()
        {
            base.OnTransitionOut();
            Context.actionButtons.ForEach(b => b.SetActive(false));
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Base;
        }
    }
}