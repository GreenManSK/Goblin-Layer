using System;
using UnityEngine;

namespace Objects.StateMachine
{
    public abstract class ABehaviour<TC, TS> : IBehaviour<TC, TS> where TC : MonoBehaviour where TS : Enum
    {
        protected TC Context;
        
        public virtual void OnTransitionIn(TC context)
        {
            Context = context;
        }

        public virtual void OnTransitionOut()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public abstract bool IsState(TS state);
    }
}