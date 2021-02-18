using Cinemachine;
using Data;
using Events;
using Events.Input;
using Objects.Player;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public static PlayerAbilities PlayerAbilities => _instance.playerAbilities;
        public static Mechanics Mechanics => _instance.mechanics;
        public static GameController Instance => _instance;
        private static GameController _instance;

        public PlayerControlls Input => _input ??= new PlayerControlls();
        private PlayerControlls _input;

        public PlayerAbilities playerAbilities = new PlayerAbilities();
        public Mechanics mechanics = new Mechanics();

        public PlayerController player;
        public CinemachineVirtualCamera mainCamera;
        public float datingRestartTimeInS = 10;

        public GameController()
        {
            _instance = this;
        }

        private void Awake()
        {
            _instance = this;
            FindDependencies();
        }

        private void FindDependencies()
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerController>();
            }

            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }

        public void SetCameraTarget(Transform target = null)
        {
            mainCamera.Follow = target ?? player.transform;
        }

        private void OnEnable()
        {
            Input.Player.Enable();
            Input.Player.Fire.started += OnFireButton;
            Input.Player.Date.started += OnDateButton;
        }

        private void OnDisable()
        {
            Input.Player.Fire.started -= OnFireButton;
            Input.Player.Date.started -= OnDateButton;
            Input.Player.Disable();
        }

        private static void OnFireButton(InputAction.CallbackContext obj)
        {
            GameEventSystem.Send(new AttackButtonEvent());
        }

        private static void OnDateButton(InputAction.CallbackContext obj)
        {
            GameEventSystem.Send(new DateButtonEvent());
        }

        public static bool IsInputEvent(IEvent @event)
        {
            return @event is AttackButtonEvent || @event is DateButtonEvent;
        }
    }
}