namespace UI.Controllers.Date.Behaviours
{
    public class DatingUiDialog : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            context.CanMove = false;
        }

        public override void OnTransitionOut()
        {
            Context.CanMove = true;
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Dialog;
        }
    }
}