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
    }

    [System.Serializable]
    public class GoblinTypeToDefinition : SerializableDictionaryBase<GoblinType, TypeDefinition>
    {
    }
}