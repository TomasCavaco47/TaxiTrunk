using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Waypoint : MonoBehaviour
{
    [SerializeField] NPC_Waypoint _previousWaypoint;
    [SerializeField] NPC_Waypoint _netxWaypoint;

    [Range(0f,5f)]
    [SerializeField] float _width = 1f;

    public NPC_Waypoint PreviousWaypoint { get => _previousWaypoint; set => _previousWaypoint = value; }
    public NPC_Waypoint NetxWaypoint { get => _netxWaypoint; set => _netxWaypoint = value; }
    public float Width { get => _width; set => _width = value; }

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * Width / 2f;
        Vector3 maxBound = transform.position - transform.right * Width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
