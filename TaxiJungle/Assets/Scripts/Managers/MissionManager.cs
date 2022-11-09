using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] UiManager _uiManager;
    [SerializeField] CarControllerScript _playerCar;
    
    [SerializeField] List<Transform> _places;
    [SerializeField] List<Client> clients;

    [Header("MissionPlaces")]
    [SerializeField] Transform _startMissionPlace;
    [SerializeField] Transform _endMissionPlace;

    [Header("MissionConfirmations")]
    [SerializeField] Mission _quickMission;

    [SerializeField] bool _missionStarted;
    [SerializeField] bool _clientPickedUp;

    public bool MissionStarted { get => _missionStarted; set => _missionStarted = value; }

    private void Start()
    {
        
    }
    private void Update()
    {
       
        if(MissionStarted)
        {
            if(_clientPickedUp ==false)
            {
                ClientPickUp();
            }
            else
            {
                ClientDestination();
            }

        }
    }
    public void StartQuickMissions()
    {
        //_startMissionPlace=_places[Random.Range(0, _places.Count)];

       
        _quickMission.Origin = _places[Random.Range(0, _places.Count)];
        Debug.Log("Pick me up in " + _quickMission.Origin.name);
        SpawnGoal();
        MissionStarted = true;
    }
    public void StartStoryMissions()
    {
        
    }
    void SpawnGoal()
    {
        _quickMission.Destination = _places[Random.Range(0, _places.Count)];
        float distance = Vector3.Distance(_quickMission.Origin.position, _quickMission.Destination.position);
        if (distance < 55)
        {
            SpawnGoal();

        }
    }
    void ClientPickUp()
    {
        Debug.Log(Vector3.Distance(_playerCar.transform.position, _quickMission.Origin.position));
        if (Vector3.Distance(_playerCar.transform.position, _quickMission.Origin.position) <= 5 && _playerCar.CurrentSpeed == 0 && _clientPickedUp == false)
        {
            Debug.Log("Take me to " + _quickMission.Destination.name);
            _uiManager.ShowTimer((int)(Vector3.Distance(_playerCar.transform.position, _quickMission.Destination.position)) / 4);
            _clientPickedUp = true;
        }
    }
    void ClientDestination()
    {
        Debug.Log(Vector3.Distance(_playerCar.transform.position, _quickMission.Destination.position));
        if (Vector3.Distance(_playerCar.transform.position, _quickMission.Destination.position) <= 5 && _playerCar.CurrentSpeed == 0)
        {
            _uiManager.ShowTimer(0);
            _clientPickedUp = false;
            MissionStarted = false;
            _startMissionPlace = null;
            _endMissionPlace = null;
            _quickMission = null;
        }
    }
}
