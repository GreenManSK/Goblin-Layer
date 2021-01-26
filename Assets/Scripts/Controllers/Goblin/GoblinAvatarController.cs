using System;
using System.Collections.Generic;
using Data;
using Entities.Types;
using RotaryHeart.Lib.SerializableDictionary;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Goblin
{
    public class GoblinAvatarController : MonoBehaviour
    {
        [SerializeField]
        public Entities.Goblin data = GoblinGenerator.Generate();

        public Image hairBehind;
        public Image hairFront;
        public Image expression;
        public GameObject beard;
        public Image costume;
        public Image blush;
        public Image glasses;
        public AccessoryDictionary accessories = new AccessoryDictionary();

        private void Start()
        {
            UpdateDesign();
        }

        private void UpdateDesign()
        {
            var sprites = GoblinAvatarSprites.Instance;
            
            beard.SetActive(data.beard);
            foreach (var accessoryObject in accessories.Values)
            {
                accessoryObject.SetActive(false);
            }
            data.accessories.ForEach(accessory => accessories[accessory].SetActive(true));

            expression.sprite = sprites.expressions[data.expression];
            costume.sprite = sprites.costumes[data.costume];
            
            glasses.gameObject.SetActive(data.glasses != Glasses.None);
            if (data.glasses != Glasses.None)
            {
                glasses.sprite = sprites.glasses[data.glasses];
            }
            
            blush.gameObject.SetActive(data.blush != Blush.None);
            if (data.blush != Blush.None)
            {
                blush.sprite = sprites.blushes[data.blush];
            }

            var hair = sprites.hair[data.hairColor];
            hairBehind.sprite = hair.hairBack[data.hairBack];
            hairFront.sprite = hair.hairFront[data.hairFront];
        }
    }
    
    [System.Serializable]
    public class AccessoryDictionary: SerializableDictionaryBase<Accessory, GameObject> {}
}