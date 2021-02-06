using System;
using Events;
using Objects.StateMachine;

namespace Controllers.Date.Behaviours
{
    [Serializable]
    public abstract class ADateBehaviour : ABehaviour<DateController, DateState>
    {
        public abstract bool ProcessEvent(IEvent @event);
    }
}