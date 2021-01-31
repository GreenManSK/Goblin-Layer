using System;
using System.Collections.Generic;
using Events;
using Objects.Golbin;
using Services;
using UI.Controllers;
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
        public List<GoblinController> goblins = new List<GoblinController>();
        public DateUiController dateUi;

        private GameObject _arrow;
        private int _activeIndex;

        private void Start()
        {
            dateUi.gameObject.SetActive(false);
        }

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
            dateUi.gameObject.SetActive(true);
            GameController.Instance.Input.Player.Move.performed += ChangeActive;
        }

        private void StopDate()
        {
            Destroy(_arrow);
            dateUi.gameObject.SetActive(false);
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
            dateUi.SetGoblin(goblins[_activeIndex].data);
        }

        private int GoblinSorter(GoblinController a, GoblinController b)
        {
            var aPos = a.gameObject.transform.position;
            var bPos = b.gameObject.transform.position;
            return Mathf.Approximately(aPos.x, bPos.x) ? aPos.y.CompareTo(bPos.y) : aPos.x.CompareTo(bPos.x);
        }
    }
}