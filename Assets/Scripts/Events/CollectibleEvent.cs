using Entities;

namespace Events
{
    public class CollectibleEvent : IEvent
    {
        public Collectible Collectible { get; }

        public CollectibleEvent(Collectible collectible)
        {
            Collectible = collectible;
        }
    }
}