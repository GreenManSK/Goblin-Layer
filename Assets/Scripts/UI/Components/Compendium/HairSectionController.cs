using Data;
using Entities.Types;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Compendium
{
    public class HairSectionController : AppearanceSectionController<HairStyle>
    {
        protected override void SetData(HairStyle appearance, GameObject go)
        {
            var images = go.GetComponentsInChildren<Image>();
            images[1].sprite = GoblinAvatarSprites.Instance.hair[HairColor.Silver].hairBack[appearance.back];
            images[2].sprite = GoblinAvatarSprites.Instance.hair[HairColor.Silver].hairFront[appearance.front];
        }
    }
}