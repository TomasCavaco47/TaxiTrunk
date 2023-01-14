using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Phone
{
    [SerializeField] GameObject _phoneImage;
    [SerializeField] GameObject _phoneQuickMissonMenu, _phoneStoryMissonMenu, _inServiceMenu;
    [SerializeField] GameObject _phoneFirstButtonSelected, _quickMissionFirstButtonSelected, _storyMissionFirstButtonSelected;
    [SerializeField] ScrolSysteam _scrollSystem;
    bool _missionMenuOppened;
    [SerializeField] List<Client> _clientsAdded;

    public GameObject PhoneImage { get => _phoneImage; set => _phoneImage = value; }
    public GameObject PhoneQuickMissonMenu { get => _phoneQuickMissonMenu; set => _phoneQuickMissonMenu = value; }
    public GameObject PhoneStoryMissonMenu { get => _phoneStoryMissonMenu; set => _phoneStoryMissonMenu = value; }
    public GameObject InServiceMenu { get => _inServiceMenu; set => _inServiceMenu = value; }
    public GameObject PhoneFirstButtonSelected { get => _phoneFirstButtonSelected; set => _phoneFirstButtonSelected = value; }
    public GameObject QuickMissionFirstButtonSelected { get => _quickMissionFirstButtonSelected; set => _quickMissionFirstButtonSelected = value; }
    public GameObject StoryMissionFirstButtonSelected { get => _storyMissionFirstButtonSelected; set => _storyMissionFirstButtonSelected = value; }
    public ScrolSysteam ScrollSystem { get => _scrollSystem; set => _scrollSystem = value; }
    public bool MissionMenuOppened { get => _missionMenuOppened; set => _missionMenuOppened = value; }
    public List<Client> ClientsAdded { get => _clientsAdded; set => _clientsAdded = value; }
}

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    
    [SerializeField] Phone _cellPhone;
    //[SerializeField] GameObject _phoneImage;
    //[SerializeField] GameObject _phoneQuickMissonMenu, _phoneStoryMissonMenu, _inServiceMenu;
    //[SerializeField] GameObject _phoneFirstButtonSelected, _quickMissionFirstButtonSelected, _storyMissionFirstButtonSelected;
    //[SerializeField] ScrolSysteam _scrollSystem;
    //bool _missionMenuOppened;
    [Header("StoryMissons")]
    [SerializeField] GameObject _buttonPrefab;
    // _scrollSystem child child
    GameObject _gridLayoutGroup;
    int _indexClient = 0;
    

    [Header("Timer")]
    [SerializeField] GameObject _timerObject;
    [SerializeField] Text _currentTimeText;

    [SerializeField] GameObject _dialogueObject;

    [Header("Gps")]
    [SerializeField] GameObject _gps;
    [SerializeField] Transform _miniMapCam;
    [SerializeField] float _miniMapSize;
    Vector3 _gpsVector = new Vector3(0, 30, 0);

    [Header("Refs")]
   


    [SerializeField] List<Client> _clients = new List<Client>();


    private void Awake()
    {
        _gridLayoutGroup = _cellPhone.ScrollSystem.transform.GetChild(0).GetChild(0).gameObject;
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
        GpsAllwaysInMap();

        //retirar
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    AddNewButton();
        //}
    }
    public void ShowTimer(bool active, float timer)
    {
        _timerObject.SetActive(active);
        if (active)
        {
            
            TimeSpan time = TimeSpan.FromSeconds(timer);
            _currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }
    }
    #region CellPhone
   
    public void OpenPhone(List<Client> clientsList)
    {
        

        if (_cellPhone.PhoneImage.activeSelf == false)
        {
           

            //  _phoneQuickMissonMenu.SetActive(false);
            // _phoneStoryMissonMenu.SetActive(false);
            _cellPhone.PhoneImage.SetActive(true);
        //_phoneFirstMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_cellPhone.PhoneFirstButtonSelected);
            if (_cellPhone.ClientsAdded.Count == 0)
            {
                _cellPhone.ClientsAdded.Add(clientsList[0]);
                AddNewButton(clientsList[0]);
                Debug.Log("addedfirst");

            }
            if (_cellPhone.ClientsAdded.Count != 0)
            {
                for (int i = 0; i < clientsList.Count; i++)
                {
                 int timesFounded = 0;
                    Debug.Log("i = " +i);

                    for (int x = 0; x < _cellPhone.ClientsAdded.Count; x++)
                    {

                        Debug.Log("x = "+x);
                        if (clientsList[i] != _cellPhone.ClientsAdded[x])
                        {
                          
                            
                            Debug.Log("not there");

                        }
                        else
                        {
                            timesFounded++;
                            Debug.Log("already there");

                        }
                    }
                    if(timesFounded==0)
                    {
                        Debug.Log("added");
                        _cellPhone.ClientsAdded.Add(clientsList[i]);
                        AddNewButton(clientsList[i]);
                    }

                }
            }
        }
    }
    public void ClosePhone()
    {
        //if (_phoneFirstMenu.activeSelf == false)
        //{
        //    BackButton();
        //}
        //else if (_phoneFirstMenu.activeSelf == true)
        //{
        //    _phoneImage.SetActive(false);
        //}

        if (_cellPhone.MissionMenuOppened)
        {
            BackButton();
            _cellPhone.MissionMenuOppened = false;
        }
        else 
        {
            _cellPhone.PhoneImage.SetActive(false);
        }

    }

    public void OpenQuickMissonMenu()
    {
        _cellPhone.MissionMenuOppened = true;
        _cellPhone.PhoneQuickMissonMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_cellPhone.QuickMissionFirstButtonSelected);              
    }

    public void OpenStoryMenu()
    {
        _cellPhone.MissionMenuOppened = true;
        _cellPhone.ScrollSystem.IndexButton=0;
        //_phoneFirstMenu.SetActive(false);
        _cellPhone.PhoneStoryMissonMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_cellPhone.StoryMissionFirstButtonSelected);

    }

    public void BackButton()
    {

        _cellPhone.PhoneQuickMissonMenu.SetActive(false);
        _cellPhone.PhoneStoryMissonMenu.SetActive(false);
        _cellPhone.InServiceMenu.SetActive(false);
       EventSystem.current.SetSelectedGameObject(_cellPhone.PhoneFirstButtonSelected);
      // _phoneFirstMenu.SetActive(true);

    }

    public void AlreadyInService()
    {
        _cellPhone.PhoneQuickMissonMenu.SetActive(false);
        _cellPhone.PhoneStoryMissonMenu.SetActive(false);
        _cellPhone.MissionMenuOppened = true;
        _cellPhone.InServiceMenu.SetActive(true);

    }
    //temporario para testes
    public void AddNewButton(Client client)
    {

        GameObject buttonPrefab = Instantiate(_buttonPrefab);
        buttonPrefab.transform.SetParent(_gridLayoutGroup.transform);
        buttonPrefab.GetComponent<ClientButton>().Client = client;
        _indexClient++;
        //buttonPrefab.transform.SetAsFirstSibling();
        _cellPhone.ScrollSystem.ButtonsList.Add(buttonPrefab);
        _cellPhone.StoryMissionFirstButtonSelected = buttonPrefab;
        _cellPhone.ScrollSystem.ValueAlterate();
    }

    #endregion
    #region Dialogue
    public void Dialogue( Sprite sprite, string text)
    {
        _dialogueObject.SetActive(true);
        _dialogueObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        _dialogueObject.transform.GetChild(1).GetComponent<Text>().text = text;
    }
    public void CloseDialogue()
    {
        _dialogueObject.SetActive(false);
       
    }
    #endregion
    #region Gps;
    public void GpsOn(Transform goal)
    {
        _gps.SetActive(true);
        _gpsVector = new Vector3(goal.transform.position.x, _gpsVector.y, goal.transform.position.z);
    }
    public void GpsOff()
    {
        _gps.SetActive(false);

    }
    public void GpsAllwaysInMap()
    {
        //isto é o que mexe o icon do gps para estar sempre a apareçer no mapa
        _gps.transform.position = _gpsVector;
        _gps.transform.position = new Vector3(
       Mathf.Clamp(_gps.transform.position.x, _miniMapCam.position.x - _miniMapSize, _miniMapSize + _miniMapCam.position.x),
       30,
       Mathf.Clamp(_gps.transform.position.z, _miniMapCam.position.z - _miniMapSize, _miniMapSize + _miniMapCam.position.z));
    }

    #endregion


}
