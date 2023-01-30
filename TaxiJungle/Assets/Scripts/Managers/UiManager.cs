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
    [SerializeField] GameObject _phoneFirstButtonSelected, _quickMissionFirstButtonSelected, _storyMissionButtonSelected;
    //[SerializeField] ScrolSysteam _scrollSystem;
    bool _missionMenuOppened;
    //int _indexClient;
    // GameObject _gridLayoutGroup;
    //[SerializeField] GameObject _buttonPrefab;




    public GameObject PhoneQuickMissonMenu { get => _phoneQuickMissonMenu; set => _phoneQuickMissonMenu = value; }
    public GameObject PhoneStoryMissonMenu { get => _phoneStoryMissonMenu; set => _phoneStoryMissonMenu = value; }
    public bool MissionMenuOppened { get => _missionMenuOppened; set => _missionMenuOppened = value; }
    public GameObject PhoneFirstButtonSelected { get => PhoneFirstButtonSelected1; set => PhoneFirstButtonSelected1 = value; }
    public GameObject StoryMissionButtonSelected { get => _storyMissionButtonSelected; set => _storyMissionButtonSelected = value; }
    public GameObject PhoneFirstButtonSelected1 { get => _phoneFirstButtonSelected; set => _phoneFirstButtonSelected = value; }
    public GameObject QuickMissionFirstButtonSelected { get => _quickMissionFirstButtonSelected; set => _quickMissionFirstButtonSelected = value; }

    public void OpenPhone()
    {
        if (_phoneImage.activeSelf == false)
        {

            //  _phoneQuickMissonMenu.SetActive(false);
            // _phoneStoryMissonMenu.SetActive(false);
            _phoneImage.SetActive(true);
            //_phoneFirstMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(PhoneFirstButtonSelected);
            
        }
    }
    public void ClosePhone()
    {
        if (_missionMenuOppened)
        {
            BackButton();
            _missionMenuOppened = false;
        }
        else
        {
            _phoneImage.SetActive(false);
        }
    }
    public void StartStoryMissonButton()
    {
        // acho que nao se devia vir aqui quando ja estamos em missão

        if (MissionManager.instance.MissionStarted == false)
        {

            MissionManager.instance.StartStoryMissions();
            ClosePhone();

        }
        else
        {
            AlreadyInService();
            Debug.Log("InMission");
        }

    }
    public void StartQuickMissonButton()
    {

        if (MissionManager.instance.MissionStarted == false)
        {
            MissionManager.instance.StartQuickMissions();
            ClosePhone();
        }
        else
        {
            AlreadyInService();
            Debug.Log("InMission");
        }

    }
    private void BackButton()
    {
        _phoneQuickMissonMenu.SetActive(false);
        _phoneStoryMissonMenu.SetActive(false);
        _inServiceMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PhoneFirstButtonSelected);
        // _phoneFirstMenu.SetActive(true);
    }
    private void AlreadyInService()
    {
        _phoneQuickMissonMenu.SetActive(false);
        _phoneStoryMissonMenu.SetActive(false);
        _missionMenuOppened = true;
        _inServiceMenu.SetActive(true);
    }

}

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    
    [SerializeField] Phone _cellPhone;

    [Header("Timer")]
    [SerializeField] GameObject _timerObject;
    [SerializeField] Text _currentTimeText;

    [SerializeField] GameObject _dialogueObject;

    [Header("Gps")]
    [SerializeField] GameObject _gps;
    [SerializeField] Transform _miniMapCam;
    [SerializeField] float _miniMapSize;
    Vector3 _gpsVector = new Vector3(0, 30, 0);

   // [Header("Refs")]

    public Phone CellPhone { get => _cellPhone; set => _cellPhone = value; }

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
        //CellPhone.GridLayoutGroup = CellPhone.ScrollSystem.transform.GetChild(0).GetChild(0).gameObject;
    }
    private void Update()
    {
            GpsAllwaysInMap();
        OpenPhone();
        ClosePhone();
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
    
    private void OpenPhone()
    {
        //List<Client> clientsList=null;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // precisa de melhorias
            CellPhone.OpenPhone();
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
   private  void ClosePhone()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CellPhone.ClosePhone();
        }
    }
    public void OpenQuickMissonMenu()
    {
        CellPhone.MissionMenuOppened = true;
        CellPhone.PhoneQuickMissonMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CellPhone.QuickMissionFirstButtonSelected);
    }

    public void OpenStoryMenu()
    {
        CellPhone.MissionMenuOppened = true;
       // CellPhone.ScrollSystem.IndexButton = 0;
        //_phoneFirstMenu.SetActive(false);
        CellPhone.PhoneStoryMissonMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CellPhone.StoryMissionButtonSelected);

    }


    public void StartQuickMissonButton()
    {
        CellPhone.StartQuickMissonButton();
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
    }
    public void CloseDialogue()
    {
        _dialogueObject.SetActive(false);
       
    }
    #endregion
    #region Gps;
    public void GpsOn(Transform goal)
    {
        _gpsVector = new Vector3(goal.transform.position.x, _gpsVector.y, goal.transform.position.z);
        _gps.SetActive(true);
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
