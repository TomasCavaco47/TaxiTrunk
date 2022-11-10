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
    [SerializeField] Text _currentTimeText;

    [SerializeField] GameObject _dialogueObject;
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

    public void ShowTimer(bool active, float timer)
    {
        _timerObject.SetActive(active);
        if (active)
        {
            
            TimeSpan time = TimeSpan.FromSeconds(timer);
            _currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }
    }

    public void Dialogue( Sprite sprite, string text)
    {
        _dialogueObject.SetActive(true);
        _dialogueObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        _dialogueObject.transform.GetChild(1).GetComponent<Text>().text = text;
    }
    public void CloseDialogue()
    {
        _dialogueObject.SetActive(false);
       
    }
    
}
