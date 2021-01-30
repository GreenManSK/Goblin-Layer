using UnityEngine;

namespace Objects
{
    public class CollisionBlocker : MonoBehaviour
    {
        public Collider2D characterCollider;
        public Collider2D characterBlockerCollider;
        private void Start()
        {
            Physics2D.IgnoreCollision(characterCollider, characterBlockerCollider);
        }
    }
}