using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiseShip : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] private float _currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_audioSource.isPlaying == false && _currentTime >= 327)
        {
            _audioSource.Play();
            _currentTime = 0;
        }
        else
        {
            _currentTime += Time.deltaTime;
        }

    }
}
