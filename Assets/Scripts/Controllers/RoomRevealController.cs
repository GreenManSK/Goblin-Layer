using Constants;
using UnityEngine;

namespace Controllers
{
    public class RoomRevealController : MonoBehaviour
    {
        public GameObject room;

        private void Start()
        {
            room.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                room.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}