namespace Events.Player
{
    public class AttackBarEvent : IEvent
    {
        public float WaitTimeInS { get; }

        public AttackBarEvent(float waitTimeInS)
        {
            WaitTimeInS = waitTimeInS;
        }
    }
}