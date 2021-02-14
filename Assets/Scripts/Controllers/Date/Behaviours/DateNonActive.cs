using System;
using Events;
using Events.Goblin;
using Events.Input;
using Events.UI;
using Services;
using UnityEngine;

namespace Controllers.Date.Behaviours
{
    [Serializable]
    public class DateNonActive : ADateBehaviour
    {
        public override void OnTransitionIn(DateController context)
        {
            base.OnTransitionIn(context);
            if (context.started)
            {
                GameEventSystem.Send(new DateEvent(false));
            }

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
                case DateButtonEvent _:
                    StartDate();
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void StartDate()
        {
            if (!Context.canDate || !GameController.PlayerAbilities.startDate)
                return;
            if (Context.goblins.Count > 0)
            {
                Context.StateController.ChangeState(DateState.Active);
                GameEventSystem.Send(new DateEvent(true));
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