using System;
using System.Collections.Generic;
using Events;
using Services;
using UnityEngine;

public class GameController : MonoBehaviour, IEventListener<GoblinActivationEvent>, IEventListener<GoblinDeathEvent>
{
    public static GameController Instance => _instance;
    private static GameController _instance;

    public PlayerControlls Input => _input ??= new PlayerControlls();
    private PlayerControlls _input;

    private HashSet<GameObject> activeGoblins = new HashSet<GameObject>();

    public GameController()
    {
        _instance = this;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        GameEventSystem.Subscribe<GoblinActivationEvent>(this);
        GameEventSystem.Subscribe<GoblinDeathEvent>(this);
    }

    private void OnDisable()
    {
        GameEventSystem.Unsubscribe<GoblinActivationEvent>(this);
        GameEventSystem.Unsubscribe<GoblinDeathEvent>(this);
    }

    public void OnEvent(GoblinActivationEvent @event)
    {
        activeGoblins.Add(@event.Object);
    }

    public void OnEvent(GoblinDeathEvent @event)
    {
        activeGoblins.Remove(@event.Object);
    }
}