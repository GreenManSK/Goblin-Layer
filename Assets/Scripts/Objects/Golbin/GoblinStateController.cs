using System;
using Objects.Golbin.Behaviours;
using Objects.StateMachine;
using UnityEngine;

namespace Objects.Golbin
{
    [RequireComponent(typeof(GoblinController))]
    public class GoblinStateController : AStateController<GoblinController, GoblinState>
    {
        public GoblinState LastState => _lastState;
        private GoblinState _lastState;

        protected override GoblinController GetContext()
        {
            return GetComponent<GoblinController>();
        }

        public override void ChangeState(GoblinState state)
        {
            _lastState = CurrentState;
            base.ChangeState(state);
        }

        protected override IBehaviour<GoblinController, GoblinState> CreateBehaviour(GoblinState state)
        {
            return state switch
            {
                GoblinState.Idle => new IdleGoblin(),
                GoblinState.Chasing => new ChasingGoblin(),
                GoblinState.Attacking => new AttackingGoblin(),
                GoblinState.Dating => new DatingGoblin(),
                GoblinState.Stopped => new StopedGoblin(),
                GoblinState.Seduced => new SeducedGoblin(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}