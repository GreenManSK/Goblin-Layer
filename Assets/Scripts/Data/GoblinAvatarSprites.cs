using Entities.Types;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Data
{
    public class GoblinAvatarSprites: MonoBehaviour
    {
        private static GoblinAvatarSprites _instance = null;

        public static GoblinAvatarSprites Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GoblinAvatarSprites>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        public ColorToHair hair = new ColorToHair();
        public ExpressionSprites expressions = new ExpressionSprites();
        public CostumeSprites costumes = new CostumeSprites();
        public BlushSprites blushes = new BlushSprites();
        public GlassesSprites glasses = new GlassesSprites();
    }
    
    [System.Serializable]
    public class HairBackSprites: SerializableDictionaryBase<HairBack, Sprite> {}
    
    [System.Serializable]
    public class HairFrontSprites: SerializableDictionaryBase<HairFront, Sprite> {}
    
    [System.Serializable]
    public class ExpressionSprites: SerializableDictionaryBase<Expression, Sprite> {}
    
    [System.Serializable]
    public class CostumeSprites: SerializableDictionaryBase<Costume, Sprite> {}
    
    [System.Serializable]
    public class BlushSprites: SerializableDictionaryBase<Blush, Sprite> {}
    
    [System.Serializable]
    public class GlassesSprites: SerializableDictionaryBase<Glasses, Sprite> {}
    
    [System.Serializable]
    public class ColorToHair: SerializableDictionaryBase<HairColor, Hair> {}

    [System.Serializable]
    public class Hair
    {
        public HairBackSprites hairBack = new HairBackSprites();
        public HairFrontSprites hairFront = new HairFrontSprites();
    }
}