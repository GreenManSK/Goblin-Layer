using UnityEngine;

namespace Controllers.Weapon
{
    public abstract class AWeaponController : MonoBehaviour
    {
        public delegate void SwingFinishDelegate();
        private static readonly Vector2 Forward = Vector2.right;
        
        public event SwingFinishDelegate SwingFinish;
        
        public void SetRotation(Vector2 direction)
        {
            var angle = Vector2.Angle(Forward, direction);
            if (Vector3.Cross(Forward, direction).z <= 0)
            {
                angle = 360 - angle;
            }

            angle -= 90;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected void OnSwingFinish()
        {
            SwingFinish?.Invoke();
        }
    }
}