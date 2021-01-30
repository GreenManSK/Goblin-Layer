namespace Events
{
    public class DateEvent : IEvent
    {
        public bool Start;

        public DateEvent(bool start = true)
        {
            this.Start = start;
        }
    }
}