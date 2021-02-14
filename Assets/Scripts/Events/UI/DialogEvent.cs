namespace Events.UI
{
    public class DialogEvent : IEvent
    {
        public string Text { get; }
        public bool Confirmational { get; }

        public DialogEvent(string text, bool confirmational = true)
        {
            Text = text;
            Confirmational = confirmational;
        }
    }
}