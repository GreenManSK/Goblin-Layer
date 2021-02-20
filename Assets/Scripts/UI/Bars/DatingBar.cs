using Data;
using Events;
using Events.Game;
using Services;

namespace UI.Bars
{
    public class DatingBar : AUIBar, IEventListener
    {
        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(CooldownUpdateEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(CooldownUpdateEvent), this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is CooldownUpdateEvent cooldown && cooldown.Type == CooldownType.Date)
            {
                UpdateScale(cooldown.Value);
            }
        }
    }
}