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
    [SerializeField] bool _isOnRoad = false;
    [SerializeField] GameObject _line2;



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

        if (timePassed >= refreshTimer /*&& _isOnRoad == true*/)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, 1))
            {
                NavMesh.CalculatePath(hit.position, _goalT.position, 1, _path);
                Agent.SetPath(_path);

            }
            //timePassed = 0;
            //NavMesh.CalculatePath(transform.position, _goalT.position, 1, _path);
            //Agent.SetPath(_path);
        }

        //else if (_isOnRoad == false)
        //{
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Roads"))
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






