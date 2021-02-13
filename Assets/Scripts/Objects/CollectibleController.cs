using Constants;
using Entities;
using Events;
using Services;
using UnityEngine;

namespace Objects
{
    public class CollectibleController : MonoBehaviour
    {
        public Collectible collectible;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                GameEventSystem.Send(new CollectibleEvent(collectible));
                Destroy(gameObject);
            }
        }
    }
}