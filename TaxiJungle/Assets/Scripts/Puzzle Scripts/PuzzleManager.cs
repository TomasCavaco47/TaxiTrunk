using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] List<ObjectScript> _objectsPrefab;
    [SerializeField] GridBuilder _grid;
    [SerializeField] Canvas _canvas;
    [SerializeField] private int _ocupiedSlots=0;
    [SerializeField] List<ObjectScript> _spawnedObjects;
    [SerializeField] int _numberOfCurrentOcupied;
    MissionManager _missionManager;

    private void Awake()
    {
        _missionManager = MissionManager.instance;
        _canvas = this.transform.parent.parent.GetComponent<Canvas>() ;
    }
    private void Start()
    {
       
        spawnObjects();

     
    }
    private void Update()
    {
        CheckIfClear();
    }
    void spawnObjects()
    {
        ObjectScript abc = null;

        //do
        //{
        //    int i = Random.Range(0, _objectsPrefab.Count);
        //    abc = Instantiate(_objectsPrefab[i], _canvas.transform);
        //    _spawnedObjects.Add(abc);
        //    _ocupiedSlots += abc.Size;
        //} while (_ocupiedSlots < 18); 


        do
        {
            int i = Random.Range(0, _objectsPrefab.Count);
            if (_objectsPrefab[i].Size + _ocupiedSlots > 18)
            {
               
            }
            else
            {
                abc = Instantiate(_objectsPrefab[i], _canvas.transform);

                _spawnedObjects.Add(abc);
                _ocupiedSlots += abc.Size;
            }
            Debug.Log(abc.Size);

            

        } while (_ocupiedSlots < 18);
        //int i = Random.Range(0, _objectsPrefab.Count);
        //abc = Instantiate(_objectsPrefab[i], _canvas.transform);

        //switch (abc.Size+ _ocupiedSlots)
        //{
        //    case < 18:
        //        _spawnedObjects.Add(abc);
        //        _ocupiedSlots += abc.Size;
        //        spawnObjects();

        //        break;
        //    case > 18:
        //        Destroy(abc);
        //        spawnObjects();
        //        break;
        //    case  18:
        //        break;

        //}






        Debug.Log(_ocupiedSlots);

        //if (_ocupiedSlots > 18)
        //{
        //    _ocupiedSlots = 0;
        //    for (int i = 0; i < _spawnedObjects.Count; i++)
        //    {
        //        Destroy(_spawnedObjects[i]);
        //    }
        //    _spawnedObjects.Clear();
        //    spawnObjects();
        //}
        //if(_ocupiedSlots >18)
        //{
        //    for (int a = 0; a < _spawnedObjects.Count; a++)
        //    {
        //        Destroy(_spawnedObjects[a]);
        //    }
        //    _spawnedObjects.Clear();
        //    spawnObjects();
        //}
    }
    void CheckIfClear()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                
                if(_grid.SlotGrid[x,y].GetComponent<Slots>().Ocupied)
                {
                    _numberOfCurrentOcupied++;
                }


            }
        }
        if(_numberOfCurrentOcupied == _ocupiedSlots)
        {
            _missionManager.StartTimer();
           SceneManager.UnloadSceneAsync("Puzzle");

        }
        else
        {
            _numberOfCurrentOcupied = 0;
        }
      

    }
}
