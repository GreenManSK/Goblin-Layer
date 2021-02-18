using Objects.Golbin;
using UnityEngine;

namespace UI.Components.Date
{
    public class TypeBarController : MonoBehaviour
    {
        public TypeBarItemController item;

        public void SetData(GoblinController goblin, float fontSize)
        {
            item.SetData(goblin, fontSize);
        }
    }
}