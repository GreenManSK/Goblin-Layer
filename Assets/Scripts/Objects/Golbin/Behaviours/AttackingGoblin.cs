using Constants;
using Entities.Types;
using Events;
using Services;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class AttackingGoblin : AGoblinBehaviour
    {
        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            GameEventSystem.Send(new AttackEvent(context.Player, Game.BaseGoblinAttack));
            GameEventSystem.Send(new SeductionEvent(Context, SeductionType.AttackPlayer, Game.BaseSeduction, false));
            Context.lastAttack = Time.time;
            Context.Chase();
        }
        
        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Attacking;
        }
    }
}