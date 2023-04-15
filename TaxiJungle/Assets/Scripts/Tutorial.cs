using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        i = _missionManager.Missions.ArcOneMissions.Count;
        _missionManager.Missions.ArcOneMissions[0].Origin =_gameManager.CurrentCarInUse;
        _missionManager.StartStoryMissions();

       
    }

    // Update is called once per frame
    void Update()
    {
       if(_missionManager.Missions.ArcOneMissions.Count==i && _missionManager.MissionStarted==false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
    public void DestroyThisObject()
    {
        Destroy(this.gameObject);

    }
}
