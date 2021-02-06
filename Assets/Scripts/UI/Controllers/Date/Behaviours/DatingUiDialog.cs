namespace UI.Controllers.Date.Behaviours
{
    public class DatingUiDialog : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            context.canMove = false;
        }

        public override void OnTransitionOut()
        {
            Context.canMove = true;
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Dialog;
        }
    }
}