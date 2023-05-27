using UnityEngine.Audio;
using UnityEngine;
using Mono.Cecil;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using static Unity.VisualScripting.Member;
using static UnityEditor.Rendering.FilterWindow;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] _soundsOfTheCity;
    [SerializeField] GameObject[] _objectsWithSoundToPause;
    [SerializeField] AudioSource _audioSourceToAdd;
    [SerializeField] GameObject _objectToAdd;
    public static AudioManager _instance { get; private set; }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseSounds()
    {
        GameObject.FindGameObjectsWithTag("AICar");
        GameObject.FindGameObjectsWithTag("SoundsToPause");
        var _objectsWithSoundToPauseAICar = GameObject.FindGameObjectsWithTag("AICar").ToList();
        var _objectsWithSoundToPauseSounds = GameObject.FindGameObjectsWithTag("SoundsToPause").ToList();
        _objectsWithSoundToPauseSounds.AddRange(_objectsWithSoundToPauseAICar);
        _objectsWithSoundToPause = _objectsWithSoundToPauseSounds.ToArray();
        
        //_soundsOfTheCity = new AudioSource[100];
        foreach (GameObject obj in _objectsWithSoundToPause)
        {
            _audioSourceToAdd = obj.GetComponent<AudioSource>();
            _soundsOfTheCity = _soundsOfTheCity.Append(_audioSourceToAdd).ToArray();
        }
        foreach (AudioSource _audioSource in _soundsOfTheCity)
        {
            _audioSource.Pause();
        }
        
    }

    public void ResumeSounds()
    {
        foreach (AudioSource _audioSource in _soundsOfTheCity)
        {
            _audioSource.UnPause();
        }
        _objectsWithSoundToPause = new GameObject[0];
        _soundsOfTheCity = new AudioSource[0];
    }

    
    
}
