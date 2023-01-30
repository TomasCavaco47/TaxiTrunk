using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    MissionManager _missionManager;
    int _money;
    [SerializeField] private List<GameObject> _playerCarsBought;

    


  

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        _missionManager = MissionManager.instance;
    }



  
    
}
