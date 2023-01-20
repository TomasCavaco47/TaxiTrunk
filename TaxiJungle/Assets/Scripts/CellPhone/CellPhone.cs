using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{
   [SerializeField] protected UiManager _uiManager;
    [SerializeField] List<Client> _clientsAdded;
   [SerializeField] protected MissionManager _missionManager;

    private void Awake()
    {
        _uiManager = UiManager.instance;

    }

    protected virtual void Start()
    {
        _missionManager = MissionManager.instance;
       
    }
    private void OnEnable()
    {
       
        
    }

 


    //temporario para testes
   
}
