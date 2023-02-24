using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad()]
public class NPC_WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(NPC_Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow*0.5f;

        }

        Gizmos.DrawSphere(waypoint.transform.position, .1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.Width / 2f), waypoint.transform.position - (waypoint.transform.right * waypoint.Width / 2f));
        if(waypoint.PreviousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.Width / 2f;
            Vector3 offsetTo = waypoint.PreviousWaypoint.transform.right * waypoint.PreviousWaypoint.Width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.PreviousWaypoint.transform.position + offsetTo);
        }

        if(waypoint.NetxWaypoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right *- waypoint.Width / 2f;
            Vector3 offsetTo = waypoint.NetxWaypoint.transform.right * -waypoint.NetxWaypoint.Width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.NetxWaypoint.transform.position + offsetTo);
        }

        if(waypoint.Branches!=null)
        {
            foreach (NPC_Waypoint branch in waypoint.Branches)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }
    }
}
