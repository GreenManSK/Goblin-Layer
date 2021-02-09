using System;

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
        public static string Name(this SeductionType seductionType)
        {
            return seductionType switch
            {
                SeductionType.Compliment => "compliments",
                SeductionType.Flirt => "flirting",
                SeductionType.Insult => "being insulted",
                SeductionType.Present => "getting presents",
                SeductionType.Attack => "being attacked by you",
                SeductionType.Ask => "being asked questions",
                SeductionType.SeeOthers => "",
                SeductionType.BeforeOthers => "",
                SeductionType.AttackPlayer => "attacking you",
                _ => throw new ArgumentOutOfRangeException(nameof(seductionType), seductionType, null)
            };
        }
    }
}