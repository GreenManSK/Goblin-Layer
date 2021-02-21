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
        public delegate void OnConfirmDelegate();

        public string Name { get; }
        public string Text { get; }
        public bool Confirmational { get; }
        public DialogColor DialogColor { get; }
        public OnConfirmDelegate OnConfirm { get; }

        public DialogEvent(string name, string text, bool confirmational = true,
            DialogColor dialogColor = DialogColor.Default, OnConfirmDelegate onConfirm = null)
        {
            Name = name;
            Text = text;
            Confirmational = confirmational || onConfirm != null;
            DialogColor = dialogColor;
            OnConfirm = onConfirm;
        }
    }
}