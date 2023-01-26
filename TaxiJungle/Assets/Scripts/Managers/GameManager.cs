using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    MissionManager _missionManager;
    int _money;

    [SerializeField] private UiManager _uiManager;
    


    public UiManager UiManager { get => _uiManager; set => _uiManager = value; }
  

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
