using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMotorSound : MonoBehaviour
{
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _currentSpeed;

    private Rigidbody _carRb;
    private AudioSource _carAudio;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    private float _pitchFromCar;

    private void Start()
    {
        _carAudio = GetComponent<AudioSource>();
        _carRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(UiManager.instance.GameIsPaused == false && GameManager.instance.InStore == false)
        {
            _carAudio.UnPause();
            EngineSound();
        }
        else
        {
            _carAudio.Pause();
        }
    }

    void EngineSound()
    {
        _currentSpeed = _carRb.velocity.magnitude;
        _pitchFromCar = _carRb.velocity.magnitude / 50f;

        if(_currentSpeed < _minSpeed)
        {
            _carAudio.pitch = _minPitch;
        }

        if(_currentSpeed > _minSpeed && _currentSpeed < _maxSpeed) 
        {
            _carAudio.pitch = _minPitch + _pitchFromCar;
        }

        if(_currentSpeed > _maxSpeed)
        {
            _carAudio.pitch = _maxPitch;
        }
    }
}
