using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class UpgradeUI
{
    [SerializeField] private List<GameObject> _acelarationUpgradeImages;
    [SerializeField] private List<GameObject> _brakingUpgradeImages;
    [SerializeField] private List<GameObject> _maxSpeedUpgradeImages;
    [Space]
    [SerializeField] private GameObject _buyMaxSpeedButton;
    [SerializeField] private GameObject _buyBrakesButton;
    [SerializeField] private GameObject _buyAcelarationButton;

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
        } 
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel; i++)
        {
            _brakingUpgradeImages[i].SetActive(true);

        } 
        for (int i = 0; i <= cars[currentcardisplay].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel; i++)
        {
            _maxSpeedUpgradeImages[i].SetActive(true);
        }
        
    }

    public List<GameObject> AcelarationUpgradeImages { get => _acelarationUpgradeImages; set => _acelarationUpgradeImages = value; }
    public List<GameObject> BrakingUpgradeImages { get => _brakingUpgradeImages; set => _brakingUpgradeImages = value; }
    public List<GameObject> MaxSpeedUpgradeImages { get => _maxSpeedUpgradeImages; set => _maxSpeedUpgradeImages = value; }
    public GameObject BuyMaxSpeedButton { get => BuyMaxSpeedButton; set => BuyMaxSpeedButton = value; }
    public GameObject BuyBrakesButton { get => BuyBrakesButton; set => BuyBrakesButton = value; }
    public GameObject BuyAcelarationButton { get => BuyAcelarationButton; set => BuyAcelarationButton = value; }
}
public class StoreManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _carModels;
    [SerializeField] int _currentDisplaying;
    [SerializeField] Transform _displayPos;
    [Space]
    [SerializeField] GameObject _changeCarbutton;
    [SerializeField] GameObject _buyCarbutton;
    [SerializeField] GameObject _carSelectedText;
    [SerializeField] GameObject _upgradePanel;
    [SerializeField] UpgradeUI _upgradeUI;
    void Start()
    {
        _carModels = GameManager.instance.CarModels;
        for (int i = 0; i < _carModels.Count; i++)
        {
            _carModels[i].SetActive(false);
            _carModels[i].transform.position = _displayPos.position;

            if (GameManager.instance.CurrentCarInUse == _carModels[i])
            {
                _currentDisplaying = i;
            }
        }
        _carModels[_currentDisplaying].SetActive(true);
        _carModels[_currentDisplaying].transform.position = _displayPos.position;
        CheckCar();



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
                ChangeCar(nextcar);
            }
            else
            {
                ChangeCar(_currentDisplaying + 1);
            }
        } 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentDisplaying == 0)
            {
                ChangeCar(_carModels.Count-1);
            }
            else
            {
                ChangeCar(_currentDisplaying - 1);
            }
        }
    }

   private void ChangeCar(int carToShow)
    {
       
        _carModels[_currentDisplaying].SetActive(false);
        _currentDisplaying = carToShow;
        _carModels[_currentDisplaying].transform.eulerAngles = new Vector3(0, 130, 0);
        _carModels[_currentDisplaying].SetActive(true);
        CheckCar();



    }
    private void CheckCar()
    {
        if (_carModels[_currentDisplaying]==GameManager.instance.CurrentCarInUse)
        {
            _carSelectedText.SetActive(true);
            _buyCarbutton.SetActive(false);
            _changeCarbutton.SetActive(false);
            _upgradePanel.SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        }
        else if(GameManager.instance.PlayerCarsBought.Contains(_carModels[_currentDisplaying]))
        {
            _carSelectedText.SetActive(false);
            _buyCarbutton.SetActive(false);
            _changeCarbutton.SetActive(true);
            _upgradePanel.SetActive(true);
            _upgradeUI.CkeckUpgrades(_currentDisplaying,_carModels);
        }
        else
        {
            _carSelectedText.SetActive(false);
            _buyCarbutton.SetActive(true);
            _changeCarbutton.SetActive(false);
            _upgradePanel.SetActive(false);
            _upgradeUI.CkeckUpgrades(_currentDisplaying, _carModels);
        }
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.BrakeUpgradeLevel == 2)
        {
            _upgradeUI.BuyBrakesButton.SetActive(false);
        }   
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.MaxSpeedLevel == 2)
        {
            _upgradeUI.BuyMaxSpeedButton.SetActive(false);
        }    
        if (_carModels[_currentDisplaying].GetComponent<CarControllerTest>().Upgrades.AcelarationUpgradeLevel == 2)
        {
            _upgradeUI.BuyAcelarationButton.SetActive(false);
        }

    }
    public void BuyCarButton()
    {
        GameManager.instance.PlayerCarsBought.Add(_carModels[_currentDisplaying]);
        GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
        CheckCar();
    }
    public void ChangeCarButton()
    {
        GameManager.instance.CurrentCarInUse = _carModels[_currentDisplaying];
        CheckCar();
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
        GameManager.instance.CurrentCarInUse.transform.position = GameManager.instance.CarExitStorePos.position;
        GameManager.instance.CurrentCarInUse.transform.rotation = GameManager.instance.CarExitStorePos.rotation;
        SceneManager.UnloadSceneAsync("Store");




    }

}
