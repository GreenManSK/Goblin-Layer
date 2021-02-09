using System.Linq;
using Data;
using Entities.Types;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Compendium
{
    public class GlassesSectionController : AppearanceSectionController<Glasses>
    {
        protected override void SetData(Glasses appearance, GameObject go)
        {
            go.GetComponentsInChildren<Image>().Last().sprite = GoblinAvatarSprites.Instance.glasses[appearance];
        }
    }
}