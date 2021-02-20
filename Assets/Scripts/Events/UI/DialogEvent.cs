namespace Events.UI
{
    public enum DialogColor
    {
        Default,
        Player,
        Goblin,
        Positive,
        Negative
    }
    
    public class DialogEvent : IEvent
    {
        public string Name { get; }
        public string Text { get; }
        public bool Confirmational { get; }
        
        public DialogColor DialogColor { get; }

        public DialogEvent(string name, string text, bool confirmational = true, DialogColor dialogColor = DialogColor.Default)
        {
            Name = name;
            Text = text;
            Confirmational = confirmational;
            DialogColor = dialogColor;
        }
    }
}