using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAICarPathEditor : EditorWindow
{
    Transform _lastCheckPoint;
    Transform _checkPointToEdit;
    private int _possibleWays;
    Transform newChekPoint;
    public enum TESTE
    {
        Create,
        Edit
    }
    public  TESTE teste;
    [MenuItem("Tools/CarAiPath")]
 
      static void Init()
    {
        var window = GetWindow<CreateAICarPathEditor>();
        window.Show();
    }
    private void OnGUI()
    {
        teste = (TESTE)EditorGUILayout.EnumPopup("What to you want to do:", teste);
        if (teste == TESTE.Create)
        {
            _lastCheckPoint = (Transform)EditorGUILayout.ObjectField("Previous Waypoint", _lastCheckPoint, typeof(Transform), true);
            if(_lastCheckPoint != null)
            {
                if (_lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length==0)
                {

                     _possibleWays = EditorGUILayout.IntField("Possible ways",_possibleWays);
                     if (_possibleWays ==1)
                     {
                         if (GUILayout.Button("Create A new Waypoint"))
                         {
                             newChekPoint = Instantiate(_lastCheckPoint);
                             newChekPoint.SetParent(_lastCheckPoint.parent);
                             newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint =new Transform[1] ;
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[0] = newChekPoint;
                             _lastCheckPoint = newChekPoint;

                         }
                         
                     }
                     else
                     {
                         if (GUILayout.Button("Create A new Waypoint"))
                         {
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_possibleWays];
                             for (int i = 0; i < _possibleWays; i++)
                             {
                                  newChekPoint = Instantiate(_lastCheckPoint);
                                  newChekPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[0];
                                  newChekPoint.SetParent(_lastCheckPoint.parent);
                                  newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;                      
                                 _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i] = newChekPoint;
        
                             }
                             _lastCheckPoint = newChekPoint;

                         }
                     }
                }
                else
                {
                    _possibleWays = EditorGUILayout.IntField("Possible ways", _possibleWays);

                    if (_possibleWays == 1)
                    {
                        if (GUILayout.Button("Create A new Waypoint"))
                        {
                            newChekPoint = Instantiate(_lastCheckPoint);
                            newChekPoint.SetParent(_lastCheckPoint.parent);
                            newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
                            int routes = _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length;
                            Transform[] temptrans = new Transform[ _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length];
                           ;
                            for (int i = 0; i < _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length; i++)
                            {
                                temptrans[i] = _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i];
                            }
                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[routes + 1];
                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = temptrans;
                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[_lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length] = newChekPoint;
                            _lastCheckPoint = newChekPoint;

                        }

                    }
                    else
                    {
                        if (GUILayout.Button("Create A new Waypoint"))
                        {
                            int routes = _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length;
                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[routes + 1];
                            for (int i = 0; i < _possibleWays; i++)
                            {
                                newChekPoint = Instantiate(_lastCheckPoint);
                                newChekPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length + _possibleWays];
                                newChekPoint.SetParent(_lastCheckPoint.parent);
                                newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
                                _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i] = newChekPoint;

                            }
                            _lastCheckPoint = newChekPoint;

                        }
                    }
                }
               

            }

        }
     
       // else
       // {
       //     _checkPointToEdit = (Transform)EditorGUILayout.ObjectField("Edit Waypoint", _checkPointToEdit, typeof(Transform), true);
       //     if (_checkPointToEdit != null)
       //     { 


       //         if (GUILayout.Button("Create new Route"))
       //         {
       //             int routes = _checkPointToEdit.GetComponent<WayPoints>().NextWaypoint.Length;

       //             newChekPoint = Instantiate(_lastCheckPoint);
       //             newChekPoint.SetParent(_lastCheckPoint.parent);
       //             newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
       //             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[1];
       //             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[0] = newChekPoint;
       //             _lastCheckPoint = newChekPoint;

       //         }
       //         if (GUILayout.Button("Remove  Route"))
       //         {

       //         }
            
       //     }

            
       //}
       
    }


}
