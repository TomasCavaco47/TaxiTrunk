using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _carModels;
    [SerializeField] int _currentDisplaying;
    void Start()
    {
        _currentDisplaying = 0;
        _carModels[_currentDisplaying].SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
       _carModels[_currentDisplaying].transform.Rotate(0, 4 * 8 * Time.deltaTime, 0 );
        Inputs();
    }
    private void Inputs()
    {
        int nextcar = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentDisplaying + 1 == _carModels.Count)
            {
                ChangeCar(nextcar);
            }
            else
            {
                ChangeCar(_currentDisplaying + 1);
            }
        } 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentDisplaying == 0)
            {
                ChangeCar(_carModels.Count-1);
            }
            else
            {
                ChangeCar(_currentDisplaying - 1);
            }
        }
    }

   private void ChangeCar(int carToShow)
    {
       
        _carModels[_currentDisplaying].SetActive(false);
        _currentDisplaying = carToShow;

        _carModels[_currentDisplaying].transform.eulerAngles = new Vector3(0, 130, 0);

        _carModels[_currentDisplaying].SetActive(true);



    }
}
