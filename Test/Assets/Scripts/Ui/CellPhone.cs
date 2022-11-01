using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellPhone : MonoBehaviour
{

    Action _passagerSpwanEvent;
    bool _isPhoneOpen = false;
    bool _isMissonOn = false;
    [SerializeField] GameObject _cellPhone;

    public Action PassagerSpwanEvent { get => _passagerSpwanEvent; set => _passagerSpwanEvent = value; }
    public bool IsMissonOn { get => _isMissonOn; set => _isMissonOn = value; }

    public void UseCellPhone()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && _isPhoneOpen == false)
        {
            _cellPhone.SetActive(true);
            _isPhoneOpen = true;
            //triger da animação
        }
        else if (Input.GetKeyDown(KeyCode.Mouse2) && _isPhoneOpen == true)
        {
            _cellPhone.SetActive(false);
            _isPhoneOpen = false;
            //triger da animação a sair
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1) && _isPhoneOpen == true && _isMissonOn == false)
        {
            _passagerSpwanEvent.Invoke();
            _cellPhone.SetActive(false);
            _isMissonOn = true;

        }



    }
}
