using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Mission
{

    public abstract void StartMission();
}

class ABMission : Mission
{
    Vector3 _target;
    UiManager _uiManager;
    float _currentTime;
    
    public ABMission(Vector3 targetPosition)
    {
        _target = targetPosition;
        
    }
    public override void StartMission()
    {
        //Lógica do que acontece quando a missão começa
        StartTimer();
        SetGoalPosition();
       
    }

    private void SetGoalPosition()
    {
        
    }
  
    public void StartTimer()
    {
       
        _currentTime = _currentTime - Time.deltaTime;

        if (_currentTime <= 0)
        {
            // quando chega a 0 o tempo aumenta um pouco mas o reward do player diminui
            //ShowTimer(0);

            Debug.Log("lose");
           // _gameManager.IsMissonOn = false;

        }

        //TimeSpan time = TimeSpan.FromSeconds(_curentTimerTime);
        //_currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        
        _uiManager.ShowTimer(10);
    }
    
}
