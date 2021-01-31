using UnityEngine;

namespace Objects
{
    public class EffectController : MonoBehaviour
    {
        public bool temporary = false;

        public void OnAnimationEnd()
        {
            if (temporary)
            {
                Destroy(gameObject);
            }
        }
    }
}