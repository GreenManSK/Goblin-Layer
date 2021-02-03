using Constants;
using Entities.Types;
using Events;
using Objects.Golbin;
using Services;
using UnityEngine;

namespace Controllers.Weapon
{
    public class GoblinWeaponController : AWeaponController
    {
        private const string TargetTag = Tags.Player;

        public GoblinController goblin;
        
        private bool _alreadyHit = false;

        private void OnEnable()
        {
            _alreadyHit = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;
            if (go.CompareTag(TargetTag) && !_alreadyHit)
            {
                GameEventSystem.Send(new AttackEvent(goblin.Player, Game.BaseGoblinAttack));
                GameEventSystem.Send(new SeductionEvent(goblin, SeductionType.AttackPlayer, Game.BaseSeduction, false));
            }
        }
    }
}