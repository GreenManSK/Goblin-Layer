using UI.Controllers.Date;

namespace Events.Date
{
    public class DateUiStateChangeEvent : IEvent
    {
        public DateUiState State { get; }

        public DateUiStateChangeEvent(DateUiState state)
        {
            this.State = state;
        }
    }
}