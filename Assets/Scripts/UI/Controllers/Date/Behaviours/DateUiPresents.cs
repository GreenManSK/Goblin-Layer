using Controllers;

namespace UI.Controllers.Date.Behaviours
{
    public class DateUiPresents : ADateUiBehaviour
    {
        public override void OnTransitionIn(DateUiController context)
        {
            base.OnTransitionIn(context);
            Context.presentsUi.SetActive(true);
            Context.inventoryGrid.SetItems(GameController.Instance.player.inventory.items);
        }

        public override void OnTransitionOut()
        {
            Context.presentsUi.SetActive(false);
        }

        public override bool IsState(DateUiState state)
        {
            return state == DateUiState.Presents;
        }
    }
}