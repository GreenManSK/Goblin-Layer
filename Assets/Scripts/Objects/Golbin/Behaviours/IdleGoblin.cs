using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class IdleGoblin : AGoblinBehaviour
    {

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Idle;
        }
    }
}