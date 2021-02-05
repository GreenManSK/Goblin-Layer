namespace Events
{
    public class ChangeActiveGoblin : IEvent
    {
        public bool Next { get; }

        public ChangeActiveGoblin(bool next)
        {
            Next = next;
        }
    }
}