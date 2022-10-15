using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour
{
    [SerializeField] CarControllerScript _car;
    [SerializeField] Transform[] _goalLocalization;
    
    [SerializeField] bool _isClientIn = false;
    CellPhone _phone;

    //caso fazer o spawn do Goal com envento.
    //Action _goalSpwan;
    //public Action GoalSpwan { get => _goalSpwan; set => _goalSpwan = value; }

    private void Awake()
    {
        _car = GameObject.Find("Free Racing Car (1)").GetComponent<CarControllerScript>();
        _phone = GameObject.Find("CellPhone").GetComponent<CellPhone>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(_car != null && _isClientIn == false)
        {
            _isClientIn = true;
            int _loc = Random.Range(0, _goalLocalization.Length);
            gameObject.transform.position = _goalLocalization[_loc].position;
            //_goalSpwan.Invoke();
        }
        else if (_car != null && _isClientIn == true)
        {
            _phone.IsMissonOn = false;
            Destroy(gameObject);
            
        }
    }

   
}
