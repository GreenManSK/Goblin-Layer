namespace Objects.Player.Behaviours
{
    public class MovingPlayer: APlayerBehaviour
    {
        public override bool IsState(PlayerState state)
        {
            return state == PlayerState.Moving;
        }
    }
}