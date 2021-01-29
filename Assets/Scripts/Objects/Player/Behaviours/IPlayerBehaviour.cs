namespace Objects.Player.Behaviours
{
    public interface IPlayerBehaviour
    {
        void OnTransitionIn(PlayerController context);
        void OnTransitionOut();
        void OnUpdate();
        void OnFixedUpdate();
        bool IsState(PlayerState state);
    }
}