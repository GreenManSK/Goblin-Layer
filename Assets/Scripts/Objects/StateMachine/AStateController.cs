using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.StateMachine
{
    public abstract class AStateController<TC, TS> : MonoBehaviour where TC : MonoBehaviour where TS : Enum
    {
        private readonly Dictionary<TS, IBehaviour<TC, TS>> _behaviours = new Dictionary<TS, IBehaviour<TC, TS>>();

        public IBehaviour<TC, TS> Behaviour { get; private set; }
        protected TC Context;

        private void Awake()
        {
            SetContext();
        }

        private void SetContext()
        {
            if (Context == null)
                Context = GetContext();
        }

        private void Update()
        {
            Behaviour?.OnUpdate();
        }

        private void FixedUpdate()
        {
            Behaviour?.OnFixedUpdate();
        }

        public bool IsState(TS state)
        {
            return Behaviour != null && Behaviour.IsState(state);
        }

        public void ChangeState(TS state)
        {
            if (Behaviour != null && Behaviour.IsState(state))
            {
                return;
            }

            if (!_behaviours.ContainsKey(state))
            {
                _behaviours.Add(state, CreateBehaviour(state));
            }

            Behaviour?.OnTransitionOut();
            Behaviour = _behaviours[state];
            Behaviour.OnTransitionIn(Context ?? GetContext());
        }

        protected abstract TC GetContext();
        protected abstract IBehaviour<TC, TS> CreateBehaviour(TS state);
    }
}