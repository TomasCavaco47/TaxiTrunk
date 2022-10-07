using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoints : MonoBehaviour
{
    [SerializeField] private  Transform target;
    [SerializeField] private bool _multipleWays;
    

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.position);
        }
        
    }
    
}
