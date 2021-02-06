using System;
using Objects.StateMachine;

namespace Objects.Player.Behaviours
{
    [Serializable]
    public abstract class APlayerBehaviour : ABehaviour<PlayerController, PlayerState>
    {
    }
}