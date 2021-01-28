namespace Objects.Player.Behaviours
{
    public interface IPlayerBehaviour
    {
        void OnTransitionIn(PlayerController context);
        void OnTransitionOut();
        void OnUpdate();
        bool IsState(PlayerState state);
    }
}