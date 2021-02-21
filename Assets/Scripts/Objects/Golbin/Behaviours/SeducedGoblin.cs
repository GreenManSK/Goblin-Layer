using Data;
using Events.UI;
using Services;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class SeducedGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            context.Animator.SetTrigger(Animation);
            context.Disable();
            Object.Instantiate(context.hearthsPrefab, context.transform);
            GameEventSystem.Send(new DialogEvent("Goblin",
                GoblinTypesConfig.GetDefinition(context.type).RandomSeducedReactionText(), true, DialogColor.Positive));
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Seduced;
        }
    }
}