using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entities.Types;
using Events;
using Events.Date;
using Events.Player;
using Events.UI;
using Objects.Golbin;
using Services;
using UI.Controllers.Date;
using UnityEngine;

namespace Controllers
{
    public class TutorialDateController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(SeductionEvent),
            typeof(PresentEvent)
        }.AsReadOnly();

        public int step = 0;
        // 0 compliment
        // 1 insult
        // 2 present

        public GoblinController goblin;
        public DateUiController dateUi;

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is SeductionEvent seductionEvent)
            {
                OnSeductionEvent(seductionEvent);
            }
            else if (@event is PresentEvent presentEvent)
            {
                OnPresentEvent(presentEvent);
            }
        }

        private void OnPresentEvent(PresentEvent presentEvent)
        {
            if (step == 2)
            {
                step++;
                GameController.Instance.playerAbilities.present = false;
                GameEventSystem.Send(new AbilityChangeEvent());
                GameEventSystem.Send(new DialogEvent("You", "It dropped a book. Maybe I should check it out."));
            }
        }

        private void OnSeductionEvent(SeductionEvent seductionEvent)
        {
            if (step == 0 && seductionEvent.Type == SeductionType.Compliment)
            {
                goblin.data.hide = false;
                step++;
                dateUi.SetGoblin(goblin);
                GameEventSystem.Send(new DialogEvent("You",
                    "What? It does not look scary at all now. Maybe I can tell it what I think of it, and it will run away!"));
                GameController.Instance.playerAbilities.compliment = false;
                GameController.Instance.playerAbilities.insult = true;
                GameEventSystem.Send(new AbilityChangeEvent());
            }
            else if (step == 1 && seductionEvent.Type == SeductionType.Insult)
            {
                step++;
                GameEventSystem.Send(new DialogEvent("You",
                    "It didn't like that... Maybe I can try to give it a present? I had this nice rock with me!"));
                GameController.Instance.playerAbilities.insult = false;
                GameController.Instance.playerAbilities.present = true;
                GameEventSystem.Send(new AbilityChangeEvent());
            }
        }
    }
}