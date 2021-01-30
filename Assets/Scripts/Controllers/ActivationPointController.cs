using System.Collections.Generic;
using Constants;
using Objects.Golbin;
using UnityEngine;

namespace Controllers
{
    public class ActivationPointController : MonoBehaviour
    {
        public GameObject player;
        public List<GoblinController> goblins = new List<GoblinController>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                player = other.gameObject;
                ActivateGoblins();
            }
        }

        private void ActivateGoblins()
        {
            goblins.ForEach(g => g.Activate(player));
            Destroy(gameObject);
        }
    }
}