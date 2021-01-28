namespace Objects.Player.Behaviours
{
    public class DatingPlayer: APlayerBehaviour
    {
        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Dating;
        }
    }
}