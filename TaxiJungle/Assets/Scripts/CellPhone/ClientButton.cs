using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClientButton:MonoBehaviour
{  
    [SerializeField] Clients _client;
    UiManager _uiManager;

    public Clients Client { get => _client; set => _client = value; }
    public UiManager UiManager { get => _uiManager; set => _uiManager = value; }

    public void StartStoryMissonButton()
    {
        //UiManager.CellPhone.StartStoryMisson(_client);
    }
}
