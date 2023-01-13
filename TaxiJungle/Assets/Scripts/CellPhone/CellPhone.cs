using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{
    GameManager _gameManager;
    protected UiManager _uiManager;
   [SerializeField] protected MissionManager _missionManager;

    protected virtual void Start()
    {
        _gameManager = GameManager.instance;
        _uiManager = UiManager.instance;
       
    }

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
