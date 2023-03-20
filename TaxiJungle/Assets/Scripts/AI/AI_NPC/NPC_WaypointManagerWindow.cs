using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPC_WaypointManagerWindow : EditorWindow
{
   [MenuItem("Tools/NPC Waypoint Editor")]
   public static void Open()
    {
        GetWindow<NPC_WaypointManagerWindow>();

    }
    public Transform npcWaypointRoot;
    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("npcWaypointRoot"));

        if (npcWaypointRoot == null )
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if(Selection.activeGameObject !=null && Selection.activeGameObject.GetComponent<NPC_Waypoint>())
        {
            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateNewBrach();
            }
            if(GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            } 
            if(GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if(GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }
    void CreateNewBrach()
    {
        GameObject waypointObject = new GameObject("Waypoint " + npcWaypointRoot.childCount, typeof(NPC_Waypoint));
        waypointObject.transform.SetParent(npcWaypointRoot, false);

        NPC_Waypoint waypoint = waypointObject.GetComponent<NPC_Waypoint>();

        NPC_Waypoint branchFrom = Selection.activeGameObject.GetComponent<NPC_Waypoint>();
        Debug.Log(waypoint);
        branchFrom.Branches.Add(waypoint);

        waypoint.transform.position = branchFrom.transform.position;
        waypoint.transform.forward = branchFrom.transform.forward;
        Selection.activeGameObject =waypoint.gameObject;

    }
    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("NPCWaypoint " + npcWaypointRoot.childCount, typeof(NPC_Waypoint));
        waypointObject.transform.SetParent(npcWaypointRoot, false);

        NPC_Waypoint waypoint = waypointObject.GetComponent<NPC_Waypoint>();
        if(npcWaypointRoot.childCount>1)
        {
            waypoint.PreviousWaypoint = npcWaypointRoot.GetChild(npcWaypointRoot.childCount - 2).GetComponent<NPC_Waypoint>();
            waypoint.PreviousWaypoint.NetxWaypoint = waypoint;
            // Place the waypoint at the last position
            waypoint.transform.position = waypoint.PreviousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.PreviousWaypoint.transform.forward;
            waypoint.Width = waypoint.PreviousWaypoint.Width;
        }

        Selection.activeObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("NPCWaypoint " + npcWaypointRoot.childCount, typeof(NPC_Waypoint));
        waypointObject.transform.SetParent(npcWaypointRoot, false);

        NPC_Waypoint newWaypoint = waypointObject.GetComponent<NPC_Waypoint>();

        NPC_Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<NPC_Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if(selectedWaypoint.PreviousWaypoint!=null)
        {
            newWaypoint.PreviousWaypoint = selectedWaypoint.PreviousWaypoint;
            selectedWaypoint.PreviousWaypoint.NetxWaypoint = newWaypoint;
            newWaypoint.Width = selectedWaypoint.PreviousWaypoint.Width;


        }

        newWaypoint.NetxWaypoint = selectedWaypoint;
        selectedWaypoint.PreviousWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("NPCWaypoint " + npcWaypointRoot.childCount, typeof(NPC_Waypoint));
        waypointObject.transform.SetParent(npcWaypointRoot, false);

        NPC_Waypoint newWaypoint = waypointObject.GetComponent<NPC_Waypoint>();

        NPC_Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<NPC_Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.PreviousWaypoint = selectedWaypoint;

        if(selectedWaypoint.NetxWaypoint != null)
        {
            selectedWaypoint.NetxWaypoint.PreviousWaypoint = newWaypoint;
            newWaypoint.NetxWaypoint = selectedWaypoint.NetxWaypoint;
            


        }
        newWaypoint.Width = newWaypoint.PreviousWaypoint.Width;
        selectedWaypoint.NetxWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void RemoveWaypoint()
    {
        NPC_Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<NPC_Waypoint>();

        if(selectedWaypoint.NetxWaypoint !=null)
        {
            selectedWaypoint.NetxWaypoint.PreviousWaypoint = selectedWaypoint.PreviousWaypoint;

        }
        if(selectedWaypoint.PreviousWaypoint !=null)
        {
            selectedWaypoint.PreviousWaypoint.NetxWaypoint = selectedWaypoint.NetxWaypoint;
            Selection.activeGameObject = selectedWaypoint.PreviousWaypoint.gameObject;
        }
        DestroyImmediate(selectedWaypoint.gameObject);
    }
}
