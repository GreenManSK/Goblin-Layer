using Constants;
using Events;
using Events.Player;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Bars
{
    public class HealthBar : AUIBar, IEventListener
    {
        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(PlayerHealthChange), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(PlayerHealthChange), this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is PlayerHealthChange healthEvent)
            {
                UpdateScale(healthEvent.Health / Game.MaxPlayerHealth);
            }
        }
    }
}