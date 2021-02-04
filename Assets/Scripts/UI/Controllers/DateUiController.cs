using System.Collections.Generic;
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
        public EncounterBarController encounterBar;
        public ActionBarController actionBar;
        public TypeBarController typeBar;

        private GoblinController _target;

        public void SetData(List<GoblinController> goblins)
        {
            encounterBar.SetData(goblins);
        }

        public void SetActions(int available, int maximum)
        {
            actionBar.SetActions(available, maximum);
        }

        public void SetGoblin(GoblinController goblin)
        {
            _target = goblin;
            encounterBar.SetActive(goblin);
            typeBar.SetData(goblin);
            avatar.data = goblin.data;
            avatar.UpdateDesign();
        }

        public void Seduce(SeductionDataMonoBehaviour obj)
        {
            // TODO: Use player stats
            GameEventSystem.Send(new SeductionEvent(_target, obj.type, Game.BaseSeduction));
        }

        public void OpenCompendium()
        {
            // TODO: Open
            Debug.Log("Opening compendium");
        }
    }
}