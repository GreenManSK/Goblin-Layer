using Cinemachine;
using Data;
using Objects.Player;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public static PlayerAbilities PlayerAbilities => _instance.playerAbilities;
        public static GameController Instance => _instance;
        private static GameController _instance;

        public PlayerControlls Input => _input ??= new PlayerControlls();
        private PlayerControlls _input;

        public PlayerAbilities playerAbilities = new PlayerAbilities();
        
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
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }

        public void SetCameraTarget(Transform target = null)
        {
            mainCamera.Follow = target ?? player.transform;
        }
    }
}