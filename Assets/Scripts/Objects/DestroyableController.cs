using UnityEngine;

namespace Objects
{
    public class DestroyableController : MonoBehaviour
    {
        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}