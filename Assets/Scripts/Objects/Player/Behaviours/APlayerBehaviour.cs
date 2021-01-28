namespace Objects.Player.Behaviours
{
    public abstract class APlayerBehaviour : IPlayerBehaviour
    {
        protected PlayerController Context;

        public void OnTransitionIn(PlayerController context)
        {
            Context = context;
        }

        public void OnTransitionOut()
        {
        }

        public void OnUpdate()
        {
        }

        public abstract bool IsState(PlayerState state);
    }
}