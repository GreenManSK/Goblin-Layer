using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class AttackingGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Idle");
        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            
            Context.Animator.SetTrigger(Animation);
            Context.weapon.goblin = Context;
            Context.weapon.SetRotation(Context.Player.transform.position - Context.transform.position);
            Context.weapon.SwingFinish += Finish;
            Context.weapon.gameObject.SetActive(true);
            
            Context.lastAttack = Time.time;
        }

        private void Finish()
        {
            Context.Chase();
        }

        public override void OnTransitionOut()
        {
            Context.weapon.SwingFinish -= Finish;
            Context.weapon.gameObject.SetActive(false);
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Attacking;
        }
    }
}