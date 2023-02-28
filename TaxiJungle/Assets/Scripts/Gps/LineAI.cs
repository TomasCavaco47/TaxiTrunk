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
  



    private NavMeshAgent _agent;
    private NavMeshPath _path;

    private float timePassed = 0;

    public NavMeshAgent Agent { get => _agent; set => _agent = value; }
    public Transform Player { get => player; set => player = value; }
    public Transform GoalT { get => _goalT; set => _goalT = value; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath(); //Init
       
    }

    private void Update()
    {
        transform.position = Player.position;
        timePassed += Time.deltaTime;

        if (timePassed >= refreshTimer /*&& _isOnRoad == true*/)
        {
            NavMeshHit hit;
            
            if (NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas))
            {
                NavMesh.CalculatePath(hit.position, GoalT.position, NavMesh.AllAreas, _path);
                Agent.SetPath(_path);
                timePassed = 0;

            }
            //
            //NavMesh.CalculatePath(transform.position, _goalT.position, 1, _path);
            //Agent.SetPath(_path);
        }

        //else if (_isOnRoad == false)
        //{NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, 4)
        //    //carro ate a estrada
        //    //ativar da estrada ate ao destino 
        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, 1))
        //    {
        //        NavMesh.CalculatePath(hit.position, _goalT.position, 1, _path);
        //        Agent.SetPath(_path);

        //    }
        //}



    }



}






