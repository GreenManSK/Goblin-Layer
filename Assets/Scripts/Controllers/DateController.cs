using System.Collections.Generic;
using Events;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class DateController : MonoBehaviour, IEventListener<GoblinActivationEvent>,
        IEventListener<GoblinDeathEvent>, IEventListener<DateEvent>
    {
        private static readonly Vector2 PrevVector = new Vector2(-1, 1);
        private static readonly Vector2 NextVector = new Vector2(1, -1);

        public GameObject arrowPrefab;
        public List<GameObject> goblins = new List<GameObject>();

        private GameObject _arrow;
        private int _activeIndex;

        private void OnEnable()
        {
            GameEventSystem.Subscribe<GoblinActivationEvent>(this);
            GameEventSystem.Subscribe<GoblinDeathEvent>(this);
            GameEventSystem.Subscribe<DateEvent>(this);
        }

        private void OnDisable()
        {
            GameEventSystem.Unsubscribe<GoblinActivationEvent>(this);
            GameEventSystem.Unsubscribe<GoblinDeathEvent>(this);
            GameEventSystem.Unsubscribe<DateEvent>(this);
        }

        public void OnEvent(GoblinActivationEvent @event)
        {
            goblins.Add(@event.Object);
        }

        public void OnEvent(GoblinDeathEvent @event)
        {
            goblins.Remove(@event.Object);
        }

        public void OnEvent(DateEvent @event)
        {
            if (@event.Start)
            {
                StartDate();
            }
            else
            {
                StopDate();
            }
        }

        private void StartDate()
        {
            if (goblins.Count <= 0)
                return;
            goblins.Sort(GoblinSorter);
            _arrow = Instantiate(arrowPrefab, transform);
            SetActiveGoblin(0);
            GameController.Instance.Input.Player.Move.performed += ChangeActive;
            // TODO: Enable moving between goblins
        }

        private void StopDate()
        {
            Destroy(_arrow);
            GameController.Instance.Input.Player.Move.performed -= ChangeActive;
        }

        private void ChangeActive(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            var prevAngle = Vector2.Angle(input, PrevVector);
            var nextAngle = Vector2.Angle(input, NextVector);
            var change = prevAngle > nextAngle ? 1 : -1;
            SetActiveGoblin((goblins.Count + _activeIndex + change) % goblins.Count);
        }

        private void SetActiveGoblin(int index)
        {
            _activeIndex = index;
            _arrow.transform.position = goblins[_activeIndex].transform.position;
        }

        private int GoblinSorter(GameObject a, GameObject b)
        {
            var aPos = a.transform.position;
            var bPos = b.transform.position;
            return Mathf.Approximately(aPos.x, bPos.x) ? aPos.y.CompareTo(bPos.y) : aPos.x.CompareTo(bPos.x);
        }
    }
}