using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComedyShow : MonoBehaviour
{
    [SerializeField] List<string> _adjectives;
    [SerializeField] List<string> _playerChoices;
    [SerializeField] List<Button> _buttonChoices;
    [SerializeField] GameObject _pleaseChooseObject;
    [SerializeField] TMP_Text _answer;
    private void Awake()
    {
      
    }
    private void Start()
    {
        ChangeWords();
    }

    private void ChangeWords()
    {
        for (int i = 0; i < _buttonChoices.Count; i++)
        {
            int x = Random.Range(0, _adjectives.Count);
            _buttonChoices[i].GetComponentInChildren<TMP_Text>().text = _adjectives[x].ToString();
            Debug.Log("1");
        }
        if(_buttonChoices[0].GetComponentInChildren<TMP_Text>().text == _buttonChoices[1].GetComponentInChildren<TMP_Text>().text)
        {
            ChangeWords();
        }
    }
    public void StorePlayerChoice(Button Buttao)
    {
      
        _playerChoices.Add(Buttao.GetComponentInChildren<TMP_Text>().text);
        if(_playerChoices.Count<3)
        {
            ChangeWords();

        }
        else
        {
            for (int i = 0; i < _buttonChoices.Count; i++)
            {

                _buttonChoices[i].gameObject.SetActive(false);
              
            }
            _pleaseChooseObject.SetActive(false);
            _answer.gameObject.SetActive(true);
            _answer.text = "I am Vin a " + _playerChoices[0] + "  comediant, i am here to give you a show and make you guys laught, but you guys seem like " + _playerChoices[1] + " so im gonna get out, i dont want to be here with you " + _playerChoices[2] + " motherfuckers";
        }
    }
}
