using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LineAI : MonoBehaviour
{
    [SerializeField] Transform _goalT;
    [SerializeField] private float refreshTimer = 0;
    [SerializeField] Transform player;
     public bool _isOnRoad;

    private NavMeshAgent _agent;
    private NavMeshPath _path;

    private float timePassed = 0;

    public NavMeshAgent Agent { get => _agent; set => _agent = value; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath(); //Init
       
    }

    private void Update()
    {
        transform.position = player.position;
        timePassed += Time.deltaTime;

        if (timePassed >= refreshTimer && _isOnRoad == true)
        {

            timePassed = 0;
            NavMesh.CalculatePath(transform.position, _goalT.position, 3, _path);
            Agent.SetPath(_path);
            Debug.Log(_path.status);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Roads"))
        {
            _isOnRoad = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Roads"))
        {
            _isOnRoad = false;

        }
    }




}
