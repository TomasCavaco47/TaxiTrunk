using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    MissionManager _missionManager;
    GameManager _gameManager;
    int i=0;

    // Start is called before the first frame update
    void Start()
    {
        _missionManager = MissionManager.instance;
        _gameManager = GameManager.instance;
        _missionManager.Missions.ArcOneMissions[0].Origin =_gameManager.CurrentCarInUse;
        _missionManager.Missions.ArcOneMissions[1].Origin =_gameManager.CurrentCarInUse;
        _missionManager.StartStoryMissions();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(i<1)
        {
            if (_missionManager.Missions.ArcOneMissions.Count == 0)
            {
                i++;
                UiManager.instance!.OpenStore();
            }

        }
        if(i>0)
        {
            if(SceneManager.sceneCount==1)
            {
                SceneManager.LoadScene("PlayTest");
            }

        }
        if(_missionManager.MissionStarted==false)
        {
            GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = false;
        }
        else
        {
            GameManager.instance.CurrentCarInUse.GetComponent<CarControllerTest>().CanMove = true;
        }
        if(_missionManager.Missions.ArcOneMissions.Count == 2 && _missionManager.MissionStarted == false)
        {
            Debug.Log(_missionManager.MissionStarted);
            SceneManager.LoadScene("Tutorial");
        }
    }
}
