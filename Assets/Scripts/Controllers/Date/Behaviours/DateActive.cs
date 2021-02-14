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
    public class DateActive : ADateBehaviour
    {
        public override void OnTransitionIn(DateController context)
        {
            base.OnTransitionIn(context);
            Context.StartDate();
        }

        private void StopDate()
        {
            Context.EnableDating();
            Context.StateController.ChangeState(DateState.NonActive);
            GameEventSystem.Send(new DateEvent(false));
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
                case ChangeActiveGoblin change:
                    OnEvent(change);
                    break;
                case DateButtonEvent _:
                    StopDate();
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnEvent(GoblinActivationEvent activation)
        {
            Context.goblins.Add(activation.Object);
        }

        private void OnEvent(GoblinDeathEvent death)
        {
            var index = Context.goblins.IndexOf(death.Object);
            if (index < 0)
                return;

            var newActiveIndex = Context.ActiveIndex;
            if (index <= Context.ActiveIndex)
            {
                newActiveIndex = Mathf.Max(0, Context.ActiveIndex - 1);
            }

            Context.goblins.RemoveAt(index);
            if (Context.goblins.Count <= 0)
            {
                GameEventSystem.Send(new DateEvent(false));
            }
            else
            {
                Context.dateUi.SetData(Context.goblins);
                Context.SetActiveGoblin(newActiveIndex);
            }
        }

        private void OnEvent(ChangeActiveGoblin change)
        {
            var newActive = Context.goblins.Count + Context.ActiveIndex + (@change.Next ? 1 : -1);
            Context.SetActiveGoblin(newActive % Context.goblins.Count);
        }

        public override bool IsState(DateState state)
        {
            return state == DateState.Active;
        }
    }
}