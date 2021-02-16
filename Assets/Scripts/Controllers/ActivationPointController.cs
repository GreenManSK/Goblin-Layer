using System.Collections.Generic;
using Constants;
using Objects.Enviroment;
using Objects.Golbin;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers
{
    public class ActivationPointController : MonoBehaviour
    {
        public List<GoblinController> goblins = new List<GoblinController>();
        public UnityEvent triggers;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                ActivateGoblins(other.gameObject);
                triggers?.Invoke();
                Destroy(gameObject);
            }
        }

        private void ActivateGoblins(GameObject player)
        {
            goblins.ForEach(g => g.Activate(player));
            Destroy(gameObject);
        }
    }
}