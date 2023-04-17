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
    [SerializeField] GameObject _PuzzleCanvas;
    public static MissionManager instance;

    [Header("Data")]
    [SerializeField] DataBase _database;
    private List<GameObject> _places;
    private List<Client> _clients;
    [Header("MissionConfirmations")]
    [SerializeField] Mission _activeMission;
    [SerializeField] bool _puzzleCompleted;
    [SerializeField] private bool _isInDialogue;

    [SerializeField] float _timer;
    [SerializeField] bool _startTimer=false;

    [SerializeField] bool _missionStarted;
    [SerializeField] bool _clientPickedUp;
    [SerializeField] bool _clientReachedDestination;
    bool isQuickMission=false;
    [SerializeField] float _slowMotionTimeScale;

    float _startTimeScale;
    float _startFixedDeltaTime;
    [SerializeField]private int _dialogueCounter;

    public bool MissionStarted { get => _missionStarted; set => _missionStarted = value; }
    public MissionData Missions { get => _missions; set => _missions = value; }
    public List<GameObject> Places { get => _places; set => _places = value; }
    public CarControllerTest PlayerCar { get => _playerCar; set => _playerCar = value; }
    public bool IsInDialogue { get => _isInDialogue;}

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
        Places = _database.Places;
        _uiManager = UiManager.instance;
        _startFixedDeltaTime = Time.fixedDeltaTime;
        _startTimeScale = Time.timeScale;
    }
    private void Start()
    {
        if(!PlayerCar)
        {
            PlayerCar=GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>();
        }
    }
    private void OnValidate()
    {
        Places = _database.Places;
        _clients = _database.Clients;
        for (int i = 0; i < Missions.ArcOneMissions.Count; i++)
        {
            if (Missions.ArcOneMissions.Count == 0)
            {
                break;
            }
            else
            {
                for (int a = 0; a < Missions.ArcOneMissions[i].DialoguesPickUp.Length; a++)
                {
                    if (Missions.ArcOneMissions[i].DialoguesPickUp.Length != 0)
                    {
                        switch (Missions.ArcOneMissions[i].DialoguesPickUp[a].WhosTalking)
                        {
                            case WhosTalking.Client:
                                Missions.ArcOneMissions[i].DialoguesPickUp[a].Sprite = Missions.ArcOneMissions[i].Client.ClientSprite;
                                break;
                            case WhosTalking.Vin:
                                Missions.ArcOneMissions[i].DialoguesPickUp[a].Sprite = _database.VinSprite;
                                break;
                            default:
                                break;
                        }
                    }
                }
                //for (int a = 0; a < Missions.ArcOneMissions[i].DialoguesInMission.Length; a++)
                //{
                //    if (Missions.ArcOneMissions[i].DialoguesInMission.Length != 0)
                //    {
                //        switch (Missions.ArcOneMissions[i].DialoguesInMission[a].WhosTalking)
                //        {
                //            case WhosTalking.Client:
                //                Missions.ArcOneMissions[i].DialoguesInMission[a].Sprite = Missions.ArcOneMissions[i].Client.ClientSprite;
                //                break;
                //            case WhosTalking.Vin:
                //                Missions.ArcOneMissions[i].DialoguesInMission[a].Sprite = _database.VinSprite;

                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //}
                for (int a = 0; a < Missions.ArcOneMissions[i].DialoguesDestination.Length; a++)
                {
                    if (Missions.ArcOneMissions[i].DialoguesDestination.Length != 0)
                    {
                        switch (Missions.ArcOneMissions[i].DialoguesDestination[a].WhosTalking)
                        {
                            case WhosTalking.Client:
                                Missions.ArcOneMissions[i].DialoguesDestination[a].Sprite = Missions.ArcOneMissions[i].Client.ClientSprite;
                                break;
                            case WhosTalking.Vin:
                                Missions.ArcOneMissions[i].DialoguesDestination[a].Sprite = _database.VinSprite;

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
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
    public void StartStoryMissions()
    {
        _activeMission = Missions.ArcOneMissions[0];
        _uiManager.GpsOn(_activeMission.Origin.transform);
        _activeMission.Origin.GetComponent<MeshRenderer>().enabled = true;
        MissionStarted = true;
    }
    #region QuickMission
    public void StartQuickMissions(int i)
    {
        _activeMission = new Mission();
        _activeMission.Origin = Places[Random.Range(0, Places.Count)];
        _activeMission.Origin.GetComponent<MeshRenderer>().enabled = true;
                SpawnDestination();
        int client = Random.Range(0, _database.Clients.Count);
        switch (i)
        {
            case 0:
                _activeMission.MissionType = MissionType.AtoB;
                _activeMission.DialoguesPickUp = new Dialogue[2];
                _activeMission.DialoguesPickUp[0].Text = "Take me to the " + _activeMission.Destination.name;
                _activeMission.DialoguesPickUp[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesPickUp[1].Text = "Here we go!";
                _activeMission.DialoguesPickUp[1].Sprite = _database.VinSprite;
                _activeMission.DialoguesDestination = new Dialogue[2];
                _activeMission.DialoguesDestination[0].Text = "Thank you!";
                _activeMission.DialoguesDestination[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesDestination[1].Text = "Anytime!";
                _activeMission.DialoguesDestination[1].Sprite = _database.VinSprite;
                break; 
            case 1:
                _activeMission.MissionType = MissionType.Coffee;
                _activeMission.DialoguesPickUp = new Dialogue[2];
                _activeMission.DialoguesPickUp[0].Text = "Take me to the " + _activeMission.Destination.name + " but be carefull i have a coffee";
                _activeMission.DialoguesPickUp[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesPickUp[1].Text = "Im allways carefull!";
                _activeMission.DialoguesPickUp[1].Sprite = _database.VinSprite;
                _activeMission.DialoguesDestination = new Dialogue[2];
                _activeMission.DialoguesDestination[0].Text = "You are amazing!! Thank you!!";
                _activeMission.DialoguesDestination[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesDestination[1].Text = "Anytime!";
                _activeMission.DialoguesDestination[1].Sprite = _database.VinSprite;

                break;
            case 2:
                _activeMission.MissionType = MissionType.Tetris;
                _activeMission.DialoguesPickUp = new Dialogue[2];
                _activeMission.DialoguesPickUp[0].Text = "Help me with my bags and take me to the" + _activeMission.Destination.name;
                _activeMission.DialoguesPickUp[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesPickUp[1].Text = "Anything for you!!";
                _activeMission.DialoguesPickUp[1].Sprite = _database.VinSprite;
                _activeMission.DialoguesDestination = new Dialogue[2];
                _activeMission.DialoguesDestination[0].Text = "Thank you!";
                _activeMission.DialoguesDestination[0].Sprite = _database.Clients[client].ClientSprite;
                _activeMission.DialoguesDestination[1].Text = "Anytime!";
                _activeMission.DialoguesDestination[1].Sprite = _database.VinSprite;
                break;
            default:
                break;
        }
        isQuickMission = true;
       
       

        
       
        _uiManager.GpsOn(_activeMission.Origin.transform);
        MissionStarted = true;
    }
    
    void SpawnDestination()
    {

        _activeMission.Destination = Places[Random.Range(0, Places.Count)];
        float distance = Vector3.Distance(_activeMission.Origin.transform.position, _activeMission.Destination.transform.position);
        if (distance < 55)
        {
            SpawnDestination();

        }
    }
    #endregion
    #region Cliente Origin and Destination Checkers
    void ClientPickUp()
    {
        if (Vector3.Distance(PlayerCar.transform.position, _activeMission.Origin.transform.position) <= 8.5f && PlayerCar.CurrentSpeed == 0 && _clientPickedUp == false)
        {
            if (_activeMission.Origin != GameManager.instance.CurrentCarInUse)
            {
                _activeMission.Origin.GetComponent<MeshRenderer>().enabled=false;

            }

            PlayerCar.CanMove = false;
            Debug.Log(_activeMission.Destination);
            _uiManager.GpsOn(_activeMission.Destination.transform);
            _timer = ((int)(Vector3.Distance(PlayerCar.transform.position, _activeMission.Destination.transform.position)) / 4);
            if (CheckDialog(_activeMission.DialoguesPickUp))
            {
                _isInDialogue = true;
                StartDialogue();
            }
        }
    }
    
    public  void StartTimer()
    {
        _clientPickedUp = true;
        _startTimer = true;
        PlayerCar.CanMove = true;
        _activeMission.Destination.GetComponent<MeshRenderer>().enabled = true;

        _dialogueCounter = 0;
        Timer();
    }

    void ClientDestination()
    {
        if (Vector3.Distance(PlayerCar.transform.position, _activeMission.Destination.transform.position) <= 8.5f && PlayerCar.CurrentSpeed == 0)
        {
            _startTimer = false;
            _activeMission.Destination.GetComponent<MeshRenderer>().enabled = false;
            if (_activeMission.MissionType == MissionType.Coffee)
            {
                if(SceneManager.sceneCount>1)
                {
                    SceneManager.UnloadSceneAsync("Coffe");

                }

            }
            _clientReachedDestination = true;
            
            _uiManager.ShowTimer(false, 0);
            PlayerCar.CanMove = false;
            _uiManager.GpsOn(_activeMission.Destination.transform);
           // _timer = ((int)(Vector3.Distance(PlayerCar.transform.position, _activeMission.Destination.transform.position)) / 4);
            if (CheckDialog(_activeMission.DialoguesDestination))
            {
               StartDialogue();
                _isInDialogue = true; 
            }
            if(_dialogueCounter >= _activeMission.DialoguesDestination.Length)
            {
                if(isQuickMission)
                {
                    isQuickMission = false;
                    GameManager.instance.Money += Random.Range(100, 250);
                }
                else
                {
                    GameManager.instance.Money += Missions.ArcOneMissions[0].Reward;
                Missions.ArcOneMissions.RemoveAt(0);
                }
                UiManager.instance.UpdateMoney();
                _dialogueCounter = 0;
                _clientPickedUp = false;
                MissionStarted = false;
                _activeMission = null;
                _startTimer = false;
                _clientReachedDestination = false;
                PlayerCar.CanMove = true;
                _isInDialogue = false;
                _uiManager.GpsOff();
                UiManager.instance.updateMissionButtonInfo(_missions.ArcOneMissions[0].Client.ClientName, _missions.ArcOneMissions[0].Client.ClientSprite, _missions.ArcOneMissions[0].Discription);

            }
            //send to ui next mission info
        }           
    }
    #endregion
    #region Dialogue

    bool CheckDialog(Dialogue[] dialogues)
    {
        if (dialogues.Length == 0)
        {
            _startTimer = true;
            PlayerCar.CanMove = true;
            _activeMission.Destination.SetActive(true);


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
        if (_clientPickedUp == false)
        {
            if (_dialogueCounter < _activeMission.DialoguesPickUp.Length)
            {
                _isInDialogue = true;
                _uiManager.Dialogue(_activeMission.DialoguesPickUp[_dialogueCounter].Sprite, _activeMission.DialoguesPickUp[_dialogueCounter].Text);
            }
            else
            {
                _uiManager.CloseDialogue();
                _isInDialogue = false;
                switch (_activeMission.MissionType)
                {
                    case MissionType.AtoB:
                        StartTimer();
                        break;
                    case MissionType.Tetris:
                        // HERE 
                        if (SceneManager.sceneCount == 1)
                        {
                            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Puzzle", LoadSceneMode.Additive);
                                                    }
                        break;

                    case MissionType.Coffee:
                        if (SceneManager.sceneCount == 1)
                        {
                            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Coffe", LoadSceneMode.Additive);
                            StartTimer();
                        }
                        break;
                }
            }
        }
        //else if (_clientPickedUp == true && _clientReachedDestination == false)
        //{
        //    if (_dialogueCounter < _activeMission.DialoguesInMission.Length)
        //    {
        //        _uiManager.Dialogue(_activeMission.DialoguesInMission[_dialogueCounter].Sprite, _activeMission.DialoguesInMission[_dialogueCounter].Text);

        //    }
        //    else
        //    {
        //        _uiManager.CloseDialogue();
        //    }

        //}
        else
        {
            if (_dialogueCounter < _activeMission.DialoguesDestination.Length)
            {

                _uiManager.Dialogue(_activeMission.DialoguesDestination[_dialogueCounter].Sprite, _activeMission.DialoguesDestination[_dialogueCounter].Text);

            }
            else
            {
                _uiManager.CloseDialogue();
            }
        }

    }
    #endregion
    #region timerLogic
    void Timer()
    {
        _timer = _timer - Time.deltaTime;
        _uiManager.ShowTimer(true, _timer);

        if (_timer < 0)
        {

            LostMission();

        }
    }
    #endregion
    public void LostMission()
    {
        // quando chega a 0 o tempo aumenta um pouco mas o reward do player diminui
        if (_activeMission.MissionType == MissionType.Coffee)
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync("Coffe");
            }
        }
        _startTimer = false;
        _uiManager.ShowTimer(false, 0);
        _uiManager.GpsOff();
        _activeMission.Destination.GetComponent<MeshRenderer>().enabled = false;

        _activeMission = null;
        _clientPickedUp = false;
        MissionStarted = false;
        _clientReachedDestination = false;

     
    }


}
