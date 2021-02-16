using UnityEngine;

namespace Objects
{
    public class DropController : MonoBehaviour
    {
        public GameObject drop;

        private void OnDestroy()
        {
            if (drop == null) return;
            var go = Instantiate(drop, transform.parent);
            go.transform.position = transform.position;
        }
    }
}