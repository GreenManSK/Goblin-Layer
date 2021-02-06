namespace Events
{
    public interface IEventListener
    {
        void OnEvent(IEvent @event);
    }
}