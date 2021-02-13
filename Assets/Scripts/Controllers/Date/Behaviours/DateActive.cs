using System;
using Events;
using Events.Goblin;
using Events.UI;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Date.Behaviours
{
    [Serializable]
    public class DateActive : ADateBehaviour
    {
        public override void OnTransitionIn(DateController context)
        {
            base.OnTransitionIn(context);
            Context.StartDate();
            GameController.Instance.Input.Player.Date.started += StopDate;
        }

        public override void OnTransitionOut()
        {
            GameController.Instance.Input.Player.Date.started -= StopDate;
        }

        private void StopDate(InputAction.CallbackContext obj)
        {
            Context.StateController.ChangeState(DateState.NonActive);
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
                case ChangeActiveGoblin change:
                    OnEvent(change);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnEvent(DateEvent @event)
        {
            if (!@event.Start)
            {
                Context.StateController.ChangeState(DateState.NonActive);
            }
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