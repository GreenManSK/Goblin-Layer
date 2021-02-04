using Objects.Golbin;
using UnityEngine;

namespace UI.Controllers
{
    public class TypeBarController : MonoBehaviour
    {
        public TypeBarItemController item;

        public void SetData(GoblinController goblin)
        {
            item.SetData(goblin);
        }
    }
}