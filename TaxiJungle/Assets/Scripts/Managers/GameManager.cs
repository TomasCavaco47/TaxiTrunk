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
    }
    private void Update()
    {
        EnterStore();
    }
    private void Start()
    {
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
        Camera.main.GetComponent<CameraFollow>().Target = _currentCarInUse.transform;

        _missionManager = MissionManager.instance;
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
}
