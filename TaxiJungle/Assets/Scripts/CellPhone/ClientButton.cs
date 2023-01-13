using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientButton : CellPhone
{  
    [SerializeField] Client _client;

    public Client Client { get => _client; set => _client = value; }

    protected override void Start()
    {
        base.Start();
        _missionManager = MissionManager.instance;

    }


    public void StoryMissonButton()
    {


        if (_missionManager.MissionStarted == false)
        {

            _missionManager.StartStoryMissions(_client);
            _uiManager.ClosePhone();
           
        }
        else
        {
            _uiManager.AlreadyInService();
            Debug.Log("InMission");
        }

    }
}
