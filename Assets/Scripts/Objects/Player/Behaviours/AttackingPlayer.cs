using System;
using UnityEngine;

namespace Objects.Player.Behaviours
{
    [Serializable]
    public class AttackingPlayer : APlayerBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");

        public override void OnTransitionIn(PlayerController context)
        {
            base.OnTransitionIn(context);
            Context.Animator.SetTrigger(Animation);
            Context.weapon.SetRotation(Context.direction);
            Context.weapon.SwingFinish += Finish;
            Context.weapon.gameObject.SetActive(true);
        }

        private void Finish()
        {
            Context.FinishAttack();
        }

        public override void OnTransitionOut()
        {
            Context.weapon.SwingFinish -= Finish;
            Context.weapon.gameObject.SetActive(false);
        }

        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Attacking;
        }
    }
}