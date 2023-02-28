using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LineControler : MonoBehaviour
{
    private LineRenderer _lr;
    [SerializeField] LineAI _lineAI;


    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lr.positionCount = _lineAI.Agent.path.corners.Length;
        _lr.SetPositions(_lineAI.Agent.path.corners);
    }

}
