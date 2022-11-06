using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{

    private Action  _startQuickMission;
    private GameManager _gameManager;
   [SerializeField] private MissionManager _missionManager;

    public Action StartQuickMission { get => _startQuickMission; set => _startQuickMission = value; }

    private void Start()
    {
        _gameManager = GameManager.instance;
    }
    private void Update()
    {
        PhoneInputs();
    }
    void PhoneInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _gameManager.IsMissonOn == false)
        {
            _missionManager.StartQuickMissions();
            //StartQuickMission.Invoke();
            //_gameManager.IsMissonOn = true;
            //_gameManager.UiManager.CellPhone();


        }
    }
   
}
