using System;
using Entities;
using Entities.Types;
using RotaryHeart.Lib.SerializableDictionary;
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
    }

    [Serializable]
    public class GoblinTypeToDefinition : SerializableDictionaryBase<GoblinType, TypeDefinition>
    {
    }
}