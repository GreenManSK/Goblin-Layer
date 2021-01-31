namespace Entities.Types
{
    public enum SeductionType
    {
        Compliment,
        Flirt,
        Insult,
        Present,
        Attack,
        Ask,
        SeeOthers,
        BeforeOthers,
        AttackPlayer
    }

    public static class SeductionTypeHelper
    {
        public static bool IsPositive(this SeductionType type)
        {
            return type == SeductionType.Compliment || type == SeductionType.Flirt || type == SeductionType.Present;
        }
    }
}