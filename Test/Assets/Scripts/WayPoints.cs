using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _nextWaypoint;


    public Transform[] NextWaypoint { get => _nextWaypoint; set => _nextWaypoint = value; }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
        try
        {
            for (int i = 0; i < _nextWaypoint.Length; i++)
            {

                Gizmos.DrawLine(transform.position, _nextWaypoint[i].position);
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
