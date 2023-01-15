using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ScrolSysteam : MonoBehaviour
{
    [SerializeField] List<GameObject> _buttonsList = new List<GameObject>();

    [Header("Scroll")]
    [SerializeField] int _indexButton;
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] bool _keyDown;
    [SerializeField] float _valueAlterater;
    [SerializeField] Transform _container;

    


    public int IndexButton { get => _indexButton; set => _indexButton = value; }
    public List<GameObject> ButtonsList { get => _buttonsList; set => _buttonsList = value; }
    public float ValueAlterater { get => _valueAlterater; set => _valueAlterater = value; }

    private void Start()
    {
        ValueAlterate();
    }
  
    void Update()
    {
       if(_buttonsList.Count > 0)
        {
        EventSystem.current.SetSelectedGameObject(_buttonsList[_indexButton]);
        }
        
        Scroll();
        ScrollRotateFix();
      
    }
    public void ValueAlterate()
    {
        _valueAlterater = 1f / (_buttonsList.Count - 3);
    }


    


    //void OnValidate()
    //{
    //    _buttonsList = new List<GameObject>();

    //    for (int i = 0; i < _container.childCount; i++)
    //    {
    //        if (_container.GetChild(i).GetComponent<button>() != null)
    //        {
    //            _buttonsList.Add(_container.GetChild(i).gameObject);
    //        }
    //    }


    //}

    public void Scroll()
    {
            if (Input.GetAxis("ArrowsVertical") != 0)
        {
            if (!_keyDown)
            {
                if (Input.GetAxis("ArrowsVertical") < 0)
                {
                    if (_eventSystem.firstSelectedGameObject != _eventSystem.currentSelectedGameObject)
                    {
                        _indexButton++;
                        if (_indexButton > 1)
                        {
                            _scrollbar.value -= _valueAlterater;
                        
                        }
                            

                    }
                }
                else if (Input.GetAxis("ArrowsVertical") > 0)
                {
                    if (_eventSystem.firstSelectedGameObject != _eventSystem.currentSelectedGameObject)
                    {
                        _indexButton--;
                        if (_indexButton < _buttonsList.Count-2)
                        {
                            _scrollbar.value += _valueAlterater;
                        }
                    }
                }
                _keyDown = true;
            }
        }
        else
        {
            _keyDown = false;
        }
    }
    public void ScrollRotateFix()
    {
        if(_indexButton < 0)
        {
            _indexButton = _buttonsList.Count - 1;
            _scrollbar.value = 0;
        }
        if(_indexButton > _buttonsList.Count - 1)
        {
            _indexButton = 0;
            _scrollbar.value = 1;
        }
        if (_indexButton == _buttonsList.Count - 1)
        {
            _scrollbar.value = 0;
        }
        if (_indexButton == 0)
        {
            _scrollbar.value = 1;
        }
    }


  #region Tests
    //public void ExpandContainer()
    //{
    //    GameObject buttonPrefab = Instantiate(_buttonPrefab);
    //    buttonPrefab.transform.SetParent(_Parrent.transform);
    //    buttonPrefab.transform.SetAsFirstSibling();
    //   _buttonsList.Add(buttonPrefab);
    //   _container.offsetMin = new Vector2(_container.offsetMin.x, _container.offsetMin.y-27.8866f);
        
    //}

    //public void Inputs()
    //{
    //    if(Input.GetKey(KeyCode.UpArrow))
    //    {
    //        //yield return new WaitForSeconds(0.5f);

    //        _scrollbar.value = Mathf.Lerp(_scrollbar.value, 1f, _scrollVelocity*Time.deltaTime);
    //        if(_scrollbar.value > 0.8)
    //        {
    //            _scrollbar.value = 1;
    //        }



    //    }
    //    else if(Input.GetKey(KeyCode.DownArrow))
    //    {
    //        //yield return new WaitForSeconds(0.5f);
    //        _scrollbar.value = Mathf.Lerp(_scrollbar.value, 0f, _scrollVelocity * Time.deltaTime);
    //        if (_scrollbar.value < 0.1)
    //        {
    //            _scrollbar.value = 0;
    //        }




    //    }
    //}
    //IEnumerator InputActionsReapeat()
    //{
    //        yield return new WaitForSeconds(0.5f);
    //     if(Input.GetKey(KeyCode.UpArrow))
    //    {

    //        _scrollbar.value = Mathf.Lerp(_scrollbar.value, 1f, _scrollVelocity*Time.deltaTime);

    //        if(_scrollbar.value > 0.8)
    //        {
    //            _scrollbar.value = 1;
    //        }



    //    }
    //    else if(Input.GetKey(KeyCode.DownArrow))
    //    {

    //        _scrollbar.value = Mathf.Lerp(_scrollbar.value, 0f, _scrollVelocity * Time.deltaTime);

    //        if (_scrollbar.value < 0.1)
    //        {
    //            _scrollbar.value = 0;
    //        }




    //    }
    //}
    #endregion


}

  