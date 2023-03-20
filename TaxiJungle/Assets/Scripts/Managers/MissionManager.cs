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
    [SerializeField] GameObject _PuzzleCanvas;
    public static MissionManager instance;

    [Header("Data")]
    [SerializeField] DataBase _database;
    private List<GameObject> _places;
    private List<Client> _clients;
    [Header("MissionConfirmations")]
    [SerializeField] Mission _activeMission;
    [SerializeField] bool _puzzleCompleted;

    [SerializeField] float _timer;
    [SerializeField] bool _startTimer=false;

    [SerializeField] bool _missionStarted;
    [SerializeField] bool _clientPickedUp;
    [SerializeField] bool _clientReachedDestination;
    [SerializeField] float _slowMotionTimeScale;
    Vector3 _initialCarPos;

    float _startTimeScale;
    float _startFixedDeltaTime;
    [SerializeField]private int _dialogueCounter;
    private bool _canShowDialogueInMission=true;

    public bool MissionStarted { get => _missionStarted; set => _missionStarted = value; }
    public MissionData Missions { get => _missions; set => _missions = value; }
    public List<GameObject> Places { get => _places; set => _places = value; }

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
        Debug.Log(_startFixedDeltaTime);
        _startTimeScale = Time.timeScale;
        Debug.Log(_startTimeScale);
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

                for (int a = 0; a < Missions.ArcOneMissions[i].DialoguesInMission.Length; a++)
                {
                    if (Missions.ArcOneMissions[i].DialoguesInMission.Length != 0)
                    {
                        switch (Missions.ArcOneMissions[i].DialoguesInMission[a].WhosTalking)
                        {
                            case WhosTalking.Client:
                                Missions.ArcOneMissions[i].DialoguesInMission[a].Sprite = Missions.ArcOneMissions[i].Client.ClientSprite;
                                break;
                            case WhosTalking.Vin:
                                Missions.ArcOneMissions[i].DialoguesInMission[a].Sprite = _database.VinSprite;

                                break;
                            default:
                                break;
                        }
                    }
                }

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
        //?????????????????????????????
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
    private void FixedUpdate()
    {
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public void StartStoryMissions()
    {
        _activeMission = Missions.ArcOneMissions[0];
        _uiManager.GpsOn(_activeMission.Origin.transform);
        MissionStarted = true;
    }
    #region QuickMission
    public void StartQuickMissions()
    {
        _activeMission = new Mission();
       _activeMission.Origin = Places[Random.Range(0,Places.Count)];

        Debug.Log("Pick me up in " + _activeMission.Origin.name);
        SpawnDestination();
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
        
        if (Vector3.Distance(GameManager.instance.CurrentCarInUse.transform.position, _activeMission.Origin.transform.position) <= 5 && GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CurrentSpeed == 0 && _clientPickedUp == false)
        {
            GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = false;
            _uiManager.GpsOn(_activeMission.Destination.transform);
            Debug.Log(_activeMission.Destination);
            _timer = ((int)(Vector3.Distance(GameManager.instance.CurrentCarInUse.transform.position, _activeMission.Destination.transform.position)) / 4);
            if (CheckDialog(_activeMission.DialoguesPickUp))
            {
                StartDialogue();
            }

        }
    }
    
    public  void StartTimer()
    {
        _clientPickedUp = true;
        _startTimer = true;
        _initialCarPos = GameManager.instance.CurrentCarInUse.transform.position;
        GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = true;
        _dialogueCounter = 0;
        Timer();
    }

    void ClientDestination()
    {
        //if(_canShowDialogueInMission)
        //{
        //    if (Vector3.Distance(GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().transform.position, _activeMission.Destination.transform.position) <= Vector3.Distance(_initialCarPos, _activeMission.Destination.transform.position) / 2)
        //    {
        //        if (CheckDialog(_activeMission.DialoguesInMission))
        //        {
        //            StartDialogue();
        //            _canShowDialogueInMission = false;
        //            Debug.Log("did");
        //            Time.timeScale = 0.3f;

        //        }
        //        if (_dialogueCounter >= _activeMission.DialoguesInMission.Length)
        //        {
        //            _dialogueCounter = 0;
        //            Time.timeScale = 1;

        //        }

        //    }
        //}
       
        if (Vector3.Distance(GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().transform.position, _activeMission.Destination.transform.position) <= 5 && GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CurrentSpeed == 0)
        {
            if (_activeMission.MissionType == MissionType.Coffee)
            {
                if(SceneManager.sceneCount>1)
                {
                    SceneManager.UnloadSceneAsync("Coffe");

                }

            }
            _clientReachedDestination = true;
            _uiManager.GpsOff();
            _uiManager.ShowTimer(false, 0);
            GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = false;
            _uiManager.GpsOn(_activeMission.Destination.transform);
            _startTimer = false;
            if (CheckDialog(_activeMission.DialoguesDestination))
            {
               StartDialogue();
                
            }
            if(_dialogueCounter >= _activeMission.DialoguesDestination.Length)
            {
                _dialogueCounter = 0;
                _clientPickedUp = false;
                MissionStarted = false;
                //_activeMission = null;
                _startTimer = false;
                _clientReachedDestination = false;
                GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = true;

                Missions.ArcOneMissions.RemoveAt(0);
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
            GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = true;

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
                _uiManager.Dialogue(_activeMission.DialoguesPickUp[_dialogueCounter].Sprite, _activeMission.DialoguesPickUp[_dialogueCounter].Text);

            }
            else
            {

                _uiManager.CloseDialogue();
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
        else if (_clientPickedUp == true && _clientReachedDestination == false )
        {
            if (_dialogueCounter < _activeMission.DialoguesInMission.Length)
            {
                _uiManager.Dialogue(_activeMission.DialoguesInMission[_dialogueCounter].Sprite, _activeMission.DialoguesInMission[_dialogueCounter].Text);

            }
            else
            {
                _uiManager.CloseDialogue();
            }

        }
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
            SceneManager.UnloadSceneAsync("Coffe");
        }
        _startTimer = false;
        _uiManager.ShowTimer(false, 0);
        _uiManager.GpsOff();
        _activeMission = null;
        _clientPickedUp = false;
        MissionStarted = false;
        _clientReachedDestination = false;
     
    }


}
