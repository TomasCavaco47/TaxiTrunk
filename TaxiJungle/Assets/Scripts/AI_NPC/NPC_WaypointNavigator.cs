using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_WaypointNavigator : MonoBehaviour
{
    NPC_NavigatorController _controller;
    [SerializeField] NPC_Waypoint _currentWaypoint;

    private void Awake()
    {
        _controller = GetComponent<NPC_NavigatorController>();
    }
    private void Start()
    {
        _controller.SetDestination(_currentWaypoint.GetPosition());
    }
    private void Update()
    {
        if(_controller.ReachedDestination)
        {
            _currentWaypoint = _currentWaypoint.NetxWaypoint;
            _controller.SetDestination(_currentWaypoint.GetPosition());
        }
    }
}
