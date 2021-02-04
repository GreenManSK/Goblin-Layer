using System;
using Data;
using Events;
using Objects.Golbin;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class EncounterBarItemController : MonoBehaviour, IEventListener<SeductionChangeEvent>
    {
        public static readonly int GoblinAnimation = Animator.StringToHash("Goblin");
        public static readonly int HearthsAnimation = Animator.StringToHash("Hearths");
        public static readonly int BoltsAnimation = Animator.StringToHash("Bolts");
        
        public GoblinController goblin;
        public GameObject activeBg;
        public Animator animator;
        public Image typeIndicator;

        private void OnEnable()
        {
            animator.SetTrigger(GoblinAnimation);
            GameEventSystem.Subscribe(this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(this);
        }

        public void SetData(GoblinController data)
        {
            goblin = data;
            var type = GoblinTypesConfig.GetDefinition(goblin.type);
            typeIndicator.color = type.color;
            // TODO: Reaction efects
        }

        public void SetActive(bool active)
        {
            activeBg.SetActive(active);
        }

        public void OnEffectEnd()
        {
            animator.SetTrigger(GoblinAnimation);
        }

        public void OnEvent(SeductionChangeEvent @event)
        {
            if (@event.Target == goblin && !Mathf.Approximately(@event.Change, 0))
            {
                animator.SetTrigger(@event.Change > 0 ? HearthsAnimation : BoltsAnimation);
            }
        }
    }
}