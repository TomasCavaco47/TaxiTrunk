using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAICarPathEditor : EditorWindow
{
    Transform _lastCheckPoint;
    private int _possibleWays;
    Transform _newChekPoint;

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
                             _newChekPoint = Instantiate(_lastCheckPoint);
                             _newChekPoint.SetParent(_lastCheckPoint.parent);
                            _newChekPoint.GetComponent<WayPoints>().SlowDown = false;
                            _newChekPoint.GetComponent<WayPoints>().HasATurn = false;
                            _newChekPoint.GetComponent<WayPoints>().Stop = false;
                             _newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint =new Transform[1] ;
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[0] = _newChekPoint;
                             _lastCheckPoint = _newChekPoint;

                         }
                         if(_newChekPoint != null)
                        {
                            Selection.objects = new Object[]
                            {
                                 _newChekPoint.gameObject
                            };

                        }
                         
                     }
                     else
                     {
                         if (GUILayout.Button("Create A new Waypoint"))
                         {
                             _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_possibleWays];
                                                        List<GameObject> _templist2 = new List<GameObject>();

                             for (int i = 0; i < _possibleWays; i++)
                             {
                                  _newChekPoint = Instantiate(_lastCheckPoint);
                                  _newChekPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[0];
                                  _newChekPoint.SetParent(_lastCheckPoint.parent);
                                _newChekPoint.GetComponent<WayPoints>().SlowDown = false;
                                _newChekPoint.GetComponent<WayPoints>().HasATurn = false;
                                _newChekPoint.GetComponent<WayPoints>().Stop = false;
                                _templist2.Add(_newChekPoint.gameObject);

                                _newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;                      
                                 _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i] = _newChekPoint;
        
                             }
                             _lastCheckPoint = _newChekPoint;

                            if (_newChekPoint != null)
                            {
                                GameObject[] arrayOfGameObjects = _templist2.ToArray();
                                Selection.objects = arrayOfGameObjects;


                            }

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
                            _newChekPoint = Instantiate(_lastCheckPoint);
                            _newChekPoint.SetParent(_lastCheckPoint.parent);
                            _newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;
                            _newChekPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[0];
                            List<Transform> _templist = new List<Transform>();

                            int routes = _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length;                              ;
                            for (int i = 0; i < routes; i++)
                            {
                                _templist.Add( _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i]);
                            }
                            _templist.Add(_newChekPoint);
                            
                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_templist.Count];

                            for (int i = 0; i < routes+_possibleWays; i++)
                            {
                                _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i] = _templist[i];

                            }
                            
                            _lastCheckPoint = _newChekPoint;
                            if (_newChekPoint != null)
                            {
                                Selection.objects = new Object[]
                                {
                                 _newChekPoint.gameObject
                                };

                            }
                        }
                  


                    }
                    else
                    {
                        if (GUILayout.Button("Create A new Waypoint"))
                        {
                            int routes = _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length;
                            List<Transform> _templist = new List<Transform>();
                            List<GameObject> _templist2 = new List<GameObject>();
                            for (int i = 0; i < routes; i++)
                            {
                                _templist.Add( _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i]);
                                
                            }
                                for (int i = 0; i < _possibleWays; i++)
                                {
                                    _newChekPoint = Instantiate(_lastCheckPoint);
                                    _templist.Add(_newChekPoint);
                                    _templist2.Add(_newChekPoint.gameObject);
                                    _newChekPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_lastCheckPoint.GetComponent<WayPoints>().NextWaypoint.Length + _possibleWays];
                                    _newChekPoint.SetParent(_lastCheckPoint.parent);
                                _newChekPoint.GetComponent<WayPoints>().SlowDown = false;
                                _newChekPoint.GetComponent<WayPoints>().HasATurn = false;
                                    _newChekPoint.GetComponent<WayPoints>().Stop = false;
                                    _newChekPoint.name = "WayPoint " + _lastCheckPoint.parent.childCount;

                                }
                         


                            _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint = new Transform[_templist.Count];

                            for (int i = 0; i < routes+_possibleWays; i++)
                            {
                                _lastCheckPoint.GetComponent<WayPoints>().NextWaypoint[i] = _templist[i];

                            }
                                //if (newChekPoint != null)
                                //{
                                //    GameObject[] arrayOfGameObjects = _templist2.ToArray();
                                //    Selection.objects = arrayOfGameObjects;
                               



                                //}
                          
                            _lastCheckPoint = _newChekPoint;
                         
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
