using System;
using Constants;
using Controllers.Goblin;
using Events;
using Objects.Golbin;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class DateUiController : MonoBehaviour
    {
        public GoblinAvatarController avatar;
        public Text typeText;
        public Text seductionText;

        private GoblinController _target;

        private void Update()
        {
            if (_target == null)
                return;
            typeText.text = $"Type: {_target.type.ToString()}";
            seductionText.text = $"Seduction: {_target.seduction}";
        }

        public void SetGoblin(GoblinController goblin)
        {
            _target = goblin;
            avatar.data = goblin.data;
            avatar.UpdateDesign();
        }

        public void Seduce(SeductionDataMonoBehaviour obj)
        {
            // TODO: Use player stats
            GameEventSystem.Send(new SeductionEvent(_target, obj.type, Game.BaseSeduction));
        }
    }
}