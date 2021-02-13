namespace Events
{
    public class DialogEvent : IEvent
    {
        public delegate void ConfirmationDelegate();

        public string Text { get; }
        public bool Confirmational { get; }

        public DialogEvent(string text, bool confirmational = true)
        {
            Text = text;
            Confirmational = confirmational;
        }
    }
}