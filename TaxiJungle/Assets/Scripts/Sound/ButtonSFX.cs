using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _buttonSFXSource;
    [SerializeField] private AudioClip _selected;
    [SerializeField] private AudioClip _click;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected()
    {
        _buttonSFXSource.PlayOneShot(_selected);
    }

    public void Click()
    {
        _buttonSFXSource.PlayOneShot(_click);
    }
}
