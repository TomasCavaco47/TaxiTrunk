using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpwanEvent : MonoBehaviour
{
    Client _client;
    [SerializeField] Transform _goalLocalization;
    [SerializeField] GameObject _goal;

    // Start is called before the first frame update
    private void Awake()
    {
        _client = GetComponent<Client>();
    }

    void OnEnable()
    {
        //_client.GoalSpwan += GoalSpawn;
    }
    void OnDisable()
    {
        //_client.GoalSpwan -= GoalSpawn;
    }

    void GoalSpawn()
    {
        GameObject goal = Instantiate(_goal);
        goal.transform.position = _goalLocalization.position;
    }
}
