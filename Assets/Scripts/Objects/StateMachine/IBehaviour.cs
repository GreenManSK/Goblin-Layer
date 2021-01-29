using System;
using UnityEngine;

namespace Objects.StateMachine
{
    public interface IBehaviour<in TC, in TS> where TC : MonoBehaviour where TS : Enum
    {
        void OnTransitionIn(TC context);
        void OnTransitionOut();
        void OnUpdate();
        void OnFixedUpdate();
        bool IsState(TS state);
    }
}