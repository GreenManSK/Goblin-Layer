using System;
using Objects.Golbin.Behaviours;
using Objects.StateMachine;
using UnityEngine;

namespace Objects.Golbin
{
    [RequireComponent(typeof(GoblinController))]
    public class GoblinStateController : AStateController<GoblinController, GoblinState>
    {
        protected override GoblinController GetContext()
        {
            return GetComponent<GoblinController>();
        }

        protected override IBehaviour<GoblinController, GoblinState> CreateBehaviour(GoblinState state)
        {
            return state switch
            {
                GoblinState.Idle => new IdleGoblin(),
                GoblinState.Chasing => new ChasingGoblin(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}