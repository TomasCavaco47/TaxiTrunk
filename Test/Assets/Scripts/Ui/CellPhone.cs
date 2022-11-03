using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{

    private Action  _passagerSpwanEvent;
    private GameManager _gameManager;

    public Action PassagerSpwanEvent { get => _passagerSpwanEvent; set => _passagerSpwanEvent = value; }

    private void Start()
    {
        _gameManager = GameManager.instance;
    }
    private void Update()
    {
        PhoneInputs();
    }
    void PhoneInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _gameManager.IsMissonOn == false)
        {
            _passagerSpwanEvent.Invoke();
            _gameManager.IsMissonOn = true;
            _gameManager.UiManager.CellPhone();


        }
    }
   
}
