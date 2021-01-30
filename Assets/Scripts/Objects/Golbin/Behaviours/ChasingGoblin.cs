using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

namespace Objects.Golbin.Behaviours
{
    public class ChasingGoblin : AGoblinBehaviour
    {
        public static readonly int Animation = Animator.StringToHash("Move");

        private IEnumerator _coroutine;
        private Path _path;
        private int _currentWaypoint = 0;
        private bool _reachedEndOfPath = false;
        private Vector2 _movement;

        public override void OnTransitionIn(GoblinController context)
        {
            base.OnTransitionIn(context);
            _coroutine = UpdatePath(Context.pathUpdateTimeInS);
            Context.StartCoroutine(_coroutine);
        }

        public override void OnTransitionOut()
        {
            Context.StopCoroutine(_coroutine);
        }

        private IEnumerator UpdatePath(float waitTime)
        {
            while (true)
            {
                if (Context.Seeker.IsDone())
                {
                    Context.Seeker.StartPath(Context.Rigidbody2D.position, Context.Target.position, OnPathComplete);
                }

                yield return new WaitForSeconds(waitTime);
            }

            // ReSharper disable once IteratorNeverReturns
        }

        private void OnPathComplete(Path p)
        {
            if (p.error)
            {
                Debug.LogError($"Error while computing path {p.error}", Context.gameObject);
                return;
            }

            Context.Animator.SetTrigger(Animation);
            _path = p;
            _currentWaypoint = 0;
        }

        public override void OnUpdate()
        {
            if (!Mathf.Approximately(_movement.x, 0))
            {
                Context.Sprite.flipX = _movement.x < 0;
            }
        }

        public override void OnFixedUpdate()
        {
            if (_path == null)
            {
                return;
            }

            _reachedEndOfPath = _currentWaypoint >= _path.vectorPath.Count;
            if (_reachedEndOfPath)
            {
                return;
            }

            var position = Context.Rigidbody2D.position;
            var target = (Vector2) _path.vectorPath[_currentWaypoint];

            var avoidanceDirection = Vector2.zero;
            if (Context.goblinsNear.Count > 0)
            {
                foreach (var near in Context.goblinsNear)
                {
                    avoidanceDirection += (position - (Vector2)near.transform.position).normalized;
                }
                avoidanceDirection /= Context.goblinsNear.Count;
                // _movement = Time.fixedDeltaTime * Context.moveSpeed * move;
            }
            
            var direction = (target - position).normalized;
            var move = (direction + avoidanceDirection).normalized;
            _movement = Time.fixedDeltaTime * Context.moveSpeed * move;

            Context.Rigidbody2D.MovePosition(position + _movement);

            var distance = Vector2.Distance(position, target);
            if (distance < Context.nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }

        public override bool IsState(GoblinState state)
        {
            return state == GoblinState.Chasing;
        }
    }
}