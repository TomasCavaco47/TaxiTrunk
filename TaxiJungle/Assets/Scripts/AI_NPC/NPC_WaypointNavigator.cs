using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_WaypointNavigator : MonoBehaviour
{
    NPC_NavigatorController _controller;
    [SerializeField] NPC_Waypoint _currentWaypoint;
    [SerializeField]int _direction;
    [SerializeField] bool _canGoToCrosswalk=true;
    [SerializeField] bool _onTheCrossWalk=false;
    int i;

    public NPC_Waypoint CurrentWaypoint { get => _currentWaypoint; set => _currentWaypoint = value; }

    private void Awake()
    {
        _controller = GetComponent<NPC_NavigatorController>();
        _onTheCrossWalk = false;
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

            if (CurrentWaypoint?.Branches != null && CurrentWaypoint.Branches.Count > 0)
            {
                if (CurrentWaypoint.HasAnEntryBranch)
                {
                    if (_canGoToCrosswalk)
                    {
                        shouldBranch = Random.Range(0f, 1f) <= CurrentWaypoint.BranchRatio ? true : false;
                    }
                }
                else
                {
                    shouldBranch = Random.Range(0f, 1f) <= CurrentWaypoint.BranchRatio ? true : false;

                }
            }



            if (shouldBranch)
            {          
                NPC_Waypoint previousWaypoint = CurrentWaypoint;
                CurrentWaypoint = CurrentWaypoint.Branches[Random.Range(0, CurrentWaypoint.Branches.Count)];
                _canGoToCrosswalk = false;
                _onTheCrossWalk = true;
                 ChangeDirection();
                if(previousWaypoint.IsAExitBranch)
                {
                    OnExitCrossRoad();
                }
                i = 0;


            }
            else
            {
                bool shoulLookAtFone = Random.Range(0f, 1f) < 0.02f;
                if (shoulLookAtFone&& _onTheCrossWalk ==false)
                {
                    StartCoroutine(LookAtFone());
                }
                if (_direction == 0)
                {
                    if (CurrentWaypoint.NetxWaypoint != null)
                    {
                        CurrentWaypoint = CurrentWaypoint.NetxWaypoint;

                    }
                    else
                    {
                        if (_direction == 0)
                        {
                            if (CurrentWaypoint.NetxWaypoint != null)
                            {
                                CurrentWaypoint = CurrentWaypoint.NetxWaypoint;

                            }
                            else
                            {
                                if (CurrentWaypoint?.Branches != null && CurrentWaypoint.Branches.Count > 0)
                                {
                                    Debug.Log("1");
                                    CurrentWaypoint = CurrentWaypoint.Branches[0];
                                }
                                else
                                { 
                                    CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;
                                    _direction = 1;
                                
                                }

                            }
                        }
                        if (_direction == 1)
                        {
                            if (CurrentWaypoint.PreviousWaypoint != null)
                            {
                                CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;

                            }
                            else
                            {
                                if (CurrentWaypoint?.Branches != null && CurrentWaypoint.Branches.Count > 0)
                                {
                                    CurrentWaypoint = CurrentWaypoint.Branches[0];
                                }
                                else
                                {
                                    CurrentWaypoint = CurrentWaypoint.NetxWaypoint;
                                    _direction = 0;

                                }
                            }
                        }                    

                    }
                }
                if (_direction == 1)
                {
                    if (CurrentWaypoint.PreviousWaypoint != null)
                    {
                        CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;

                    }
                    else
                    {
                        CurrentWaypoint = CurrentWaypoint.NetxWaypoint;
                        _direction = 0;
                    }
                }
                i++;
                if (i == 6)
                {
                    _canGoToCrosswalk = true;
                }
            }

            _controller.SetDestination(CurrentWaypoint.GetPosition());
            
        }
    }
    public void ChangeDirection()
    {
        if (_currentWaypoint.BeginingCrosswalk)
        {
            switch (_direction)
            {
                case 0:
                    _direction = 0;
                    break;
                case 1:
                    _direction = 0;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_direction)
            {
                case 0:
                    _direction = 1;
                    break;
                case 1:
                    _direction = 1;
                    break;
                default:
                    break;
            }
        }
        
    }
    public void OnExitCrossRoad()
    {
        _direction = Random.Range(0, 2);
        _onTheCrossWalk = false;
    }
    IEnumerator LookAtFone()
    {
        float _normalSpeed = _controller.MovementSpeed;
        _controller.MovementSpeed = 0;
        //Animation
        yield return new WaitForSeconds(Random.Range(8,20));
        _controller.MovementSpeed = _normalSpeed;
    }
}
