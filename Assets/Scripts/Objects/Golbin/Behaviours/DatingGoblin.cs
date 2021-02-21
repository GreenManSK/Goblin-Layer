using System;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    [Serializable]
    public class DatingGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            context.Animator.SetTrigger(Animation);
            if (context.StateController.LastState != GoblinState.Stopped)
            {
                context.lastSeduction = new LastSeduction();
            }
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Dating;
        }
    }
}