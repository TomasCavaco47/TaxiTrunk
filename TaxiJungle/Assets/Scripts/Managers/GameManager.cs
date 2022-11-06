using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private UiManager _uiManager;
    private bool _isMissonOn = false;


    public UiManager UiManager { get => _uiManager; set => _uiManager = value; }
    public bool IsMissonOn { get => _isMissonOn; set => _isMissonOn = value; }

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

    void Update()
    {
        OpenPhone();
    }
    void OpenPhone()
    {
        if(IsMissonOn)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            _uiManager.CellPhone();
        }
    }
    
}
