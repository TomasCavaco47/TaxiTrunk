using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] UiManager _uiManager;
    [SerializeField] CarControllerScript _playerCar;
    [SerializeField] List<Transform> _places;
    [SerializeField] ABMission _mission;
    [SerializeField] bool _missionStarted;
    [SerializeField] Transform _startMissionPlace;
    [SerializeField] Transform _endMissionPlace;
    GameManager _gameManager;
    float _timer;

    private void Start()
    {
        _gameManager = GameManager.instance;
        
    }
    private void Update()
    {
        if(_missionStarted)
        {
            if(Vector3.Distance(_playerCar.transform.position,_startMissionPlace.position) <= 10 && _playerCar.CurrentSpeed ==0)
            {
                _uiManager.ShowTimer((int)(Vector3.Distance(_playerCar.transform.position, _startMissionPlace.position)) / 4);
            }
        }
    }
    public void StartQuickMissions()
    {
        _startMissionPlace=_places[Random.Range(0, _places.Count)];
        Debug.Log("Pick me up in " + _startMissionPlace.name);
        SpawnGoal();
       
        //_mission = new ABMission(_missionPoints[0].position);
        //_mission.StartMission();
        //Debug.Log("start quick");
    }
    void  SpawnGoal()
    {
        _endMissionPlace = _places[Random.Range(0, _places.Count)];
        float distance = Vector3.Distance(_startMissionPlace.position, _endMissionPlace.position);
        if (distance < 55)
        {
            SpawnGoal();

        }
        else
        {
           // _gameManager.UiManager.ShowTimer((int)_distance / 4);
            //gameObject.transform.position = _goalLocalization[_loc].position;
        }
    }

    public void StartStoryMissions()
    {
        
    }
}
