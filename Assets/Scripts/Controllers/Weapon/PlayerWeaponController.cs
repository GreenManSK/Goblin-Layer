using System.Collections.Generic;
using Constants;
using Entities.Types;
using Events;
using Objects;
using Objects.Golbin;
using Services;
using UnityEngine;

namespace Controllers.Weapon
{
    public class PlayerWeaponController : AWeaponController
    {
        private readonly HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

        private void OnEnable()
        {
            _alreadyHit.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;
            if (_alreadyHit.Contains(go.gameObject))
                return;
            _alreadyHit.Add(go);
            if (go.CompareTag(Tags.Goblin))
            {
                GameEventSystem.Send(new SeductionEvent(go.GetComponent<GoblinController>(), SeductionType.Attack,
                    Game.BaseSeduction, true));
            } else if (go.CompareTag(Tags.Destroyable))
            {
                go.GetComponent<DestroyableController>()?.Hit();
            }
        }
    }
}