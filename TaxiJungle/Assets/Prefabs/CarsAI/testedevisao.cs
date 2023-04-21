using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class testedevisao : MonoBehaviour
{
    [SerializeField] Transform _checkFront;
    [SerializeField] private float _size;
    [SerializeField] LayerMask _aiCarLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Collider> hitColliders = new List<Collider>();
        hitColliders = Physics.OverlapSphere(_checkFront.position, 10, _aiCarLayer ).ToList();
        for (var i = 0; i < hitColliders.Count; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            Vector3 targetPos = tempTarget.position - transform.position; // find target direction
            Vector3 targetDir = tempTarget.forward - transform.forward; // find target direction
            Vector3 myDir = _checkFront.forward;
            float visionRadious = Vector3.SignedAngle(myDir, targetPos, Vector3.up);
            float targetAngle = Vector3.SignedAngle(myDir, targetDir, Vector3.up);
            Debug.Log(tempTarget.name + " " + targetAngle);
            if (visionRadious > -55 && visionRadious < 20)
            {
                Debug.Log(targetAngle);
                if (targetAngle <= 100 && targetAngle >= 90 || targetAngle <= -90 && targetAngle >= -100)
                {

                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.black);
                }
                else
                {
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
                }
            }
        }




        //List<Collider> hitColliders = new List<Collider>();
        //hitColliders = Physics.OverlapSphere(_checkFront.position, _size, _aiCarLayer).ToList();
        //for (var i = 0; i < hitColliders.Count; i++)
        //{
        //    Transform tempTarget = hitColliders[i].transform;
        //    Vector3 targetPos = tempTarget.position - _checkFront.position; // find target direction
        //    Vector3 targetDir = tempTarget.forward - _checkFront.forward; // find target direction
        //    Vector3 myDir = _checkFront.forward;
        //    float visionRadious = Vector3.SignedAngle(myDir, targetPos, Vector3.up);
        //    float targetAngle = Vector3.SignedAngle(myDir, targetDir, Vector3.up);
        //    Debug.Log(tempTarget.name + " " + targetAngle);

        //    if (visionRadious >= -105 && visionRadious <= -55)
        //    {
        //        if (targetAngle <= 155 && targetAngle >= 105)
        //        {
        //            Debug.Log("esquerda");
                    
        //            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.magenta);
        //        }
        //        else
        //        {
        //            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
        //        }
        //    }
        //    else if (visionRadious > -55 && visionRadious < 20)
        //    {
        //        if (targetAngle <= -110 && targetAngle >= -180 || targetAngle <= 180 && targetAngle >= 105)
        //        {
                    
        //            Debug.Log("centro");
        //            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.green);
        //        }
        //        else
        //        {
        //            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
        //        }
        //    }
        //    else if (visionRadious >= 20 && visionRadious <= 90)
        //    {
        //        if (targetAngle <= -112 && targetAngle >= -155)
        //        {
        //            Debug.Log(Vector3.Distance(targetPos, transform.position));
        //            if (Vector3.Distance(targetPos, transform.position) > 104)
        //            {
                       
        //                Debug.Log("Direita");
        //                Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.yellow);
        //            }

        //        }
        //        else
        //        {
        //            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
        //        }
        //    }
        //    else
        //    {
        //        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
        //    }
        //}
    }
}
