namespace Events.UI
{
    public class DialogEvent : IEvent
    {
        public string Name { get; }
        public string Text { get; }
        public bool Confirmational { get; }

        public DialogEvent(string name, string text, bool confirmational = true)
        {
            Name = name;
            Text = text;
            Confirmational = confirmational;
        }
    }
}