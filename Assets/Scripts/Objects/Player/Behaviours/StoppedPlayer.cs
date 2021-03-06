using System;
using UnityEngine;

namespace Objects.Player.Behaviours
{
    [Serializable]
    public class StoppedPlayer: APlayerBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(PlayerController context)
        {
            base.OnTransitionIn(context);
            context.Animator.SetTrigger(Animation);
        }
        
        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Stopped;
        }
    }
}