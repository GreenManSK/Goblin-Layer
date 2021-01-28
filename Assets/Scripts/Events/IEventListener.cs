namespace Events
{
    public interface IEventListener
    {
        
    }
    
    public interface IEventListener<in T> : IEventListener where T: IEvent
    {
        void OnEvent(T @event);
    }
}