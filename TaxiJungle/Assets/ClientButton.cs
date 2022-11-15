using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientButton : CellPhone
{
    
    [SerializeField] GameObject _phone;
    [SerializeField] private  Client _client;


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
