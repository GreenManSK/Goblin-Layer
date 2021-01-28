namespace Objects.Player.Behaviours
{
    public class IdlePlayer : APlayerBehaviour
    {
        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Idle;
        }
    }
}