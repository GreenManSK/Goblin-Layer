using System.Linq;
using Data;
using Entities.Types;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Compendium
{
    public class CostumeSectionController : AppearanceSectionController<Costume>
    {
        protected override void SetData(Costume appearance, GameObject go)
        {
            go.GetComponentsInChildren<Image>().Last().sprite = GoblinAvatarSprites.Instance.costumes[appearance];
        }
    }
}