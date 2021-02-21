using Data;
using Events;
using Events.Goblin;
using Objects.Golbin;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Date
{
    public class EncounterBarItemController : MonoBehaviour, IEventListener
    {
        public static readonly int GoblinAnimation = Animator.StringToHash("Goblin");
        public static readonly int HearthsAnimation = Animator.StringToHash("Hearths");
        public static readonly int BoltsAnimation = Animator.StringToHash("Bolts");

        public GoblinController goblin;
        public GameObject activeBg;
        public Animator animator;
        public Image typeIndicator;

        public Image blush;
        public Image angryMark;

        private void OnEnable()
        {
            animator.SetTrigger(GoblinAnimation);
            GameEventSystem.Subscribe(typeof(SeductionChangeEvent), this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(typeof(SeductionChangeEvent), this);
        }

        public void SetData(GoblinController data)
        {
            goblin = data;
            var type = GoblinTypesConfig.GetDefinition(goblin.type);
            typeIndicator.color = type.color;
            SetGoblinEffects();
        }

        public void SetActive(bool active)
        {
            activeBg.SetActive(active);
        }

        public void OnEffectEnd()
        {
            animator.SetTrigger(GoblinAnimation);
            SetGoblinEffects();
        }

        private void SetGoblinEffects()
        {
            blush.gameObject.SetActive(goblin.blush.gameObject.activeSelf);
            blush.color = goblin.blush.color;
            angryMark.gameObject.SetActive(goblin.angryMark.gameObject.activeSelf);
            angryMark.color = goblin.angryMark.color;
        }

        public void OnEvent(IEvent @event)
        {
            if (!(@event is SeductionChangeEvent seductionChangeEvent)) return;
            if (Mathf.Approximately(seductionChangeEvent.Change, 0))
                return;
            if (seductionChangeEvent.Target == goblin)
            {
                blush.gameObject.SetActive(false);
                angryMark.gameObject.SetActive(false);
                animator.SetTrigger(seductionChangeEvent.Change > 0 ? HearthsAnimation : BoltsAnimation);
            }
        }
    }
}