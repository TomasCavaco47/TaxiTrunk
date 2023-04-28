using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]

public class Phone
{
    [SerializeField] GameObject _phoneImage;
    [SerializeField] GameObject _quickTab, _storyTab, _inServiceMenu;
    [SerializeField] GameObject  _quickTabFirstButton, _storyTabButton;
    [SerializeField] List<GameObject> _buttonsDescriptonList = new List<GameObject>();
    [SerializeField] List<GameObject> _buttonsList = new List<GameObject>();
    [SerializeField] Button _startStoryMissionButton;
    [SerializeField] EventSystem _eventSystem;
    bool _missionMenuOppened;
    public GameObject QuickTab { get => _quickTab; set => _quickTab = value; }
    public GameObject StoryTab { get => _storyTab; set => _storyTab = value; }
    public GameObject QuickTabFirstButton { get => _quickTabFirstButton; set => _quickTabFirstButton = value; }
    public GameObject StoryTabButton { get => _storyTabButton; set => _storyTabButton = value; }
    public void OpenClosePhone()
    {
        if (_phoneImage.activeSelf == false)
        {
            QuickTab.SetActive(false);
            StoryTab.SetActive(true);
            _phoneImage.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(StoryTabButton);
            
            
        }else
        {
            if (_missionMenuOppened)
            {
                _missionMenuOppened = false;
            }
            else
            {
                _phoneImage.SetActive(false);
                
            }
        }
        if(MissionManager.instance.Missions.ArcOneMissions.Count == 0)
        {
            _startStoryMissionButton.interactable = false;
        }
    }
    public void OpenStorytab()
    {
        QuickTab.SetActive(false);
        StoryTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_storyTabButton);
    }
    public void OpenQuickTab()
    {
        QuickTab.SetActive(true);
        StoryTab.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_quickTabFirstButton);
    }
    public void ShowQuickButtonsInfo()
    {
        if(_eventSystem.currentSelectedGameObject == _buttonsList[0])
        {
            _buttonsDescriptonList[1].SetActive(false);
            _buttonsDescriptonList[2].SetActive(false);
            _buttonsDescriptonList[0].SetActive(true);
        }
        if (_eventSystem.currentSelectedGameObject == _buttonsList[1])
        {
            _buttonsDescriptonList[0].SetActive(false);
            _buttonsDescriptonList[2].SetActive(false);
            _buttonsDescriptonList[1].SetActive(true);
        }
        if (_eventSystem.currentSelectedGameObject == _buttonsList[2])
        {
            _buttonsDescriptonList[0].SetActive(false);
            _buttonsDescriptonList[1].SetActive(false);
            _buttonsDescriptonList[2].SetActive(true);
        }
    }
    public void StartStoryMissonButton()
    {
        // acho que nao se devia vir aqui quando ja estamos em missão

        if (MissionManager.instance.MissionStarted == false)
        {

            MissionManager.instance.StartStoryMissions();
            OpenClosePhone();

        }
        else
        {
            AlreadyInService();
            Debug.Log("InMission");
        }

    }
    public void StartQuickMissonButton(int i)
    {

        if (MissionManager.instance.MissionStarted == false)
        {
            MissionManager.instance.StartQuickMissions(i);
            OpenClosePhone();
        }
        else
        {
            AlreadyInService();
            Debug.Log("InMission");
        }

    }
    
    private void AlreadyInService()
    {
        _quickTab.SetActive(false);
        StoryTab.SetActive(false);
        _missionMenuOppened = true;
        _inServiceMenu.SetActive(true);
    }

}

public class UiManager : MonoBehaviour
{
    [Header("MisionButtonInfo")]
    [SerializeField] TMP_Text _clientName;
    [SerializeField] Image _clientIconSlot;
    [SerializeField] TMP_Text _missionDiscription;

    

    [Header("refs")]
    [SerializeField] Phone _cellPhone;
    [SerializeField] CarControllerTest _car;
    [SerializeField] GameObject _inGameUi;
    [SerializeField] GameObject _enterStoreText;
    [SerializeField] GameObject _pausePanel;
    public static UiManager instance;

    [Header("Speedometer")]
    [SerializeField] Image _visualSpeedometer;
    [SerializeField] Text _numbersSpeedometer;

    [Header("Timer")]
    [SerializeField] GameObject _timerObject;
    [SerializeField] Text _currentTimeText;

    [SerializeField] GameObject _dialogueObject;

    [Header("Gps")]
    [SerializeField] LineAI _lineAI;
    [SerializeField] GameObject _lineRender;
    [SerializeField] GameObject _gps;
    [SerializeField] Transform _miniMapCam;
    [SerializeField] float _miniMapSize;
    [SerializeField] bool gameIsPaused=false;
    Vector3 _gpsVector = new Vector3(0, 30, 0);

    [SerializeField] Text _money;

   // [Header("Refs")]

    public Phone CellPhone { get => _cellPhone; set => _cellPhone = value; }
    public CarControllerTest Car { get => _car; set => _car = value; }
    public GameObject InGameUi { get => _inGameUi; set => _inGameUi = value; }
    public GameObject EnterStoreText { get => _enterStoreText; set => _enterStoreText = value; }
    public GameObject PausePanel { get => _pausePanel; set => _pausePanel = value; }

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
    private void Start()
    {
        UiManager.instance.UpdateMoney();
        
    }
    private void Update()
    {
        
        switchTabs();
        GpsAllwaysInMap();
        OpenPhone();
        CellPhone.ShowQuickButtonsInfo();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            PauseGame();
        }
    }

    public void UpdateMoney()
    {
        _money.text = GameManager.instance.Money.ToString();
    }

    public void PauseGame()
    {
        
        if (GameManager.instance.InStore==false)
        {
            gameIsPaused = !gameIsPaused;
            if (gameIsPaused)
            {
                _pausePanel.SetActive(true);
                Time.timeScale = 0f;
                

            }
            else
            {
                _pausePanel.SetActive(false);
                Time.timeScale = 1;
                
            }
        }    
    }
    public void ExitGame()
    {
        Application.Quit();
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
    
    private void OpenPhone()
    {
        //List<Client> clientsList=null;
        if (Input.GetKeyDown(KeyCode.C) && MissionManager.instance.MissionStarted==false)
        {
            // precisa de melhorias
            CellPhone.OpenClosePhone();
            //clientsList = MissionManager.instance.PlacesAndClients.Clients;
            //if (CellPhone.ClientsAdded.Count == 0)
            //{
            //    CellPhone.ClientsAdded.Add(clientsList[0]);
            //    AddNewButton(clientsList[0]);
            //    Debug.Log("addedfirst");
            //}
            //if (CellPhone.ClientsAdded.Count != 0)
            //{
            //    for (int i = 0; i < clientsList.Count; i++)
            //    {
            //        int timesFounded = 0;
            //        Debug.Log("i = " + i);
            //        for (int x = 0; x < CellPhone.ClientsAdded.Count; x++)
            //        {
            //            Debug.Log("x = " + x);
            //            if (clientsList[i] != CellPhone.ClientsAdded[x])
            //            {
            //                Debug.Log("not there");
            //            }
            //            else
            //            {
            //                timesFounded++;
            //                Debug.Log("already there");
            //            }
            //        }
            //        if (timesFounded == 0)
            //        {
            //            Debug.Log("added");
            //            CellPhone.ClientsAdded.Add(clientsList[i]);
            //            AddNewButton(clientsList[i]);
            //        }
            //    }
            //}
        }
        
    }
 
    //public void OpenQuickMissonMenu()
    //{
    //    CellPhone.MissionMenuOppened = true;
    //    CellPhone.PhoneQuickMissonMenu.SetActive(true);
    //    EventSystem.current.SetSelectedGameObject(CellPhone.QuickMissionFirstButtonSelected);
    //}

    //public void OpenStoryMenu()
    //{
    //    CellPhone.MissionMenuOppened = true;
    //   // CellPhone.ScrollSystem.IndexButton = 0;
    //    //_phoneFirstMenu.SetActive(false);
    //    CellPhone.PhoneStoryMissonMenu.SetActive(true);
    //    EventSystem.current.SetSelectedGameObject(CellPhone.StoryMissionButtonSelected);

    //}
    public void switchTabs()
    {
        if (CellPhone.StoryTab.activeSelf == false && (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            CellPhone.OpenStorytab();
        }
        else if(CellPhone.QuickTab.activeSelf == false && (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            CellPhone.OpenQuickTab();
        }
    }


    public void StartQuickMissonButton(int i)
    {
        CellPhone.StartQuickMissonButton(i);
    }
    public void StartStoryMissonButton()
    {
        CellPhone.StartStoryMissonButton();
    }
    //private void AddNewButton(Clients client)
    //{
    //    GameObject buttonPrefab = Instantiate(CellPhone.ButtonPrefab);
    //    //buttonPrefab.transform.SetParent(CellPhone.GridLayoutGroup.transform);
    //    buttonPrefab.GetComponent<ClientButton>().Client = client;
    //    buttonPrefab.GetComponent<ClientButton>().UiManager = this;

    //    CellPhone.IndexClient++;
    //    //buttonPrefab.transform.SetAsFirstSibling();
    //   //CellPhone.ScrollSystem.ButtonsList.Add(buttonPrefab);
    //   // CellPhone.StoryMissionFirstButtonSelected = buttonPrefab;
    //   // CellPhone.ScrollSystem.ValueAlterate();
    //}



    #endregion
    #region Dialogue
    public void Dialogue( Sprite sprite, string text)
    {
        _dialogueObject.SetActive(true);
        _dialogueObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        _dialogueObject.transform.GetChild(1).GetComponent<Text>().text = text;
        if(sprite.name=="Vin")
        {
            _dialogueObject.transform.GetChild(2).GetComponent<Text>().text = "Vin";
        }
        else
        {
            _dialogueObject.transform.GetChild(2).GetComponent<Text>().text = MissionManager.instance.Missions.ArcOneMissions[0].Client.ClientName;

        }

    }
    public void CloseDialogue()
    {
        _dialogueObject.SetActive(false);
       
    }
    #endregion
    #region Gps;
    public void GpsOn(Transform goal)
    {
        _lineRender.SetActive(true);
        _lineAI.GoalT = goal;
        _gpsVector = new Vector3(goal.transform.position.x, _gpsVector.y, goal.transform.position.z);
        _gps.SetActive(true);
    }
    public void GpsOff()
    {
        _lineRender.SetActive(false);
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
    public void Speedometer(int currentSpeed, float MaxSpeed )
    {
        _visualSpeedometer.fillAmount = (currentSpeed / MaxSpeed);
        _numbersSpeedometer.text = currentSpeed.ToString();
    }
    public void OpenStore()
    {
        InGameUi.SetActive(false);
        SceneManager.LoadSceneAsync("Store",LoadSceneMode.Additive);
    }
    public void updateMissionButtonInfo(string clientName,Sprite clientImage, string description)
    {
        _clientName.text=clientName;
        _missionDiscription.text = description;
        _clientIconSlot.sprite = clientImage;
    }
}
