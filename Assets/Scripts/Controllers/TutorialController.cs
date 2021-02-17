using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Constants;
using Data;
using Entities.Types;
using Events;
using Events.Goblin;
using Events.Player;
using Events.UI;
using Objects.Enviroment;
using Objects.Golbin;
using Services;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Constants.Scene;

namespace Controllers
{
    public class TutorialController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(CollectibleEvent),
            typeof(PlayerHealthChange),
            typeof(PlayerAttackEvent),
            typeof(DateEvent),
            typeof(GoblinDeathEvent)
        }.AsReadOnly();

        public AttackBar attackBar;
        public DatingBar dateBar;

        public DoorController door;
        public GoblinController firstGoblin;
        public List<SpikesController> spikes = new List<SpikesController>();
        public TrapdoorController trapdoor;
        public GameObject blocker;

        public GameObject datePrompt;
        public Scene nextScene;

        private bool _attacked = false;
        private int _date = 0;
        private int _deadGoblins = 0;

        private void Start()
        {
            attackBar.SetVisibility(false);
            dateBar.SetVisibility(false);
        }

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe(ListenEvents, this);
        }

        public void ToggleSpikes()
        {
            spikes.ForEach(s => s.ChangeState(s.state == SpikesState.Down ? SpikesState.Up : SpikesState.Down));
            blocker.SetActive(!blocker.activeSelf);
        }

        public void PrepareSecondEncounter()
        {
            _date++;
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(nextScene.GetName());
        }

        public void OnEvent(IEvent @event)
        {
            switch (@event)
            {
                case CollectibleEvent collectibleEvent:
                    OnCollectibleEvent(collectibleEvent);
                    break;
                case PlayerHealthChange playerHealthChangeEvent:
                    OnPlayerHealthChangeEvent(playerHealthChangeEvent);
                    break;
                case PlayerAttackEvent _:
                    OnPlayerAttackEvent();
                    break;
                case DateEvent dateEvent:
                    OnDateEvent(dateEvent);
                    break;
                case GoblinDeathEvent _:
                    OnGoblinDeathEvent();
                    break;
            }
        }

        private void OnGoblinDeathEvent()
        {
            _deadGoblins++;
            if (_deadGoblins >= 3)
            {
                trapdoor.SetState(TrapdoorState.Open);
            }
        }

        private void OnDateEvent(DateEvent @event)
        {
            if (_date == 3 && !@event.Start)
            {
                GameEventSystem.Send(new DialogEvent("You",
                    "It's so embarrassing! I need some time before I speak with them again. Maybe I should run, so they don't kill me in the meantime."));
                _date++;
            }

            if (!@event.Start)
                return;
            if (_date == 0)
            {
                dateBar.SetVisibility(true);
                GameEventSystem.Send(new DialogEvent("You", "Maybe a compliment will convince it to spare my life?",
                    false));
                _date++;
            }
            else if (_date == 2)
            {
                GameEventSystem.Send(new DialogEvent("You", "Now there are two of them. I should check out both."));
                _date++;
            }
        }

        private void OnPlayerAttackEvent()
        {
            if (_attacked)
                return;
            _attacked = true;
            StartCoroutine(AttackPrompt());
        }

        private IEnumerator AttackPrompt()
        {
            yield return new WaitForSeconds(0.5f);
            GameEventSystem.Send(new DialogEvent("You", "Swinging a sword is hard! I can't do this too often.", true));
            attackBar.SetVisibility(true);
        }

        private void OnPlayerHealthChangeEvent(PlayerHealthChange @event)
        {
            if (GameController.PlayerAbilities.startDate)
                return;
            if (@event.Health < 20f)
            {
                GameEventSystem.Send(new DialogEvent("You",
                    "I'm too weak to kill it. Maybe I can do something to save my life..."));
                firstGoblin.attackSpeedInS = float.MaxValue;
                datePrompt.SetActive(true);
                GameController.Mechanics.seduction = true;
                GameController.PlayerAbilities.startDate = true;
            }
        }

        private void OnCollectibleEvent(CollectibleEvent @event)
        {
            switch (@event.Collectible.type)
            {
                case CollectibleType.Key:
                    GameEventSystem.Send(new DialogEvent("You", "Key! Now I can open the door.", true));
                    door.ChangeState(DoorState.Close);
                    break;
                case CollectibleType.Compendium:
                    GameEventSystem.Send(new DialogEvent("You",
                        "\"Compendium: Guide to seducing goblins\" What degenerate would write something like this? And who would read it?"));
                    GameEventSystem.Send(new HealEvent(640f));
                    GameController.PlayerAbilities.die = true;
                    GameController.Mechanics.attackSeduction = true;
                    ToggleSpikes();
                    break;
            }
        }
    }
}