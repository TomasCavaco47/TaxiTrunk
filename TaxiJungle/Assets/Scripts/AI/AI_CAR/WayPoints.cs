using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]


public class WayPoints : MonoBehaviour
{
   
    [SerializeField] private Transform[] _nextWaypoint;
    [SerializeField] private bool _hasATurn;
    [SerializeField] private bool _stop;   
    [SerializeField] private bool _hasATraficLight;   
    [SerializeField] private TraficLight _traficLight;   
    [SerializeField] private int _roadMaxSpeed;



    public Transform[] NextWaypoint { get => _nextWaypoint; set => _nextWaypoint = value; }
    public bool HasATurn { get => _hasATurn; set => _hasATurn = value; }
    public int RoadMaxSpeed { get => _roadMaxSpeed; set => _roadMaxSpeed = value; }
    public bool Stop { get => _stop; set => _stop = value; }
    public TraficLight TraficLight { get => _traficLight; set => _traficLight = value; }
    public bool HasATraficLight { get => _hasATraficLight; set => _hasATraficLight = value; }

    private void OnDrawGizmos()
    {
        if(HasATraficLight)
        {
            Gizmos.color = Color.yellow;

        }
        else if(_stop)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;

        }
        Gizmos.DrawWireSphere(transform.position, 1f);
        try
        {
            for (int i = 0; i < _nextWaypoint.Length; i++)
            {
                if(_nextWaypoint.Length ==1)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, _nextWaypoint[i].position);

                }
                else
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(transform.position, _nextWaypoint[i].position);

                }
            }

        }
        catch
        {
            List<Transform> gameObjectList = new List<Transform>(_nextWaypoint); 
            gameObjectList .RemoveAll(x => x == null);
            _nextWaypoint = gameObjectList .ToArray();

        }
    }

  
  
 

}
