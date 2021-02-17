using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Controllers.Date;
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
            typeof(PresentEvent),
            typeof(ChangeActiveGoblin),
            typeof(DateUiStateChangeEvent)
        }.AsReadOnly();

        public int step = 0;
        // 0 compliment
        // 1 insult
        // 2 present
        // 3 switching
        // 4 first compliment
        // 5 compendium

        public GoblinController goblin;
        public DateUiController dateUi;
        public DateController date;

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
            switch (@event)
            {
                case SeductionEvent seductionEvent:
                    OnSeductionEvent(seductionEvent);
                    break;
                case PresentEvent presentEvent:
                    OnPresentEvent(presentEvent);
                    break;
                case ChangeActiveGoblin changeEvent:
                    OnChangeEvent(changeEvent);
                    break;
                case DateUiStateChangeEvent dateUiStateEvent:
                    OnDateUiStateEvent(dateUiStateEvent);
                    break;
            }
        }

        private void OnDateUiStateEvent(DateUiStateChangeEvent dateUiStateEvent)
        {
            if (step == 5 && dateUiStateEvent.State == DateUiState.Compendium)
            {
                GameController.Instance.playerAbilities.changeActive = true;
                GameController.Instance.playerAbilities.compendium = true;
                GameController.Instance.playerAbilities.compliment = true;
                GameController.Instance.playerAbilities.flirt = true;
                GameController.Instance.playerAbilities.insult = true;
                GameController.Instance.playerAbilities.present = true;
                GameController.Instance.playerAbilities.ask = true;
                GameEventSystem.Send(new AbilityChangeEvent());
                step++;
            }
        }

        private void OnChangeEvent(ChangeActiveGoblin changeEvent)
        {
            if (step == 3 && date.ActiveGoblin.type == GoblinType.Yandere)
            {
                GameEventSystem.Send(new DialogEvent("You",
                    "They look and act really different. I thought all goblins are the same. Time for compliments!"));
                GameController.Instance.playerAbilities.changeActive = false;
                GameController.Instance.playerAbilities.compliment = true;
                GameEventSystem.Send(new AbilityChangeEvent());
                step++;
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
            else if (step == 4 && seductionEvent.Type == SeductionType.Compliment)
            {
                GameEventSystem.Send(new DialogEvent("You",
                    "The other goblin didn't seem to like I complimented this one. Maybe I should take a look at the book I picked up... Mom, please forgive me."));
                GameController.Instance.playerAbilities.compliment = false;
                GameController.Instance.playerAbilities.changeActive = true;
                GameController.Instance.playerAbilities.compendium = true;
                GameEventSystem.Send(new AbilityChangeEvent());
                step++;
            }
        }
    }
}