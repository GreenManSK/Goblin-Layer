using Events;
using Events.Player;
using Services;

namespace UI.Bars
{
    public class AttackBar : AUIBar, IEventListener
    {
        public float sizeUpdates = 10;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(typeof(AttackBarEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(AttackBarEvent), this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is AttackBarEvent attackBarEvent)
            {
                UpdateScale(0);
                StartCoroutine(UpdateSize(sizeUpdates, attackBarEvent.WaitTimeInS));
            }
        }
    }
}