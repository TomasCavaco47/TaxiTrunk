using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private List<GameObject> _carModels;
    [SerializeField] private List<GameObject> _playerCarsBought;
    [SerializeField] GameObject _currentCarInUse;
    [SerializeField] Transform _carExitStorePos;
    [SerializeField] Transform _startgamePos;
    [SerializeField] MiniMapCameraFolows _miniMapCamera;
    [SerializeField] LineAI _gps;
    [SerializeField] UiManager _ui;
    MissionManager _missionManager;
    [SerializeField]bool _canEnterStore;
    int _money;
    

    public GameObject CurrentCarInUse { get => _currentCarInUse; set => _currentCarInUse = value; }
    public List<GameObject> CarModels { get => _carModels; set => _carModels = value; }
    public bool CanEnterStore { get => _canEnterStore; set => _canEnterStore = value; }
    public List<GameObject> PlayerCarsBought { get => _playerCarsBought; set => _playerCarsBought = value; }
    public Transform CarExitStorePos { get => _carExitStorePos; set => _carExitStorePos = value; }

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
    }
    private void Update()
    {
        EnterStore();
    }
    private void Start()
    {
        _missionManager = MissionManager.instance;

        _missionManager.PlayerCar = _currentCarInUse.GetComponent<CarControllerTest>();
        UpdateCamerasAndGps();
        //_ui.Car = _currentCarInUse;
        

    }
    public void UpdateCamerasAndGps()
    {
        Camera.main.GetComponent<CameraFollow>().Target = _currentCarInUse.transform;
        _miniMapCamera.Target = _currentCarInUse.transform;
        _gps.Player = _currentCarInUse.transform;
    }
    public void EnterStore()
    {
        if(_canEnterStore==true)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                if(_currentCarInUse.GetComponent<CarControllerTest>().CurrentSpeed==0)
                {
                    
                    UiManager.instance!.OpenStore();
                }
            }
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
