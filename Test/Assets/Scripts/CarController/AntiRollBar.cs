using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [SerializeField] private WheelCollider _wheelL;
    [SerializeField] private WheelCollider _wheelR;
    [SerializeField] private float _antiRoll = 5000;

    private Rigidbody _car;
    private void Start()
    {
        _car = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1;
        float travelR = 1;

        bool groundedL = _wheelL.GetGroundHit(out hit);
        if(groundedL)
        {
            travelL = (-_wheelL.transform.InverseTransformPoint(hit.point).y - _wheelL.radius) / _wheelL.suspensionDistance;
        } 
        bool groundedR = _wheelL.GetGroundHit(out hit);
        if(groundedR)
        {
            travelL = (-_wheelR.transform.InverseTransformPoint(hit.point).y - _wheelR.radius) / _wheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * _antiRoll;

        if(groundedL)
        {
            _car.AddForceAtPosition(_wheelL.transform.up * -antiRollForce, _wheelL.transform.position);
        }if(groundedR)
        {
            _car.AddForceAtPosition(_wheelR.transform.up * -antiRollForce, _wheelR.transform.position);
        }

    }
}
