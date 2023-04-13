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
    #region Variaveis
    [SerializeField] List<GameObject> _acelarationUpgradeImages;
    [SerializeField] List<GameObject> _maxSpeedUpgradeImages;
    [SerializeField] List<GameObject> _brakingUpgradeImages;
    [Space]
    [SerializeField] GameObject _acelarationPrice, _topSpeedPrices, _brakePrices;
    public List<GameObject> AcelarationUpgradeImages { get => _acelarationUpgradeImages; set => _acelarationUpgradeImages = value; }
    public List<GameObject> BrakingUpgradeImages { get => _brakingUpgradeImages; set => _brakingUpgradeImages = value; }
    public List<GameObject> MaxSpeedUpgradeImages { get => _maxSpeedUpgradeImages; set => _maxSpeedUpgradeImages = value; }
    
    #endregion

    public void CkeckUpgrades(int currentcardisplay, List<GameObject> cars)
    {
      
        for (int i = 0; i < 4; i++)
        {
            _acelarationUpgradeImages[i].SetActive(false);
            _brakingUpgradeImages[i].SetActive(false);
            _maxSpeedUpgradeImages[i].SetActive(false);
        }
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel; i++)
        {
            _acelarationUpgradeImages[i].SetActive(true);
            _acelarationPrice.GetComponent<BuyUpgradeButton>().ChangePriceInTheButton(i);

        }
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel; i++)
        {
            _maxSpeedUpgradeImages[i].SetActive(true);

            _topSpeedPrices.GetComponent<BuyUpgradeButton>().ChangePriceInTheButton(i);
            
        }
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel; i++)
        {
            _brakingUpgradeImages[i].SetActive(true);

            _brakePrices.GetComponent<BuyUpgradeButton>().ChangePriceInTheButton(i);

        } 
        
    }

}
public class StoreManager : MonoBehaviour
{
    #region Variaveis
    [Header("Refss & Info")]
    [SerializeField] UiManager _uiManager;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] Transform _displayPos;
    [SerializeField] int _currentDisplaying;
    [SerializeField] List<GameObject> _carModels;

    [Space]
    [Header("LowerPainel")]
    [SerializeField] Text _carName;
    [SerializeField] List<string> _carNames;

    [Header("RightPainel")]
    [SerializeField] GameObject _equipTab;
    [SerializeField] GameObject _buyTab;
    [SerializeField] GameObject _upgradeTab;
    [SerializeField] List<GameObject> _allButtons;
    bool _but1 = false, _but2 = false, _but3 = false;
    [SerializeField] List<GameObject> _carLogos;
    [SerializeField] List<GameObject> _descripton;

    [Space]
    [SerializeField] UpgradeUI _upgradeUI;

    int carPrices = 1000;
    public int _carIndex = 0;
    public int CurrentDisplaying { get => _currentDisplaying; set => _currentDisplaying = value; }
    #endregion

    #region MonoBehavior
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

    void Update()
    {
       _carModels[_currentDisplaying].transform.Rotate(0, 4 * 8 * Time.deltaTime, 0 );
        Inputs();
    }
    #endregion
    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentDisplaying + 1 == _carModels.Count)
            {
                //VOLTAR AO CARRO INICIAL dar o flip
                _carIndex = 0;
                CheckCar();
            }
            else
            {
                //vai para o proximo
                _carIndex = _currentDisplaying + 1;
                CheckCar();
            }
        } 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentDisplaying == 0)
            {
               _carIndex =_carModels.Count-1;
                CheckCar();
            }
            else
            {
                _carIndex =_currentDisplaying - 1;
                CheckCar();
            }
        }
    }

  
    private void CheckCar()
    {
        _carModels[_currentDisplaying].SetActive(false);
        _buyTab.SetActive(false); _equipTab.SetActive(false); _descripton[_currentDisplaying].SetActive(false); _carLogos[_currentDisplaying].SetActive(false);
        _upgradeTab.SetActive(false);

        _currentDisplaying = _carIndex;
        _carModels[_currentDisplaying].transform.eulerAngles = new Vector3(0, 130, 0);
        _carName.text = _carNames[_currentDisplaying].ToString();
        _carModels[_currentDisplaying].SetActive(true);

        //Ativaa Tab De Upgrades
        if (_carModels[_currentDisplaying]==GameManager.instance.CurrentCarInUse)
        {
            _upgradeTab.SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
            WitchButonToBeSelected();
            ButtonsSwitch();
        }
        //Ativa a Tab de equip
        else if(GameManager.instance.PlayerCarsBought.Contains(_carModels[_currentDisplaying]))
        {
            _equipTab.SetActive(true); _descripton[_currentDisplaying].SetActive(true); _carLogos[_currentDisplaying].SetActive(true);
            WitchButonToBeSelected();
        }
        //Ativa a tab de Buy
        else
        {
            _buyTab.SetActive(true); _descripton[_currentDisplaying].SetActive(true); _carLogos[_currentDisplaying].SetActive(true);
            WitchButonToBeSelected();
        }

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
            if(_but1 == false)
            {
                Debug.Log("testeqw");
             WitchButonToBeSelected();
            }
            _but1 = true;
        }
        else
        {
            _allButtons[0].SetActive(true);
            _but1 = false;
        }
       
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel == 3)
        {
            _allButtons[1].SetActive(false);
            if(_but2 == false)
            {
             WitchButonToBeSelected();
            }
            _but2 = true;
        }
        else
        {
            _allButtons[1].SetActive(true);
            _but2 = false;
        }
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel == 3)
        {
            _allButtons[2].SetActive(false);
            if(_but3 == false)
            {
             WitchButonToBeSelected();
            }
            _but3 = true;
        }
        else
        {
            _allButtons[2].SetActive(true);
            _but3 = false;
        }


    }
   

    #region ButtonsActions
    public void BuyCarButton()
    {
            GameManager.instance.PlayerCarsBought.Add(_carModels[_currentDisplaying]);
            GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
            CheckCar();
            WitchButonToBeSelected();
        if(GameManager.instance.Money>=carPrices)
        {

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
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        ButtonsSwitch();

        #region SalvaçãoDoRafa
        //if (_carModels[CurrentDisplaying]== _carModels[0])
        //{

        //    string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if(_carModels[CurrentDisplaying] == _carModels[1])
        //{
        //    string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i) 
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if (_carModels[CurrentDisplaying] == _carModels[2])
        //{
        //    string value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.AcelarationPrice.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i) 
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        #endregion
    }
    public void UpgradeMaxSpeed()
    {
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        ButtonsSwitch();

        #region Salvavação Do Rafa 2
        //if (_carModels[CurrentDisplaying] == _carModels[0])
        //{

        //    string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i) 
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if (_carModels[CurrentDisplaying] == _carModels[1])
        //{

        //    string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if (_carModels[CurrentDisplaying] == _carModels[2])
        //{

        //    string value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.TopSpeedPrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        #endregion
    }
    public void UpgradeBrakes()
    {
        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        ButtonsSwitch();

        #region SalvaçãoDoRafa3
        //if (_carModels[CurrentDisplaying] == _carModels[0])
        //{

        //    string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car1Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if (_carModels[CurrentDisplaying] == _carModels[1])
        //{

        //    string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car2Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        //if (_carModels[CurrentDisplaying] == _carModels[2])
        //{

        //    string value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel];
        //    value2 = _upgradeUI.BrakePrices.GetComponent<BuyUpgradeButton>().Car3Prices[_carModels[CurrentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel].Substring(0, value2.Length - 1);
        //    int i = int.Parse(value2);


        //    if (GameManager.instance.Money >= i)
        //    {
        //        _carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel++;
        //        _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        //        ButtonsSwitch();
        //    }
        //}
        #endregion
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
