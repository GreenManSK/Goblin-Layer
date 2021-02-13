using UnityEngine;

namespace Objects
{
    public class DestroyableController : MonoBehaviour
    {
        public GameObject drop;

        public void Hit()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (drop == null) return;
            var go = Instantiate(drop, transform.parent);
            go.transform.position = transform.position;
        }
    }
}