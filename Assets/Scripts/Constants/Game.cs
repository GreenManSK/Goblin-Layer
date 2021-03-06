using Entities.Types;

namespace Constants
{
    public static class Game
    {
        public const float BaseGoblinAttack = 10;
        public const float BaseSeduction = 20;
        public const int MaxActions = 3;
        public const int InventoryRows = 2;
        public const int InventoryColumns = 5;
        public const float MaxPlayerHealth = 100f;
        
        public static readonly GoblinType[] UnlockedTypes = new[] {GoblinType.Tsundere, GoblinType.Yandere, GoblinType.M};
    }
}