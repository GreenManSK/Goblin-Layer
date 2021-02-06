using System;
using Objects.StateMachine;

namespace Objects.Golbin.Behaviours
{
    [Serializable]
    public abstract class AGoblinBehaviour : ABehaviour<GoblinController, GoblinState>
    {
    }
}