namespace Objects.Player.Behaviours
{
    public abstract class APlayerBehaviour : IPlayerBehaviour
    {
        protected PlayerController Player;

        public virtual void OnTransitionIn(PlayerController context)
        {
            Player = context;
        }

        public virtual void OnTransitionOut()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public abstract bool IsState(PlayerState state);
    }
}