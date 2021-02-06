using System;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    [Serializable]
    public class IdleGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            context.Animator.SetTrigger(Animation);
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Idle;
        }
    }
}