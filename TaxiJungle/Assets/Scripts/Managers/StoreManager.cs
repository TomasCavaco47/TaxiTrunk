using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]

public class UpgradeUI
{
    [SerializeField] List<GameObject> _acelarationUpgradeImages;
    [SerializeField] List<GameObject> _maxSpeedUpgradeImages;
    [SerializeField] List<GameObject> _brakingUpgradeImages;
    [Space]
    [SerializeField] GameObject _acelarationPrice, _topSpeedPrices, _brakePrices;


    public void CkeckUpgrades(int currentcardisplay, List<GameObject> cars)
    {
        for (int i = 0; i < 4; i++)
        {
            _acelarationUpgradeImages[i].SetActive(false);
            _brakingUpgradeImages[i].SetActive(false);
            _maxSpeedUpgradeImages[i].SetActive(false);
        }
        for (int i = 0; i < cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel; i++)
        {
            _acelarationUpgradeImages[i].SetActive(true);
            if (i < 3)
            {
             AcelarationPrice.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);
            }
        } 
        for (int i = 0; i < cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel; i++)
        {
            _maxSpeedUpgradeImages[i].SetActive(true);
            if (i < 3)
            {
                TopSpeedPrices.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);
            }
        }
        for (int i = 0; i < cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel; i++)
        {
            _brakingUpgradeImages[i].SetActive(true);
            if (i <3)
            {
                BrakePrices.GetComponent<BuyUpgradeButton>().CkeckUpgradesButtons(i);
            }

        } 
        
    }

    public List<GameObject> AcelarationUpgradeImages { get => _acelarationUpgradeImages; set => _acelarationUpgradeImages = value; }
    public List<GameObject> BrakingUpgradeImages { get => _brakingUpgradeImages; set => _brakingUpgradeImages = value; }
    public List<GameObject> MaxSpeedUpgradeImages { get => _maxSpeedUpgradeImages; set => _maxSpeedUpgradeImages = value; }
    public GameObject AcelarationPrice { get => _acelarationPrice; set => _acelarationPrice = value; }
    public GameObject TopSpeedPrices { get => _topSpeedPrices; set => _topSpeedPrices = value; }
    public GameObject BrakePrices { get => _brakePrices; set => _brakePrices = value; }
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
    [SerializeField] GameObject _equipTab, _buyTab, _UpgradeTab;
    [SerializeField] List<GameObject> _allButtons;
    bool _But1 = false, _But2 = false, _But3 = false;
    [SerializeField] Text _carName;
    [SerializeField] List<string> _carNames;
    [SerializeField] List<GameObject> _carLogos;
    [SerializeField] List<GameObject> _descripton;
    [Space]
    [SerializeField] UpgradeUI _upgradeUI;

    int carPrices = 1000;


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
            _carModels[i].GetComponent<CarControllerTest>().CanMove = false;
            if (GameManager.instance.CurrentCarInUse == _carModels[i])
            {
                _currentDisplaying = i;
            }
        }
        _carName.text = _carNames[_currentDisplaying].ToString();
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
        //Desliga tudo o que ta ativo
        _carModels[_currentDisplaying].SetActive(false);
        _buyTab.SetActive(false); _equipTab.SetActive(false); _descripton[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false);
        _UpgradeTab.SetActive(false);

        _currentDisplaying = carToShow;
        _carModels[_currentDisplaying].transform.eulerAngles = new Vector3(0, 130, 0);
        _carName.text = _carNames[_currentDisplaying].ToString();
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

    private void ButtonsSwitch()
    {
        if(_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel == 3)
        {
            _allButtons[0].SetActive(false);
            if(_But1 == false)
            {
                Debug.Log("testeqw");
             WitchButonToBeSelected();
            }
            _But1 = true;
        }
        else
        {
            _allButtons[0].SetActive(true);
            _But1 = false;
        }
       
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel == 3)
        {
            _allButtons[1].SetActive(false);
            if(_But2 == false)
            {
             WitchButonToBeSelected();
            }
            _But2 = true;
        }
        else
        {
            _allButtons[1].SetActive(true);
            _But2 = false;
        }
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel == 3)
        {
            _allButtons[2].SetActive(false);
            if(_But3 == false)
            {
             WitchButonToBeSelected();
            }
            _But3 = true;
        }
        else
        {
            _allButtons[2].SetActive(true);
            _But3 = false;
        }


    }
   
    private void CheckCar()
    {
        if (_carModels[_currentDisplaying]==GameManager.instance.CurrentCarInUse)
        {
            _buyTab.SetActive(false); _equipTab.SetActive(false); _descripton[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false);
            _UpgradeTab.SetActive(false);
            //ativaa Tab De Upgrades
            _UpgradeTab.SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
            WitchButonToBeSelected();
            ButtonsSwitch();

            


        }
        else if(GameManager.instance.PlayerCarsBought.Contains(_carModels[_currentDisplaying]))
        {
            _buyTab.SetActive(false); _equipTab.SetActive(false); _descripton[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false);
            _UpgradeTab.SetActive(false);
            //Ativa a Tab de equip
            _equipTab.SetActive(true); _descripton[_currentDisplaying].SetActive(true); _carLogos[_currentDisplaying].SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying,_carModels);
           
        }
        else
        {
            _buyTab.SetActive(false); _equipTab.SetActive(false); _descripton[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false);
            _UpgradeTab.SetActive(false);
            //ativa a tab de Buy
            _buyTab.SetActive(true); _descripton[_currentDisplaying].SetActive(true); _carLogos[_currentDisplaying].SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
           
        }



        //Dessativar os botoes dps de todas a melhorias sempre compradas
      
     

    }

    #region ButtonsActions
    public void BuyCarButton()
    {
        if(GameManager.instance.Money>=carPrices)
        {
            GameManager.instance.PlayerCarsBought.Add(_carModels[_currentDisplaying]);
            GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
            CheckCar();
            WitchButonToBeSelected();

        }
    }
    public void EquipCarButton()
    {
        GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
        CheckCar();
        WitchButonToBeSelected();
    }
    public void UpgradeAcelaration()
    {
        if (_carModels[CurrentDisplaying]== _carModels[0])
        {
          
            string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);
           

            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if(_carModels[CurrentDisplaying] == _carModels[1])
        {
            string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i) 
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if (_carModels[CurrentDisplaying] == _carModels[2])
        {
            string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i) 
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }



    } 
    public void UpgradeMaxSpeed()
    {

        if (_carModels[CurrentDisplaying] == _carModels[0])
        {

            string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i) 
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if (_carModels[CurrentDisplaying] == _carModels[1])
        {

            string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if (_carModels[CurrentDisplaying] == _carModels[2])
        {

            string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }

    } 
    public void UpgradeBrakes()
    {
        if (_carModels[CurrentDisplaying] == _carModels[0])
        {

            string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if (_carModels[CurrentDisplaying] == _carModels[1])
        {

            string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }
        if (_carModels[CurrentDisplaying] == _carModels[2])
        {

            string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
            value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
            int i = int.Parse(value2);


            if (GameManager.instance.Money >= i)
            {
                _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
                _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
                ButtonsSwitch();
            }
        }

    }
      

    public void ExitStore()
    {
        _carModels[_currentDisplaying].SetActive(false);
        GameManager.instance.CurrentCarInUse.SetActive(true);
        GameManager.instance.UpdateCamerasAndGps();
        GameManager.instance.CurrentCarInUse.transform.position = GameManager.instance.CarExitStorePos.position;
        GameManager.instance.CurrentCarInUse.transform.rotation = new Quaternion(0, -174.8f, 0, 0);
        MissionManager.instance.PlayerCar = GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>();
        GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = true;

        Test.instance.ChangeCameraTargets();
        _uiManager.InGameUi.SetActive(true);
        GameManager.instance.InStore = false;
        SceneManager.UnloadSceneAsync("Store");
        

    }
    #endregion

}
