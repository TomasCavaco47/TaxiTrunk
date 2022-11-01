using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    bool _timerActive = false;
    float _currentTime;
    [SerializeField] int _startMinutes;
    [SerializeField] Text _currentTimeText;
    [SerializeField] GameObject _casio;

    public float CurrentTime { get => _currentTime; set => _currentTime = value; }
    public int StartMinutes { get => _startMinutes; set => _startMinutes = value; }

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = _startMinutes /** 60*/;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerActive == true)
        {
            _currentTime = _currentTime - Time.deltaTime;
            if (_currentTime <= 0)
            {
                _timerActive = false;
                Start();
            }
        }
        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        _currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }

    public void StartTimer()
    {
        _casio.SetActive(true);
        _currentTime = _startMinutes /** 60*/;
        _timerActive = true;
    }
    public void StopTimer()
    {
        _casio.SetActive(false);
        _timerActive = false;
    }
}
