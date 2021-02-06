using System;

namespace Objects.Player.Behaviours
{
    [Serializable]
    public class DatingPlayer: APlayerBehaviour
    {
        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Dating;
        }
    }
}