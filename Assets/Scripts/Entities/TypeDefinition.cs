using System;
using System.Collections.Generic;
using Entities.Types;
using Services;
using UnityEngine;
using Random = System.Random;

namespace Entities
{
    [Serializable]
    public class TypeDefinition
    {
        private static Random _random = new Random();

        public Color color;
        public float compliment = 1f;
        public float flirt = 1f;
        public float insult = 1f;
        public float present = 1f;
        public float attack = 1f;
        public float ask = 1f;
        public float attackPlayer = 1f;
        public float envy = 1f;
        public float pride = 1f;

        public Glasses[] glasseses = GoblinGenerator.NoGlasses;
        public AccessoryDictionary accessories = new AccessoryDictionary();
        public Costume[] costumes = GoblinGenerator.AllCostumes;
        public HairStyle[] hairs = GoblinGenerator.AllHairstyles;
        public Expression[] expressions = GoblinGenerator.AllExpressions;

        public string description = null;
        public List<string> dateStartTexts = new List<string>();
        public List<string> positiveReactionTexts = new List<string>();
        public List<string> negativeReactionTexts = new List<string>();
        public List<string> neutralReactionTexts = new List<string>();

        public string RandomDateStartText()
        {
            return RandomText(dateStartTexts) ?? "Date start text placeholder";
        }

        public string RandomPositiveReactionText()
        {
            return RandomText(positiveReactionTexts) ?? "Positive reaction placeholder";
        }

        public string RandomNegativeReactionText()
        {
            return RandomText(negativeReactionTexts) ?? "Negative reaction placeholder";
        }

        public string RandomNeutralReactionText()
        {
            return RandomText(neutralReactionTexts) ?? "Neutral reaction placeholder";
        }

        private string RandomText(IReadOnlyList<string> texts)
        {
            if (texts.Count == 0)
                return null;
            var index = _random.Next(0, texts.Count);
            return texts[index];
        }
    }
}