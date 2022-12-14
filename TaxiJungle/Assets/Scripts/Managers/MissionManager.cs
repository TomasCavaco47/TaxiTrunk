using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MissionManager : MonoBehaviour
{
    [SerializeField] UiManager _uiManager;
    [SerializeField] CarControllerScript _playerCar;
    
    [SerializeField] List<Transform> _places;
    [SerializeField] List<Client> _clients;
    [SerializeField] Client _currentClient;

    [Header("MissionConfirmations")]
    [SerializeField] Mission _activeMission;

    [SerializeField]private float _timer;
   [SerializeField] private bool _startTimer;

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
        // _activeMission = _clients[0].MissionsArcOne[0];

        MissionStarted = true;


    }
    #region QuickMission
    public void StartQuickMissions()
    {

        _activeMission.Origin = _places[Random.Range(0, _places.Count)];

        Debug.Log("Pick me up in " + _activeMission.Origin.name);
        SpawnGoal();
        _uiManager.GpsOn(_activeMission.Origin);
        MissionStarted = true;
    }
    
    void SpawnGoal()
    {
        _activeMission.Destination = _places[Random.Range(0, _places.Count)];
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
        
            Debug.Log(Vector3.Distance(_playerCar.transform.position, _activeMission.Origin.position));
            if (Vector3.Distance(_playerCar.transform.position, _activeMission.Origin.position) <= 5 && _playerCar.CurrentSpeed == 0 && _clientPickedUp == false)
            {

                Debug.Log("Take me to " + _activeMission.Destination.name);
                _uiManager.GpsOn(_activeMission.Destination);


            _timer = ((int)(Vector3.Distance(_playerCar.transform.position, _activeMission.Destination.position)) / 4);
            if(_activeMission.DialoguesPickUp.Length>0)
            {
                Time.timeScale = _slowMotionTimeScale;
                Time.fixedDeltaTime = _slowMotionTimeScale * _slowMotionTimeScale;
                _uiManager.Dialogue(_activeMission.DialoguesPickUp[_dialogueCounter].Sprite, _activeMission.DialoguesPickUp[_dialogueCounter].Text);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    _dialogueCounter++;

                }
                if (_dialogueCounter == _activeMission.DialoguesPickUp.Length)
                {
                    _uiManager.CloseDialogue();
                    _clientPickedUp = true;
                    _startTimer = true;
                    Time.timeScale = _startTimeScale;
                    Time.fixedDeltaTime = _startFixedDeltaTime;
                }
            }
            else
            {
                _clientPickedUp = true;
                _startTimer = true;
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
        
        for (int i = 0; i < _clients.Count; i++)
        {
            if (_clients[i].MissionsArcOne.Count > 0)
            {
                Debug.Log("mostrar");

            }

        }


    }
    #endregion


}
