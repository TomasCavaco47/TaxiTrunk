using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Waypoint : MonoBehaviour
{
    [SerializeField] NPC_Waypoint _previousWaypoint;
    [SerializeField] NPC_Waypoint _netxWaypoint;
    [SerializeField] TraficLight _traficLight;

    [Range(0f,5f)]
    [SerializeField] float _width = 1f;

    [SerializeField] List<NPC_Waypoint> _branches;
    [SerializeField] bool _isAExitBranch=false;
    [SerializeField] bool _beginingCrosswalk=false;
    [SerializeField] bool _hasAnEntryBranch=false;

    [Range(0f, 1f)]
    [SerializeField] float _branchRatio = 0.5f;
    public NPC_Waypoint PreviousWaypoint { get => _previousWaypoint; set => _previousWaypoint = value; }
    public NPC_Waypoint NetxWaypoint { get => _netxWaypoint; set => _netxWaypoint = value; }
    public float Width { get => _width; set => _width = value; }
    public List<NPC_Waypoint> Branches { get => _branches; set => _branches = value; }
    public float BranchRatio { get => _branchRatio; set => _branchRatio = value; }
    public bool IsAExitBranch { get => _isAExitBranch; set => _isAExitBranch = value; }
    public bool BeginingCrosswalk { get => _beginingCrosswalk; set => _beginingCrosswalk = value; }
    public bool HasAnEntryBranch { get => _hasAnEntryBranch; set => _hasAnEntryBranch = value; }
    public TraficLight TraficLight { get => _traficLight; set => _traficLight = value; }

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * Width / 2f;
        Vector3 maxBound = transform.position - transform.right * Width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
