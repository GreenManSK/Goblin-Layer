using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.StateMachine
{
    public abstract class AStateController<TC, TS> : MonoBehaviour where TC : MonoBehaviour where TS : Enum
    {
        private readonly Dictionary<TS, IBehaviour<TC, TS>> _behaviours = new Dictionary<TS, IBehaviour<TC, TS>>();

        public TS CurrentState => _currentState;
        public IBehaviour<TC, TS> Behaviour => _behaviour;
        protected TC Context;

        private TS _currentState;
        
        [SerializeField]
        private IBehaviour<TC, TS> _behaviour;
        
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
            _behaviour?.OnUpdate();
        }

        private void FixedUpdate()
        {
            _behaviour?.OnFixedUpdate();
        }

        public bool IsState(TS state)
        {
            return _behaviour != null && _behaviour.IsState(state);
        }

        public virtual void ChangeState(TS state)
        {
            if (_behaviour != null && _behaviour.IsState(state))
            {
                return;
            }

            if (!_behaviours.ContainsKey(state))
            {
                _behaviours.Add(state, CreateBehaviour(state));
            }

            _behaviour?.OnTransitionOut();
            _behaviour = _behaviours[state];
            _currentState = state;
            _behaviour.OnTransitionIn(Context ?? GetContext());
        }

        protected abstract TC GetContext();
        protected abstract IBehaviour<TC, TS> CreateBehaviour(TS state);
    }
}