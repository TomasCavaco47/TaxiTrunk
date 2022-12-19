using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientButton : CellPhone
{  
    [SerializeField] private  Client _client;
    [SerializeField] TMP_Text _ContactName;
    [SerializeField] 

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
