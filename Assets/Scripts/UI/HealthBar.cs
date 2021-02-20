using Constants;
using Events;
using Events.Player;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour, IEventListener
    {
        public RectTransform bar;

        public GameObject icon;
        public Image barSprite;
        
        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(PlayerHealthChange), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(PlayerHealthChange), this);
        }

        public void SetVisibility(bool visible)
        {
            var color = barSprite.color;
            color.a = visible ? 1 : 0;
            barSprite.color = color;
            icon.SetActive(visible);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is PlayerHealthChange healthEvent)
            {
                UpdateScale(healthEvent.Health / Game.MaxPlayerHealth);
            }
        }

        private void UpdateScale(float y)
        {
            var localScale = bar.localScale;
            bar.localScale = new Vector3(localScale.x, y, localScale.z);
        }
    }
}