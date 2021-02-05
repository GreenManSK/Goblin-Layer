namespace UI.Controllers.Date.Behaviours
{
    public class DateUiTalking : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            Context.talkButtons.ForEach(b => b.SetActive(true));
        }

        public override void OnTransitionOut()
        {
            base.OnTransitionOut();
            Context.talkButtons.ForEach(b => b.SetActive(false));
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Talking;
        }
    }
}