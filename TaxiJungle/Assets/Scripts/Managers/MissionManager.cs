using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetPlacesAndClients
{
    [SerializeField] Transform _place;
    [SerializeField] List<Transform> _places;
    [SerializeField] GameObject _client;
    [SerializeField] List<Client> _clients;

    public Transform Place { get => _place; set => _place = value; }
    public List<Transform> Places { get => _places; set => _places = value; }
    public GameObject Client { get => _client; set => _client = value; }
    public List<Client> Clients { get => _clients; set => _clients = value; }
}

public class MissionManager : MonoBehaviour
{
    [Header("Refferences")]
    [SerializeField] UiManager _uiManager;
    [SerializeField] CarControllerTest _playerCar;

    [Header("Data")]

    [SerializeField] GetPlacesAndClients _placesAndClients;
  

    [Header("MissionConfirmations")]
    [SerializeField] Client _currentClient;
    [SerializeField] Mission _activeMission;

    [SerializeField] float _timer;
    [SerializeField] bool _startTimer;

    [SerializeField] bool _missionStarted;
    [SerializeField] bool _clientPickedUp;
    float _startTimeScale;
    [SerializeField] float _slowMotionTimeScale;
    float _startFixedDeltaTime;
    private int _dialogueCounter;

    public bool MissionStarted { get => _missionStarted; set => _missionStarted = value; }
    private void Awake()
    {
        _uiManager = UiManager.instance;
        _startFixedDeltaTime = Time.fixedDeltaTime;
        _startTimeScale = Time.timeScale;
    }
    private void Start()
    {
       

    }
    private void OnValidate()
    {
        for (int i = 0; i < _placesAndClients.Place.childCount; i++)
        {
            if(_placesAndClients.Places.Contains(_placesAndClients.Place.GetChild(i))==false)
            {
                _placesAndClients.Places.Add(_placesAndClients.Place.GetChild(i));
            }
            

        }
        for (int i = 0; i < _placesAndClients.Client.transform.childCount; i++)
        {
            if (_placesAndClients.Clients.Contains(_placesAndClients.Client.transform.GetChild(i).GetComponent<Client>()) == false)
            {
                _placesAndClients.Clients.Add(_placesAndClients.Client.transform.GetChild(i).GetComponent<Client>());
            }

        }
    }
    private void Update()
    {
       
        if(_activeMission!=null)
        {

        }

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
            if (_startTimer)
            {
                Timer();
            }

        }
    }
    public void StartStoryMissions(Client client)
    {
        _currentClient = client;

        _activeMission = _currentClient.MissionsArcOne[0];




        _uiManager.GpsOn(_activeMission.Origin);

        MissionStarted = true;


    }
    #region QuickMission
    public void StartQuickMissions()
    {

        _activeMission.Origin = _placesAndClients.Places[Random.Range(0, _placesAndClients.Places.Count)];

        Debug.Log("Pick me up in " + _activeMission.Origin.name);
        SpawnGoal();
        _uiManager.GpsOn(_activeMission.Origin);
        MissionStarted = true;
    }
    
    void SpawnGoal()
    {
        _activeMission.Destination = _placesAndClients.Places[Random.Range(0, _placesAndClients.Places.Count)];
        float distance = Vector3.Distance(_activeMission.Origin.position, _activeMission.Destination.position);
        if (distance < 55)
        {
            SpawnGoal();

        }
    }
    #endregion
    #region Cliente Origin and Destination Checkers
    void ClientPickUp()
    {
        switch (_activeMission.MissionType)
        {
            case MissionType.AtoB:
                if (Vector3.Distance(_playerCar.transform.position, _activeMission.Origin.position) <= 5 && _playerCar.CurrentSpeed == 0 && _clientPickedUp == false)
                {
                    _playerCar.CanMove = false;
                    _uiManager.GpsOn(_activeMission.Destination);
                    _timer = ((int)(Vector3.Distance(_playerCar.transform.position, _activeMission.Destination.position)) / 4);
                    if (CheckDialog(_activeMission.DialoguesPickUp))
                    {
                        StartDialogue();
                    }            
                }
                break;
            case MissionType.Tetris:
                _playerCar.CanMove = false;
                _uiManager.GpsOn(_activeMission.Destination);
                _timer = ((int)(Vector3.Distance(_playerCar.transform.position, _activeMission.Destination.position)) / 4);
                if (CheckDialog(_activeMission.DialoguesPickUp))
                {
                    StartDialogue();
                }
                break;
            case MissionType.Coffee:
                break;

        }
                        
    }
    bool CheckDialog(Dialogue[] dialogues)
    {
        if(dialogues.Length == 0)
        {
            _clientPickedUp = true;
            _startTimer = true;
            _playerCar.CanMove = true;

            return false;
        }
        else
        {
            return true;
        }
    }
    void StartDialogue()
    {
        _uiManager.Dialogue(_activeMission.DialoguesPickUp[_dialogueCounter].Sprite, _activeMission.DialoguesPickUp[_dialogueCounter].Text);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _dialogueCounter++;
        }
        if (_dialogueCounter == _activeMission.DialoguesPickUp.Length)
        {
            _uiManager.CloseDialogue();
            if(_activeMission.MissionType != MissionType.Tetris)
            {
            _clientPickedUp = true;
            _startTimer = true;
            _playerCar.CanMove = true;

            }
        }
    }
    void ClientDestination()
    {
       
        Debug.Log(Vector3.Distance(_playerCar.transform.position, _activeMission.Destination.position));
        if (Vector3.Distance(_playerCar.transform.position, _activeMission.Destination.position) <= 5 && _playerCar.CurrentSpeed == 0)
        {
            _uiManager.GpsOff();
            _uiManager.ShowTimer(false, 0);
            _clientPickedUp = false;
            MissionStarted = false;
            _activeMission = null;
            _startTimer = false;
            _currentClient.MissionsArcOne.RemoveAt(0);
            _currentClient = null;
            //_clients[0].MissionsArcOne.RemoveAt(0);           
        }           
    }
    #endregion
    #region timerLogic
    void Timer()
    {
        _timer = _timer - Time.deltaTime;
        _uiManager.ShowTimer(true, _timer);
        if (_timer <= 0)
        {
            // quando chega a 0 o tempo aumenta um pouco mas o reward do player diminui
            _uiManager.ShowTimer(false,0);
            _uiManager.GpsOff();

            Debug.Log("lose");
            MissionStarted = false;
            _startTimer = false;
            _clientPickedUp = false;

        }
      
        
        
    }
    #endregion


}
