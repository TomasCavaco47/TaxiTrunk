using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_WaypointNavigator : MonoBehaviour
{
    NPC_NavigatorController _controller;
    [SerializeField] NPC_Waypoint _currentWaypoint;
    int _direction;

    public NPC_Waypoint CurrentWaypoint { get => _currentWaypoint; set => _currentWaypoint = value; }

    private void Awake()
    {
        _controller = GetComponent<NPC_NavigatorController>();
    }
    private void Start()
    {
        _direction = Random.Range(0, 2);
        _controller.SetDestination(CurrentWaypoint.GetPosition());
    }
    private void Update()
    {
        if(_controller.ReachedDestination)
        {
            bool shouldBranch=false;
            if(CurrentWaypoint.Branches==null)
            {

            }
            else
            {
                if(CurrentWaypoint.Branches != null && CurrentWaypoint.Branches.Count>0)
                {
                    shouldBranch= Random.Range(0f,1f) <= CurrentWaypoint.BranchRatio ? true :false;
                }

            }

            if (shouldBranch)
            {
                CurrentWaypoint = CurrentWaypoint.Branches[Random.Range(0, CurrentWaypoint.Branches.Count)];
            }
            else
            {
              
                if (_direction == 0)
                {
                    if(CurrentWaypoint.NetxWaypoint!=null)
                    {
                        CurrentWaypoint = CurrentWaypoint.NetxWaypoint;

                    }
                    else
                    {
                        CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;
                        _direction = 1;
                    }
                }
                else if (_direction == 1)
                {
                    if (CurrentWaypoint.NetxWaypoint != null)
                    {
                        CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;

                    }
                    else
                    {
                        CurrentWaypoint = CurrentWaypoint.NetxWaypoint;
                        _direction = 0;
                    }
                }
            }

            _controller.SetDestination(CurrentWaypoint.GetPosition());
            
        }
    }
}
