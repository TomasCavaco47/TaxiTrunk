using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

using UnityEngine;

public class UiManager : MonoBehaviour
{
    private GameManager _gameManager;
    public static UiManager instance;

    [SerializeField] GameObject _cellPhoneImage;
    [SerializeField] GameObject _cellPhoneFirstMenu;
    [SerializeField] GameObject _cellPhoneSecoundMenu;

    [SerializeField] GameObject _timerObject;
    [SerializeField] private bool _startTimer = false;
    [SerializeField] private float _curentTimerTime;
    [SerializeField] Text _currentTimeText;
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
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if(_startTimer)
        {
            TickTimer();
        }
    }
    public void CellPhone()
    {
      if(_cellPhoneImage.activeSelf ==true)
        {
           _cellPhoneImage.SetActive(false);
       }
       else
       {
           _cellPhoneImage.SetActive(true);
       }

    }   

    public void CellPhoneStoryMissions()
    {

    }
    public void ShowTimer(int time)
    {
        if (_timerObject.activeSelf == true)
        {
            _timerObject.SetActive(false);
            _curentTimerTime = time;
            _startTimer = false;

        }
        else
        {
            _startTimer = true;
            _curentTimerTime = time;
            _timerObject.SetActive(true);


        }
    }
    void TickTimer()
    {
        _curentTimerTime = _curentTimerTime - Time.deltaTime;

            if (_curentTimerTime <= 0)
            {
            // quando chega a 0 o tempo aumenta um pouco mas o reward do player diminui
            ShowTimer(0);
            
            Debug.Log("lose");
            _gameManager.IsMissonOn = false;
            
            }
        
        TimeSpan time = TimeSpan.FromSeconds(_curentTimerTime);
        _currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }
}
