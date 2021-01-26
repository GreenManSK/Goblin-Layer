using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Types;

namespace Services
{
    public static class GoblinGenerator
    {
        public static readonly Glasses[] AllGlasses = GetArrayValues<Glasses>();
        public static readonly Glasses[] NoGlasses = {Glasses.None};
        public static readonly Blush[] AllBlushes = GetArrayValues<Blush>();
        public static readonly Blush[] NoBlushes = {Blush.None};
        public static readonly Expression[] AllExpressions = GetArrayValues<Expression>();
        public static readonly Costume[] AllCostumes = GetArrayValues<Costume>();
        public static readonly HairColor[] AllColors = GetArrayValues<HairColor>();

        public static readonly Dictionary<Accessory, float> DefaultAccessories = new Dictionary<Accessory, float>()
        {
            {Accessory.Choker, 0.5f},
            {Accessory.Flower, 0.5f}
        };

        public static readonly (HairFront, HairBack)[] AllHairstyles =
        {
            (HairFront.Long, HairBack.Long),
            (HairFront.Hime, HairBack.Short),
            (HairFront.Short, HairBack.ShortBot),
            (HairFront.TwinTails, HairBack.TwinTails)
        };

        private static Random _random = new Random();

        public static Goblin Generate(
            float beard = 0.5f,
            Glasses[] glasses = null,
            Blush[] blushes = null,
            Expression[] expressions = null,
            Costume[] costumes = null,
            HairColor[] hairColors = null,
            Dictionary<Accessory, float> accessories = null,
            (HairFront, HairBack)[] hairstyles = null
        )
        {
            glasses ??= AllGlasses;
            blushes ??= AllBlushes;
            expressions ??= AllExpressions;
            costumes ??= AllCostumes;
            hairColors ??= AllColors;
            accessories ??= DefaultAccessories;
            hairstyles ??= AllHairstyles;
            var randomAccessories = from accesory in accessories
                where _random.NextDouble() < accesory.Value
                select accesory.Key;
            var hairstyle = GetRandom(hairstyles);
            return new Goblin()
            {
                glasses = GetRandom(glasses),
                blush = GetRandom(blushes),
                beard = _random.NextDouble() < beard,
                accessories = randomAccessories.ToList(),
                costume = GetRandom(costumes),
                hairColor = GetRandom(hairColors),
                hairFront = hairstyle.Item1,
                hairBack = hairstyle.Item2,
                expression = GetRandom(expressions)
            };
        }

        private static T GetRandom<T>(IReadOnlyList<T> array)
        {
            var index = _random.Next(0, array.Count);
            return array[index];
        }

        private static T[] GetArrayValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}