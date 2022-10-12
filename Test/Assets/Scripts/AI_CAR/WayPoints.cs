using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
   
    [SerializeField] private Transform[] _nextWaypoint;
    [SerializeField] private bool _slowDown;
    [SerializeField] private bool _itsATurn;


    public Transform[] NextWaypoint { get => _nextWaypoint; set => _nextWaypoint = value; }
    public bool SlowDown { get => _slowDown; set => _slowDown = value; }
    public bool ItsATurn { get => _itsATurn; set => _itsATurn = value; }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
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
