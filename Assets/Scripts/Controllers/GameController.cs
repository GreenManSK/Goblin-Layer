using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance => _instance;
        private static GameController _instance;

        public PlayerControlls Input => _input ??= new PlayerControlls();
        private PlayerControlls _input;

        public float datingRestartTimeInS = 10;
    
        private HashSet<GameObject> activeGoblins = new HashSet<GameObject>();

        public GameController()
        {
            _instance = this;
        }

        private void Awake()
        {
            _instance = this;
        }
    }
}