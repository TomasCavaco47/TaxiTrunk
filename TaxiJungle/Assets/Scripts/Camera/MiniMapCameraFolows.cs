using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFolows : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public Transform Target { get => _target; set => _target = value; }

    private void Start()
    {
        _target = GameManager.instance.CurrentCarInUse.transform;
    }
    private void LateUpdate()
    {
      
        Vector3 temp = transform.position;
        
        transform.eulerAngles = new Vector3(90, Target.eulerAngles.y, 0);
        temp.x = Target.position.x;
        temp.z = Target.position.z;
        transform.position = temp;
        
    }

   
}
