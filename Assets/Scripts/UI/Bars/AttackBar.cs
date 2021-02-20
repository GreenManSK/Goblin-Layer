using Data;
using Events;
using Events.Game;
using Services;

namespace UI.Bars
{
    public class AttackBar : AUIBar, IEventListener
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
            if (@event is CooldownUpdateEvent cooldown && cooldown.Type == CooldownType.Attack)
            {
                UpdateScale(cooldown.Value);
            }
        }
    }
}