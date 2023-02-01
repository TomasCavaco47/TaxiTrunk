using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LineControler : MonoBehaviour
{
    private LineRenderer _lr;
    private NavMeshAgent _agent;
    private Rigidbody _rb;
    //[SerializeField] Transform[] _points;
    [SerializeField] List<Vector3> _point = new List<Vector3>();
    [SerializeField] Transform _goalT;
    [SerializeField] Transform _player;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _lr = GetComponent<LineRenderer>();
        _agent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
      
        _agent.SetDestination(_goalT.position);
        DisplayLineDestination();
    }
    private void DisplayLineDestination()
    {
        _point= _agent.path.corners.ToList();
        _lr.positionCount = _point.Count;
        _lr.SetPositions(_agent.path.corners);
   
        //for (int i = 0; i < _point.Count; i++)
        //{
        //    _lr.SetPosition(i, _point[i]);

        //}
        Debug.Log(_agent.path.corners.Length);            
    }

}
