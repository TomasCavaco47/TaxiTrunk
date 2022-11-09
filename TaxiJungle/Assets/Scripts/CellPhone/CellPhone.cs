using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{

    private Action  _startQuickMission;
    private GameManager _gameManager;
    private UiManager _uiManager;
   [SerializeField] private MissionManager _missionManager;

    public Action StartQuickMission { get => _startQuickMission; set => _startQuickMission = value; }
    private void Awake()
    {
        
    }
    private void Start()
    {
        _gameManager = GameManager.instance;
        _uiManager = UiManager.instance;
    }
    private void Update()
    {
        PhoneInputs();
    }
    void PhoneInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _gameManager.IsMissonOn == false)
        {
            if (_missionManager.MissionStarted == false)
            {
            _missionManager.StartQuickMissions();
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("InMission");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _gameManager.IsMissonOn == false)
        {
            if (_missionManager.MissionStarted == false)
            {
                
            }
            else
            {
                Debug.Log("InMission");
            }
        }
    }
   
}
