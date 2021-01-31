using Controllers.Goblin;
using Entities;
using UnityEngine;

namespace UI.Controllers
{
    public class DateUiController : MonoBehaviour
    {
        public GoblinAvatarController avatar;

        public void SetGoblin(Goblin goblin)
        {
            avatar.data = goblin;
            avatar.UpdateDesign();
        }
    }
}