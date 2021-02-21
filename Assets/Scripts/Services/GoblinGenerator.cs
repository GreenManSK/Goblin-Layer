using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Types;
using RotaryHeart.Lib.SerializableDictionary;

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

        public static readonly AccessoryDictionary DefaultAccessories = new AccessoryDictionary()
        {
            {Accessory.Choker, 0.5f},
            {Accessory.Flower, 0.5f}
        };

        public static readonly HairStyle[] AllHairstyles =
        {
            new HairStyle(HairFront.Long, HairBack.Long),
            new HairStyle(HairFront.Hime, HairBack.Short),
            new HairStyle(HairFront.Short, HairBack.ShortBot),
            new HairStyle(HairFront.TwinTails, HairBack.TwinTails)
        };

        private static Random _random = new Random();

        public static Goblin Generate(
            float beard = 0.25f,
            Glasses[] glasses = null,
            Blush[] blushes = null,
            Expression[] expressions = null,
            Costume[] costumes = null,
            HairColor[] hairColors = null,
            AccessoryDictionary accessories = null,
            HairStyle[] hairstyles = null
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
            var hairstyle = Helpers.GetRandom(hairstyles);
            return new Goblin()
            {
                glasses = Helpers.GetRandom(glasses),
                blush = Helpers.GetRandom(blushes),
                beard = _random.NextDouble() < beard,
                accessories = randomAccessories.ToList(),
                costume = Helpers.GetRandom(costumes),
                hairColor = Helpers.GetRandom(hairColors),
                hairFront = hairstyle.front,
                hairBack = hairstyle.back,
                expression = Helpers.GetRandom(expressions)
            };
        }

        private static T[] GetArrayValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }

    [Serializable]
    public class HairStyle
    {
        public HairFront front;
        public HairBack back;

        public HairStyle(HairFront front, HairBack back)
        {
            this.front = front;
            this.back = back;
        }
    }
    
    [System.Serializable]
    public class AccessoryDictionary: SerializableDictionaryBase<Accessory, float> {}
}