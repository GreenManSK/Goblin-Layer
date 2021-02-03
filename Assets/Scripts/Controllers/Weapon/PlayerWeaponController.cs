using System.Collections.Generic;
using Constants;
using Entities.Types;
using Events;
using Objects.Golbin;
using Services;
using UnityEngine;

namespace Controllers.Weapon
{
    public class PlayerWeaponController : AWeaponController
    {
        private const string TargetTag = Tags.Goblin;

        private readonly HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

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
                GameEventSystem.Send(new SeductionEvent(go.GetComponent<GoblinController>(), SeductionType.Attack,
                    Game.BaseSeduction, true));
            }
        }
    }
}