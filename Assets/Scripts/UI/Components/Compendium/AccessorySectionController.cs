using System.Linq;
using Data;
using Entities.Types;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Compendium
{
    public class AccessorySectionController : AppearanceSectionController<Accessory>
    {
        protected override void SetData(Accessory appearance, GameObject go)
        {
            go.GetComponentsInChildren<Image>().Last().sprite = GoblinAvatarSprites.Instance.accessories[appearance];
        }
    }
}