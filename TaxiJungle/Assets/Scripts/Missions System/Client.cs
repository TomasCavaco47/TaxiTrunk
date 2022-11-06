using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class Client : MonoBehaviour
{
    

    [SerializeField] Transform[] _goalLocalization;
    
    [SerializeField] bool _isClientIn = false;
    [SerializeField] GameObject _casio;
    private GameManager _gameManager;

    //Distacia entre pontos
    [SerializeField] float _distance;
    [SerializeField] int _loc;
  
   

  
    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    void SpawnGoal()
    {
        _loc = Random.Range(0, _goalLocalization.Length);
        _distance = Vector3.Distance(gameObject.transform.position, _goalLocalization[_loc].transform.position);
        if (_distance < 55)
        {
            SpawnGoal();

        }
        else
        {
            _gameManager.UiManager.ShowTimer((int)_distance / 4);
            gameObject.transform.position = _goalLocalization[_loc].position;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isClientIn == false)
        {
           
            _isClientIn = true;
            
            SpawnGoal();



        }
        else if (other.tag == "Player" && _isClientIn == true)
        {
            _gameManager.IsMissonOn = false;
            _gameManager.UiManager.ShowTimer(0);
           
            
            Destroy(gameObject);

        }
    }
   
}
