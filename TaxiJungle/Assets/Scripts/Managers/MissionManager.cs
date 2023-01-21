using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MissionData
{
    [SerializeField] List<Mission> _arcOneMissions;

    

    public List<Mission> ArcOneMissions { get => _arcOneMissions; set => _arcOneMissions = value; }
}

public class MissionManager : MonoBehaviour
{
    [SerializeField] MissionData _missions;

    [Header("Refferences")]
    [SerializeField] UiManager _uiManager;
    [SerializeField] CarControllerTest _playerCar;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _PuzzleCanvas;
    public static MissionManager instance;

    [Header("Data")]
    [SerializeField] DataBase _database;
    private List<Transform> _places;
    private List<Client> _clients;
    [Header("MissionConfirmations")]
    //[SerializeField] Client _currentClient;
    [SerializeField] Mission _activeMission;
    [SerializeField] bool _puzzleCompleted;

    [SerializeField] float _timer;
    [SerializeField] bool _startTimer=false;

    [SerializeField] bool _missionStarted;
    [SerializeField] bool _clientPickedUp;
    float _startTimeScale;
    [SerializeField] float _slowMotionTimeScale;
    float _startFixedDeltaTime;
    [SerializeField]private int _dialogueCounter;

    public bool MissionStarted { get => _missionStarted; set => _missionStarted = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
        }
        _uiManager = UiManager.instance;
        _startFixedDeltaTime = Time.fixedDeltaTime;
        _startTimeScale = Time.timeScale;
    }
  
    private void OnValidate()
    {
        _places = _database.Places;
        _clients = _database.Clients;
        for (int i = 0; i < _missions.ArcOneMissions.Count; i++)
        {
            if (_missions.ArcOneMissions.Count == 0)
            {
                break;
            }
            else
            {
                for (int a = 0; a < _missions.ArcOneMissions[i].DialoguesPickUp.Length; a++)
                {
                    if (_missions.ArcOneMissions[i].DialoguesPickUp.Length != 0)
                    {
                        switch (_missions.ArcOneMissions[i].DialoguesPickUp[a].WhosTalking)
                        {
                            case WhosTalking.Client:
                                _missions.ArcOneMissions[i].DialoguesPickUp[a].Sprite = _missions.ArcOneMissions[i].Client.ClientSprite;
                                break;
                            case WhosTalking.Vin:
                                _missions.ArcOneMissions[i].DialoguesPickUp[a].Sprite = _database.VinSprite;
                                break;
                            default:
                                break;
                        }
                    }


                    //if (_missions.ArcOneMissions[i].DialoguesInMission.Length != 0)
                    //{
                    //    switch (_missions.ArcOneMissions[i].DialoguesInMission[a].WhosTalking)
                    //    {
                    //        case WhosTalking.Client:
                    //            _missions.ArcOneMissions[i].DialoguesInMission[a].Sprite = _clientSprite;
                    //            break;
                    //        case WhosTalking.Vin:
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}


                    //if (_missions.ArcOneMissions[i].DialoguesDestination.Length != 0)
                    //{
                    //    switch (_missions.ArcOneMissions[i].DialoguesDestination[a].WhosTalking)
                    //    {
                    //        case WhosTalking.Client:
                    //            _missions.ArcOneMissions[i].DialoguesDestination[a].Sprite = _clientSprite;
                    //            break;
                    //        case WhosTalking.Vin:
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}

                }
            }
        }
    }
 
    private void Update()
    {
       

        if (_activeMission!=null)
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
    public void StartStoryMissions()
    {
        _activeMission = _missions.ArcOneMissions[0];
        _uiManager.GpsOn(_activeMission.Origin);
        MissionStarted = true;
    }
    #region QuickMission
    public void StartQuickMissions()
    {

       /////////////////// _activeMission.Origin = Places[Random.Range(0,Places.Count)];

        Debug.Log("Pick me up in " + _activeMission.Origin.name);
        SpawnGoal();
        _uiManager.GpsOn(_activeMission.Origin);
        MissionStarted = true;
    }
    
    void SpawnGoal()
    {

        //logica do quick mission
       // _activeMission.Destination = PlacesAndClients.Places[Random.Range(0, PlacesAndClients.Places.Count)];
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _dialogueCounter++;
        }
        if (_dialogueCounter == _activeMission.DialoguesPickUp.Length)
        {
            _uiManager.CloseDialogue();
            switch (_activeMission.MissionType)
            {
                case MissionType.AtoB:
                    StartTimer();
                    break;

                case MissionType.Tetris:
                    // HERE 
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Puzzle", LoadSceneMode.Additive); break;

                case MissionType.Coffee:
                    StartTimer();
                    break;

              
            }
            _dialogueCounter++;
        }
        if (_dialogueCounter < _activeMission.DialoguesPickUp.Length)
        {
            _uiManager.Dialogue(_activeMission.DialoguesPickUp[_dialogueCounter].Sprite, _activeMission.DialoguesPickUp[_dialogueCounter].Text);

        }
    }
    public  void StartTimer()
    {
        _clientPickedUp = true;
        _startTimer = true;
        _playerCar.CanMove = true;
        Timer();
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
            _missions.ArcOneMissions.RemoveAt(0);
            //send to ui next mission info
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

        //for (int i = 0; i < _clients.Count; i++)
        //{
        //    if (_clients[i].MissionsArcOne.Count > 0)
        //    {
        //        Debug.Log("mostrar");

        //    }

        //}


    }
    #endregion
    


}
