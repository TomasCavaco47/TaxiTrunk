using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    #region Variaveís
    [SerializeField] private AudioSource _radio1AudioSource;
    [SerializeField] private AudioSource _radio2AudioSource;
    [SerializeField] private AudioSource _radio3AudioSource;
    [SerializeField] private AudioClip[] _songsRadio1;
    [SerializeField] private AudioClip[] _songsRadio2;
    [SerializeField] private AudioClip[] _songsRadio3;
    private float volume;
    [SerializeField] private float _volumeSetByUser;
    private float _songsPlayedRadio1;
    private float _songsPlayedRadio2;
    private float _songsPlayedRadio3;
    private bool[] _beenPlayedRadio1;
    private bool[] _beenPlayedRadio2;
    private bool[] _beenPlayedRadio3;
    [SerializeField] private int _radioValue;
    #endregion
    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {
        _radioValue = 0;
        volume = _volumeSetByUser;
       _radio1AudioSource.volume = volume;
        _radio2AudioSource.volume = 0;
        _radio3AudioSource.volume = 0;


        _beenPlayedRadio1 = new bool[_songsRadio1.Length];
        _beenPlayedRadio2 = new bool[_songsRadio2.Length];
        _beenPlayedRadio3 = new bool[_songsRadio3.Length];

        ChangeSong(Random.Range(0, _songsRadio1.Length), _beenPlayedRadio1);
        ChangeSong(Random.Range(0, _songsRadio2.Length), _beenPlayedRadio2);
        ChangeSong(Random.Range(0, _songsRadio3.Length), _beenPlayedRadio3);


        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyUp(KeyCode.Tab)) 
       {
            ChangeStation();
       }

        if (_radio1AudioSource.isPlaying == false)
        {
            ResetShuffle();
            ChangeSong(Random.Range(0, _songsRadio1.Length), _beenPlayedRadio1);
         
        }

        if (_radio2AudioSource.isPlaying == false)
        {
            ResetShuffle();
            ChangeSong(Random.Range(0, _songsRadio2.Length), _beenPlayedRadio2);
        }

        if (_radio3AudioSource.isPlaying == false)
        {
            ResetShuffle();
            ChangeSong(Random.Range(0, _songsRadio3.Length), _beenPlayedRadio3);
        }

        if (MissionManager.instance.IsInDialogue == true)
        {
            _radio1AudioSource.volume = 0;
            _radio2AudioSource.volume = 0;
            _radio3AudioSource.volume = 0;
        }
        else
        {
            switch (_radioValue)
            {
                case 0:
                    _radio1AudioSource.volume = volume;
                    _radio2AudioSource.volume = 0;
                    _radio3AudioSource.volume = 0;
                    break;
                case 1:
                    _radio1AudioSource.volume = 0;
                    _radio2AudioSource.volume = volume;
                    _radio3AudioSource.volume = 0;
                    break;
                case 2:
                    _radio1AudioSource.volume = 0;
                    _radio2AudioSource.volume = 0;
                    _radio3AudioSource.volume = volume;
                    break;
                case 3:
                    _radio1AudioSource.volume = 0;
                    _radio2AudioSource.volume = 0;
                    _radio3AudioSource.volume = 0;
                    break;
            }
           
            volume = _volumeSetByUser;
        }
    }
    #endregion
    #region ChangeSong / ChangeStation / ResetShuffle
    public void ChangeSong(int songPicked, bool[] bools)
    {
        if (bools == _beenPlayedRadio1 && !_beenPlayedRadio1[songPicked])
        {
            _songsPlayedRadio1++;
            _beenPlayedRadio1[songPicked] = true;
            _radio1AudioSource.clip = _songsRadio1[songPicked];
            _radio1AudioSource.Play();
            
        }
     
       

        if (bools == _beenPlayedRadio2 && !_beenPlayedRadio2[songPicked])
        {
            _songsPlayedRadio2++;
            _beenPlayedRadio2[songPicked] = true;
            _radio2AudioSource.clip = _songsRadio2[songPicked];
            _radio2AudioSource.Play();
        }
        

        if (bools == _beenPlayedRadio3 && !_beenPlayedRadio3[songPicked])
        {
            _songsPlayedRadio3++;
            _beenPlayedRadio3[songPicked] = true;
            _radio3AudioSource.clip = _songsRadio3[songPicked];
            _radio3AudioSource.Play();
        }
        

    }

    private void ResetShuffle()
    {
        if (_songsPlayedRadio1 == _songsRadio1.Length)
        {
            _songsPlayedRadio1 = 0;
            for (int i = 0; i < _songsRadio1.Length; i++)
            {
                if (i == _songsRadio1.Length)
                {
                    break;
                }
                else
                {
                    _beenPlayedRadio1[i] = false;
                }
            }
        }

        if (_songsPlayedRadio2 == _songsRadio2.Length)
        {
            _songsPlayedRadio2 = 0;
            for (int i = 0; i < _songsRadio2.Length; i++)
            {
                if (i == _songsRadio2.Length)
                {
                    break;
                }
                else
                {
                    _beenPlayedRadio2[i] = false;
                }
            }
        }

        if (_songsPlayedRadio3 == _songsRadio3.Length)
        {
            _songsPlayedRadio3 = 0;
            for (int i = 0; i < _songsRadio3.Length; i++)
            {
                if (i == _songsRadio3.Length)
                {
                    break;
                }
                else
                {
                    _beenPlayedRadio3[i] = false;
                }
            }
        }
    }

    public void ChangeStation()
    {
        _radioValue++;
        if(_radioValue >= 4 ) 
        {
            _radioValue = 0;
        }
        switch (_radioValue)
        {
            case 0:
                _radio1AudioSource.volume = volume; 
                _radio2AudioSource.volume = 0;
                _radio3AudioSource.volume = 0;
                break;
            case 1:
                _radio1AudioSource.volume = 0;
                _radio2AudioSource.volume = volume;
                _radio3AudioSource.volume = 0;
                break;
            case 2:
                _radio1AudioSource.volume = 0;
                _radio2AudioSource.volume = 0;
                _radio3AudioSource.volume = volume;
                break;
            case 3:
                _radio1AudioSource.volume = 0;
                _radio2AudioSource.volume = 0;
                _radio3AudioSource.volume = 0;
                break;
        }
    }
    #endregion
}
