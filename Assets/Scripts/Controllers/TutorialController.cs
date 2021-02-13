using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entities.Types;
using Events;
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
            typeof(CollectibleEvent)
        }.AsReadOnly();
        
        public GameObject attackBar;
        public GameObject dateBar;

        public DoorController door;

        private void Start()
        {
            attackBar.SetActive(false);
            dateBar.SetActive(false);
            GameController.Instance.Input.Player.Fire.performed += OnFirstAttack;
        }

        private void OnEnable()
        {
            GameEventSystem.Subscribe(ListenEvents, this);
        }

        private void OnDisable()
        {
            GameController.Instance.Input.Player.Fire.performed -= OnFirstAttack;
            GameEventSystem.Unsubscribe(ListenEvents, this);
        }

        private void OnFirstAttack(InputAction.CallbackContext obj)
        {
            GameController.Instance.Input.Player.Fire.performed -= OnFirstAttack;
            GameEventSystem.Send(new DialogEvent("Swinging a sword is hard! I can't do this too often.", true));
            attackBar.SetActive(true);
        }

        public void OnEvent(IEvent @event)
        {
            if (@event is CollectibleEvent collectibleEvent)
            {
                OnCollectibleEvent(collectibleEvent);
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