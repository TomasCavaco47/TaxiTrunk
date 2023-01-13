using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    GameManager _gameManager;
    public static UiManager instance;

    [Header("CellPhone")]
    
    [SerializeField] GameObject _phoneImage;
    [SerializeField] GameObject _phoneFirstMenu, _phoneQuickMissonMenu, _phoneStoryMissonMenu, _alreadyMenu;
    [SerializeField] GameObject _firstButtonMenu1, _firstButtonQuickMisson, _firstButtonStoryMisson;
    [Header("StoryMissons")]
    [SerializeField] GameObject _buttonPrefab;
    [SerializeField] GameObject _Parrent;
    int _indexClient = 0;
    

    [Header("Timer")]
    [SerializeField] GameObject _timerObject;
    [SerializeField] Text _currentTimeText;

    [SerializeField] GameObject _dialogueObject;

    [Header("Gps")]
    [SerializeField] GameObject _gps;
    [SerializeField] Transform _miniMapCam;
    [SerializeField] float _miniMapSize;
    Vector3 _v3 = new Vector3(0, 30, 0);

    [Header("Refs")]
    [SerializeField] ScrolSysteam _scrolSysteam;


    [SerializeField] List<Client> _clients = new List<Client>();


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
        _gameManager = GameManager.instance;
    }

    private void Update()
    {

        GpsAllwaysInMap();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddNewButton();
        }
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
   
    public void OpenPhone()
    {
        if (_phoneImage.activeSelf == false)
        {
            _phoneQuickMissonMenu.SetActive(false);
        _phoneStoryMissonMenu.SetActive(false);
        _phoneImage.SetActive(true);
        _phoneFirstMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstButtonMenu1);
      
        }
    }
    public void ClosePhone()
    {
        if (_phoneFirstMenu.activeSelf == false)
        {
            BackButton();
        }
        else if (_phoneFirstMenu.activeSelf == true)
        {
            _phoneImage.SetActive(false);
        }

    }

    public void OpenQuickMissonMenu()
    {
        
        _phoneFirstMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstButtonQuickMisson);
        _phoneQuickMissonMenu.SetActive(true);
        

    }

    public void OpenStoryMenu()
    {
        _scrolSysteam.IndexButton=0;
        _phoneFirstMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstButtonStoryMisson);
        _phoneStoryMissonMenu.SetActive(true);
        
    }

    public void BackButton()
    {
       
       _phoneQuickMissonMenu.SetActive(false);
       _phoneStoryMissonMenu.SetActive(false);
        _alreadyMenu.SetActive(false);
       EventSystem.current.SetSelectedGameObject(null);
       EventSystem.current.SetSelectedGameObject(_firstButtonMenu1);
       _phoneFirstMenu.SetActive(true);
       
    }

    public void AlreadyInService()
    {
        _phoneFirstMenu.SetActive(false);
        _phoneQuickMissonMenu.SetActive(false);
        _phoneStoryMissonMenu.SetActive(false);
        _alreadyMenu.SetActive(true);

    }
    public void AddNewButton()
    {
        GameObject buttonPrefab = Instantiate(_buttonPrefab);
        buttonPrefab.transform.SetParent(_Parrent.transform);
        buttonPrefab.GetComponent<ClientButton>().Client = _clients[_indexClient];
        _indexClient++;
        //buttonPrefab.transform.SetAsFirstSibling();
        _scrolSysteam.ButtonsList.Add(buttonPrefab);
        _firstButtonStoryMisson = buttonPrefab;
        _scrolSysteam.ValueAlterate();
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
        _v3 = new Vector3(goal.transform.position.x, _v3.y, goal.transform.position.z);
    }
    public void GpsOff()
    {
        _gps.SetActive(false);

    }
    public void GpsAllwaysInMap()
    {
        //isto é o que mexe o icon do gps para estar sempre a apareçer no mapa
        _gps.transform.position = _v3;
        _gps.transform.position = new Vector3(
       Mathf.Clamp(_gps.transform.position.x, _miniMapCam.position.x - _miniMapSize, _miniMapSize + _miniMapCam.position.x),
       30,
       Mathf.Clamp(_gps.transform.position.z, _miniMapCam.position.z - _miniMapSize, _miniMapSize + _miniMapCam.position.z));
    }

    #endregion


}
