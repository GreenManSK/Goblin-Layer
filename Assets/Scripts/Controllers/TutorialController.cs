using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entities.Types;
using Events;
using Events.Player;
using Events.UI;
using Objects.Enviroment;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class TutorialController : MonoBehaviour, IEventListener
    {
        private static readonly ReadOnlyCollection<Type> ListenEvents = new List<Type>
        {
            typeof(CollectibleEvent),
            typeof(PlayerHealthChange),
            typeof(PlayerAttackEvent)
        }.AsReadOnly();

        public GameObject attackBar;
        public GameObject dateBar;

        public DoorController door;

        public GameObject datePrompt;

        private bool _attacked = false;

        private void Start()
        {
            attackBar.SetActive(false);
            dateBar.SetActive(false);
        }

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
                case CollectibleEvent collectibleEvent:
                    OnCollectibleEvent(collectibleEvent);
                    break;
                case PlayerHealthChange playerHealthChangeEvent:
                    OnPlayerHealthChangeEvent(playerHealthChangeEvent);
                    break;
                case PlayerAttackEvent _:
                    OnPlayerAttackEvent();
                    break;
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
            GameEventSystem.Send(new DialogEvent("Swinging a sword is hard! I can't do this too often.", true));
            attackBar.SetActive(true);
        }

        private void OnPlayerHealthChangeEvent(PlayerHealthChange @event)
        {
            if (GameController.PlayerAbilities.startDate)
                return;
            if (@event.Health < 20f)
            {
                GameEventSystem.Send(new DialogEvent("I'm too weak to kill it. Maybe I can do something to save my life..."));
                datePrompt.SetActive(true);
                GameController.Mechanics.seduction = true;
                GameController.PlayerAbilities.startDate = true;
            }
        }

        private void OnCollectibleEvent(CollectibleEvent @event)
        {
            if (@event.Collectible.type == CollectibleType.Key)
            {
                GameEventSystem.Send(new DialogEvent("Key! Now I can open the door.", true));
                door.ChangeState(DoorState.Close);
            }
        }
    }
}