using System;
using Events;
using Events.Goblin;
using Events.UI;
using Services;

namespace Controllers.Date.Behaviours
{
    [Serializable]
    public class DateNonActive : ADateBehaviour
    {
        public override void OnTransitionIn(DateController context)
        {
            base.OnTransitionIn(context);
            if (context.started)
                GameEventSystem.Send(new DateEvent(false));
            Context.StopDate();
        }

        public override bool ProcessEvent(IEvent @event)
        {
            switch (@event)
            {
                case GoblinActivationEvent activation:
                    OnEvent(activation);
                    break;
                case GoblinDeathEvent death:
                    OnEvent(death);
                    break;
                case DateEvent date:
                    OnEvent(date);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnEvent(DateEvent @event)
        {
            if (!@event.Start)
                return;
            if (Context.goblins.Count > 0)
            {
                Context.StateController.ChangeState(DateState.Active);
            }
            else
            {
                GameEventSystem.Send(new DialogEvent("Nobody is here. Who am I supposed to seduce? You?!", true));
            }
        }

        private void OnEvent(GoblinActivationEvent activation)
        {
            Context.goblins.Add(activation.Object);
        }

        private void OnEvent(GoblinDeathEvent death)
        {
            Context.goblins.Remove(death.Object);
        }

        public override bool IsState(DateState state)
        {
            return state == DateState.NonActive;
        }
    }
}