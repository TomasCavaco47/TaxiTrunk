using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]

public class UpgradeUI
{
    [SerializeField] private List<GameObject> _acelarationUpgradeImages;
    [SerializeField] private List<GameObject> _maxSpeedUpgradeImages;
    [SerializeField] private List<GameObject> _brakingUpgradeImages;
    [Space]
    [SerializeField] private GameObject _buyAcelarationButton;
    [SerializeField] private GameObject _buyMaxSpeedButton;
    [SerializeField] private GameObject _buyBrakesButton;
    [SerializeField] GameObject _text1, _text2, _text3;


    public void CkeckUpgrades(int currentcardisplay, List<GameObject> cars)
    {
        for (int i = 0; i < _acelarationUpgradeImages.Count; i++)
        {
            _acelarationUpgradeImages[i].SetActive(false);
            _brakingUpgradeImages[i].SetActive(false);
            _maxSpeedUpgradeImages[i].SetActive(false);
        }
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel; i++)
        {
            _acelarationUpgradeImages[i].SetActive(true);
            _text1.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);
        } 
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel; i++)
        {
            _maxSpeedUpgradeImages[i].SetActive(true);
            _text2.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);
        }
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel; i++)
        {
            _brakingUpgradeImages[i].SetActive(true);
            _text3.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);

        } 
        
    }

    public List<GameObject> AcelarationUpgradeImages { get => _acelarationUpgradeImages; set => _acelarationUpgradeImages = value; }
    public List<GameObject> BrakingUpgradeImages { get => _brakingUpgradeImages; set => _brakingUpgradeImages = value; }
    public List<GameObject> MaxSpeedUpgradeImages { get => _maxSpeedUpgradeImages; set => _maxSpeedUpgradeImages = value; }
    public GameObject BuyMaxSpeedButton { get => _buyMaxSpeedButton; set => _buyMaxSpeedButton = value; }
    public GameObject BuyBrakesButton { get => _buyBrakesButton; set => _buyBrakesButton = value; }
    public GameObject BuyAcelarationButton { get => _buyAcelarationButton; set => _buyAcelarationButton = value; }
}
public class StoreManager : MonoBehaviour
{
    [SerializeField] UiManager _uiManager;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] int _currentDisplaying;
    [SerializeField] List<GameObject> _carModels;
    [SerializeField] Transform _displayPos;
    [Space]
    [Header("editables")]
    [SerializeField] List<GameObject> _carLogos;
    [SerializeField] GameObject _equipTab, _buyTab, _UpgradeTab;
    [SerializeField] GameObject _soldOut1, _soldOut2, _soldOut3;
    [SerializeField] List<GameObject> _descripton;
    [SerializeField] List<GameObject> _allButtons;


    [Space]
   
    
 
   
    [SerializeField] UpgradeUI _upgradeUI;
   


    public int CurrentDisplaying { get => _currentDisplaying; set => _currentDisplaying = value; }

    private void Awake()
    {
        _uiManager = UiManager.instance;
    }
    void Start()
    {
        _carModels = GameManager.instance.CarModels;
        for (int i = 0; i < _carModels.Count; i++)
        {
            _carModels[i].SetActive(false);
            _carLogos[i].SetActive(false);
            _carModels[i].transform.position = _displayPos.position;

            if (GameManager.instance.CurrentCarInUse == _carModels[i])
            {
                _currentDisplaying = i;
            }
        }
        _carLogos[_currentDisplaying].SetActive(true);
        _carModels[_currentDisplaying].SetActive(true);
        _carModels[_currentDisplaying].transform.position = _displayPos.position;
        CheckCar();
        WitchButonToBeSelected();


    }

    // Update is called once per frame
    void Update()
    {
       _carModels[_currentDisplaying].transform.Rotate(0, 4 * 8 * Time.deltaTime, 0 );
        Inputs();
    }
    private void Inputs()
    {
        int nextcar = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentDisplaying + 1 == _carModels.Count)
            {
                ChangeCarAndTab(nextcar);
            }
            else
            {
                ChangeCarAndTab(_currentDisplaying + 1);
            }
        } 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentDisplaying == 0)
            {
                ChangeCarAndTab(_carModels.Count-1);
            }
            else
            {
                ChangeCarAndTab(_currentDisplaying - 1);
            }
        }
    }

   private void ChangeCarAndTab(int carToShow)
    {
       
        _carModels[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false); _descripton[_currentDisplaying].SetActive(false);
        _soldOut1.SetActive(false); _soldOut2.SetActive(false);_soldOut3.SetActive(false);

        _currentDisplaying = carToShow;
        _carModels[_currentDisplaying].transform.eulerAngles = new Vector3(0, 130, 0);
        _carModels[_currentDisplaying].SetActive(true);
        _carLogos[_currentDisplaying].SetActive(true);
        CheckCar();
        WitchButonToBeSelected();



    }
    private void WitchButonToBeSelected()
    {
        for (int i = 0; i < _allButtons.Count; i++)
        {
            if(_allButtons[i].activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_allButtons[i]);
                break;
            }
        }
    }
   
    private void CheckCar()
    {
        if (_carModels[_currentDisplaying]==GameManager.instance.CurrentCarInUse)
        {
            _buyTab.SetActive(false); _equipTab.SetActive(false);_descripton[_currentDisplaying].SetActive(false);
            _soldOut1.SetActive(false); _soldOut2.SetActive(false); _soldOut3.SetActive(false);
            _UpgradeTab.SetActive(true);
            _upgradeUI.BuyAcelarationButton.SetActive(true); _upgradeUI.BuyMaxSpeedButton.SetActive(true); _upgradeUI.BuyBrakesButton.SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
            
        }
        else if(GameManager.instance.PlayerCarsBought.Contains(_carModels[_currentDisplaying]))
        {
            _UpgradeTab.SetActive(false); _buyTab.SetActive(false);_soldOut1.SetActive(false); _soldOut2.SetActive(false); _soldOut3.SetActive(false);
            _equipTab.SetActive(true);_descripton[_currentDisplaying].SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying,_carModels);
        }
        else
        {
            _UpgradeTab.SetActive(false); _equipTab.SetActive(false); _soldOut1.SetActive(false); _soldOut2.SetActive(false); _soldOut3.SetActive(false);
            _buyTab.SetActive(true); _descripton[_currentDisplaying].SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        }
      if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel == 2)
        {
            _upgradeUI.BuyAcelarationButton.SetActive(false);
            WitchButonToBeSelected();
            _soldOut1.SetActive(true);
           
        }
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel == 2)
        {
            _upgradeUI.BuyMaxSpeedButton.SetActive(false);
            WitchButonToBeSelected();
                _soldOut2.SetActive(true);
          
        }    
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel == 2)
        {
            _upgradeUI.BuyBrakesButton.SetActive(false);
            WitchButonToBeSelected();
                _soldOut3.SetActive(true);
            
        }   

    }
    public void BuyCarButton()
    {
        GameManager.instance.PlayerCarsBought.Add(_carModels[_currentDisplaying]);
        GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
        CheckCar();
        WitchButonToBeSelected();
    }
    public void ChangeCarButton()
    {
        GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
        CheckCar();
        WitchButonToBeSelected();
    }
    public void UpgradeMaxSpeed()
    {
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
        CheckCar();
        
    } 
    public void UpgradeAcelaration()
    {
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
        CheckCar();
        
    } 
    public void UpgradeBrakes()
    {
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
        CheckCar();
        
    }
      

    public void ExitStore()
    {
        GameManager.instance.UpdateCamerasAndGps();
        GameManager.instance.CurrentCarInUse.transform.position = GameManager.instance.CarExitStorePos.position;
        GameManager.instance.CurrentCarInUse.transform.rotation = GameManager.instance.CarExitStorePos.rotation;
        SceneManager.UnloadSceneAsync("Store");
        _uiManager.InGameUi.SetActive(true);
        




    }

}
