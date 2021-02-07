namespace UI.Controllers.Date.Behaviours
{
    public class DateUiCompendium : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            Context.compendiumUi.SetActive(true);
        }

        public override void OnTransitionOut()
        {
            Context.compendiumUi.SetActive(false);
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Compendium;
        }
    }
}