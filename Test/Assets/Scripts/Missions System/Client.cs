using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour
{
    [SerializeField] Transform[] _goalLocalization;
    
    [SerializeField] bool _isClientIn = false;
    CellPhone _phone;
    Timer _timer;
    [SerializeField] GameObject _casio;

    //Distacia entre pontos
    [SerializeField] float _distance;
    [SerializeField] int _loc;
  
   

    private void Awake()
    {
        _timer = GameObject.Find("Timer").GetComponent<Timer>();
        _phone = GameObject.Find("CellPhone").GetComponent<CellPhone>();
    }

    private void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isClientIn == false)
        {
           
            _isClientIn = true;
            
            StartCoroutine(SpwanGoal());



        }
        else if (other.tag == "Player" && _isClientIn == true)
        {
            _phone.IsMissonOn = false;
            _timer.StopTimer();
            
            Destroy(gameObject);

        }
    }

   
    IEnumerator SpwanGoal()
    {
            _loc = Random.Range(0, _goalLocalization.Length);
            _distance = Vector3.Distance(gameObject.transform.position, _goalLocalization[_loc].transform.position);
        if (_distance < 55)
        {
            StartCoroutine(SpwanGoal());
        }
        else
        {
            _timer.StartMinutes = (int)_distance / 4;
            _timer.StartTimer();
            gameObject.transform.position = _goalLocalization[_loc].position;
        }
            yield return new WaitForSeconds(0.1f);
    }





   
}
