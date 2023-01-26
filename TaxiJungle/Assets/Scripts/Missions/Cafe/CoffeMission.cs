using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoffeMission : MonoBehaviour
{
    MissionManager _missionManager;
    CarControllerTest _playerCar;
    [SerializeField]Image _coffeImage;
    float _coffeSize;
    [SerializeField] float _currentCoffe;

    float _timer;
    private void Awake()
    {
        _missionManager = MissionManager.instance;
        _playerCar = _missionManager.PlayerCar;
        _coffeSize = Random.Range(30, 100);
        _currentCoffe = _coffeSize;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >0.01f)
        {
            if(_playerCar.CurrentSpeed > 50 && Input.GetAxis("Horizontal")== 1 || Input.GetAxis("Horizontal") ==-1)
            {
                _currentCoffe -= 0.2f;
                UpdateCoffeImage();
            }
            _timer = 0.0f;
            
        }
        if(_currentCoffe <=0)
        {
            _missionManager.LostMission();
        }
    }

    private void UpdateCoffeImage()
    {
        _coffeImage.fillAmount = _currentCoffe/_coffeSize;
    }
}

