using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{
    private GameManager _gameManager;
    private protected UiManager _uiManager;
   [SerializeField] private protected MissionManager _missionManager;

    

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
        //PhoneInputs();
    }
    //void PhoneInputs()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1) && _gameManager.IsMissonOn == false)
    //    {
    //        if (_missionManager.MissionStarted == false)
    //        {
    //        _missionManager.StartQuickMissions();
    //            gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            Debug.Log("InMission");
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2) && _gameManager.IsMissonOn == false)
    //    {
    //        if (_missionManager.MissionStarted == false)
    //        {
    //            _missionManager.StartStoryMissions(_client);
    //        }
    //        else
    //        {
    //            Debug.Log("InMission");
    //        }
    //    }
    //}

    public void QuickMissonButton()
    {
        
            if (_missionManager.MissionStarted == false)
            {
                _missionManager.StartQuickMissions();
                gameObject.SetActive(false);
            }
            else
            {
            _uiManager.AlreadyInService();
                Debug.Log("InMission");
            }
        
    }
  

   
   
}
