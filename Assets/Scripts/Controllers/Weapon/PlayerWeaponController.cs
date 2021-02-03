using System;
using System.Collections.Generic;
using Constants;
using Entities.Types;
using Events;
using Objects.Golbin;
using Services;
using UnityEngine;

namespace Controllers.Weapon
{
    public class PlayerWeaponController : MonoBehaviour
    {
        public delegate void SwingFinishDelegate();

        private const string TargetTag = Tags.Goblin;
        private static readonly Vector2 Forward = Vector2.right;

        public event SwingFinishDelegate SwingFinish;

        private readonly HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

        public void SetRotation(Vector2 direction)
        {
            var angle = Vector2.Angle(Forward, direction);
            if (Vector3.Cross(Forward, direction).z <= 0)
            {
                angle = 360 - angle;
            }

            angle -= 90;

            transform.rotation = Quaternion.Euler(0,0,angle);
        }

        private void OnEnable()
        {
            _alreadyHit.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;
            if (go.CompareTag(TargetTag) && !_alreadyHit.Contains(go.gameObject))
            {
                _alreadyHit.Add(go);
                Debug.Log("hit", go);
                GameEventSystem.Send(new SeductionEvent(go.GetComponent<GoblinController>(), SeductionType.Attack,
                    Game.BaseSeduction, true));
            }
        }

        protected void OnSwingFinish()
        {
            SwingFinish?.Invoke();
        }
    }
}