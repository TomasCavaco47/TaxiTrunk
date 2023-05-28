using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private List<GameObject> _carModels;
    [SerializeField] private List<GameObject> _playerCarsBought;
    [SerializeField] private List<GameObject> _aICars;
    [SerializeField] GameObject aIWaypointsParent;
    [SerializeField] int numberOfAICars;
    [SerializeField] GameObject _currentCarInUse;
    [SerializeField] Transform _carExitStorePos;
    [SerializeField] Transform _startgamePos;
    [SerializeField] MiniMapCameraFolows _miniMapCamera;
    [SerializeField] LineAI _gps;
    [SerializeField] UiManager _ui;
    MissionManager _missionManager;
    [SerializeField]bool _canEnterStore;
    [SerializeField]bool _inStore=false;
    [SerializeField] int _money;
    [SerializeField] GameObject _riverRespawn;

    [SerializeField] LayerMask _aiCarLayer;
    [SerializeField] LayerMask _playerCarLayer;
                     RadioManager _radioManager;


    public GameObject CurrentCarInUse { get => _currentCarInUse; set => _currentCarInUse = value; }
    public List<GameObject> CarModels { get => _carModels; set => _carModels = value; }
    public bool CanEnterStore { get => _canEnterStore; set => _canEnterStore = value; }
    public List<GameObject> PlayerCarsBought { get => _playerCarsBought; set => _playerCarsBought = value; }
    public Transform CarExitStorePos { get => _carExitStorePos; set => _carExitStorePos = value; }
    public int Money { get => _money; set => _money = value; }
    public bool InStore { get => _inStore; set => _inStore = value; }

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
        for (int i = 0; i < CarModels.Count; i++)
        {
        GameObject a= Instantiate(CarModels[i]);
            CarModels[i] = a;
            CarModels[i].SetActive(false);
        }
        PlayerCarsBought.Add(CarModels[0]);
        _currentCarInUse = PlayerCarsBought[0];
        _currentCarInUse.SetActive(true);
        _currentCarInUse.transform.position = _startgamePos.position;
        _currentCarInUse.transform.rotation=new Quaternion(-0.000317146303f, 0.383167118f, 0.000130905319f, 0.923678994f);


        spawnAICars();

    }
    private void Update()
    {
        EnterStore();
    }
    private void Start()
    {
        _radioManager = RadioManager._instance;
        _missionManager = MissionManager.instance;

        _missionManager.PlayerCar = _currentCarInUse.GetComponent<CarControllerTest>();
        UpdateCamerasAndGps();
        //_ui.Car = _currentCarInUse;
        

    }
    private void spawnAICars()
    {
        List<int> teste = new List<int>(); 
        for (int i = 0;i<=numberOfAICars;i++) 
        {
            int pos = Random.Range(0, aIWaypointsParent.transform.childCount);
            if(teste.Contains(pos))
            {
                i--;
            }
            else
            {
                teste.Add(pos);
                WayPoints waypointToSPawn = aIWaypointsParent.transform.GetChild(pos).GetComponent<WayPoints>();
                GameObject a= Instantiate(_aICars[ Random.Range(0, _aICars.Count)]);
                a.transform.position = waypointToSPawn.transform.position;
                a.GetComponent<AI_Car_Script>().CurrentWaypoint=waypointToSPawn.NextWaypoint[0];
                a.transform.LookAt(waypointToSPawn.NextWaypoint[0]);

            }

        }
    }
    public void RespawnCars(GameObject aiCar)
    {
        aiCar.SetActive(false);
        
        StartCoroutine(WaitToActive(aiCar));
    }
    public void RespawInRiverCar()
    {
        _currentCarInUse.transform.position=_riverRespawn.transform.position;
        _currentCarInUse.transform.rotation= new Quaternion(-0.000310014904f, -0.406461269f, -0.000138505347f, 0.913667977f);
    }
    IEnumerator WaitToActive(GameObject aiCar)
    {
        bool wait = true;
        int pos = Random.Range(0, aIWaypointsParent.transform.childCount);
        WayPoints waypointToSPawn = aIWaypointsParent.transform.GetChild(pos).GetComponent<WayPoints>();
        aiCar.transform.position = waypointToSPawn.transform.position;
        aiCar.GetComponent<AI_Car_Script>().CurrentWaypoint = waypointToSPawn.NextWaypoint[0];
        aiCar.transform.LookAt(waypointToSPawn.NextWaypoint[0]);
        while (wait)
        {
            List<Collider> hitColliders = new List<Collider>();
            hitColliders = Physics.OverlapSphere(aiCar.transform.position, 5, _aiCarLayer + _playerCarLayer).ToList();
            if(hitColliders.Count==0)
            {
                wait = false; 
                break;
            }
        }
        aiCar.SetActive(true);

        yield return null;
    }
    public void UpdateCamerasAndGps()
    {
        Camera.main.GetComponent<CameraFollow>().Target = _currentCarInUse.transform;
        _miniMapCamera.Target = _currentCarInUse.transform;
        _gps.Player = _currentCarInUse.transform;
    }
    public void EnterStore()
    {
        if(_canEnterStore==true && MissionManager.instance.MissionStarted==false )
        {
           
            if (_currentCarInUse.GetComponent<CarControllerTest>().CurrentSpeed < 2)
            {
                UiManager.instance.EnterStoreText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.K))
                {

                    _radioManager.PauseRadio();
                    UiManager.instance!.OpenStore();
                    InStore = true;

                }
            }

                
        }
        else
        {
            UiManager.instance.EnterStoreText.SetActive(false);

        }


    }
    public void changestattion()
    {
        
        int i = 0;

        i++;
        
        if (i >= 5)
        {
            i = 0;
        }

        switch (i)
        {
            case 0:
                bool radio1 = true;
                break;
            case 1:
                EnterStore();
                bool radio2 = true;
                int volumeRadio1 = 0;
                int volumeRadio2 = 1;
                int volumeRadio3 = 0;
                i = -1;
                break;
            default:
                break;
        }
     
    }

}
