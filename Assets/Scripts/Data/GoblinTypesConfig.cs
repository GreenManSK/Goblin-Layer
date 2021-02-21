using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Types;
using RotaryHeart.Lib.SerializableDictionary;
using Services;
using UnityEngine;

namespace Data
{
    public class GoblinTypesConfig : MonoBehaviour
    {
        private static GoblinTypesConfig _instance = null;

        public static GoblinTypesConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GoblinTypesConfig>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        public GoblinTypeToDefinition definitions;
        public List<SeductionToExpression> seductionExpressions;
        public ReactionToExpression reactionToExpression;

        public static float GetMultiplier(GoblinType type, SeductionType seductionType)
        {
            var definition = Instance.definitions[type];
            return seductionType switch
            {
                SeductionType.Compliment => definition.compliment,
                SeductionType.Flirt => definition.flirt,
                SeductionType.Insult => definition.insult,
                SeductionType.Present => definition.present,
                SeductionType.Attack => definition.attack,
                SeductionType.Ask => definition.ask,
                SeductionType.SeeOthers => definition.envy,
                SeductionType.BeforeOthers => definition.pride,
                SeductionType.AttackPlayer => definition.attackPlayer,
                _ => 0
            };
        }

        public static TypeDefinition GetDefinition(GoblinType type)
        {
            return Instance.definitions[type];
        }

        public static bool IsPositiveSeduction(GoblinType type, SeductionType seductionType)
        {
            var multiplier = GetMultiplier(type, seductionType);
            if (seductionType == SeductionType.Present)
                multiplier -= 1;

            return multiplier > 0;
        }

        public static Expression GetSeductionExpression(float seduction, Expression currentExpression)
        {
            foreach (var seductionToExpression in Instance.seductionExpressions)
            {
                if (seduction < seductionToExpression.upperBound)
                {
                    return !seductionToExpression.expressions.Contains(currentExpression)
                        ? Helpers.GetRandom(seductionToExpression.expressions)
                        : currentExpression;
                }
            }

            return Expression.Normal;
        }

        public static Expression GetReactionExpression(SeductionReaction reaction)
        {
            return Helpers.GetRandom(Instance.reactionToExpression[reaction].expressions);
        }
    }

    [Serializable]
    public class GoblinTypeToDefinition : SerializableDictionaryBase<GoblinType, TypeDefinition>
    {
    }
    
    [Serializable]
    public class ReactionToExpression : SerializableDictionaryBase<SeductionReaction, ExpressionList> {}

    
    [Serializable]
    public class ExpressionList
    {
        public Expression[] expressions;
    }
}