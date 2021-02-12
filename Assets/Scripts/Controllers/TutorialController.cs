using Events;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class TutorialController : MonoBehaviour
    {
        public GameObject attackBar;
        public GameObject dateBar;

        private void Start()
        {
            attackBar.SetActive(false);
            dateBar.SetActive(false);
            GameController.Instance.Input.Player.Fire.performed += OnFirstAttack;
        }

        private void OnDisable()
        {
            GameController.Instance.Input.Player.Fire.performed -= OnFirstAttack;
        }

        private void OnFirstAttack(InputAction.CallbackContext obj)
        {
            GameController.Instance.Input.Player.Fire.performed -= OnFirstAttack;
            GameEventSystem.Send(new DialogEvent("Swinging a sword is hard! I can't do this too often.", true));
            attackBar.SetActive(true);
        }
    }
}