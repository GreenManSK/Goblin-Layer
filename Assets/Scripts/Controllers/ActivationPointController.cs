using System.Collections.Generic;
using Constants;
using Objects.Enviroment;
using Objects.Golbin;
using UnityEngine;

namespace Controllers
{
    public class ActivationPointController : MonoBehaviour
    {
        public GameObject player;
        public List<GoblinController> goblins = new List<GoblinController>();
        public List<SpikesController> spikes = new List<SpikesController>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                player = other.gameObject;
                ActivateGoblins();
                ActivateSpikes();
                Destroy(gameObject);
            }
        }

        private void ActivateSpikes()
        {
            spikes.ForEach(s => s.ChangeState(SpikesState.Up));
        }

        private void ActivateGoblins()
        {
            goblins.ForEach(g => g.Activate(player));
            Destroy(gameObject);
        }
    }
}