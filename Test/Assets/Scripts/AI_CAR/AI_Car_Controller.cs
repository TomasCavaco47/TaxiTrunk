using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Car_Controller : MonoBehaviour
{
    //private carModifier modifier;
    [SerializeField] private WheelCollider[] wheels;

    [SerializeField] private float totalPower;
    [SerializeField] private float vertical, horizontal;
    private bool _tractionControl = true;

    private float _antiRoll = 1000f;
    private int speed;
    [SerializeField] private GameObject _pointToCastForward;
    [SerializeField] private GameObject _pointToCastLeft;


    private float radius = 8;
    [SerializeField] private WayPoints _wayPoints;
    private Transform _currentWaypoint;
    private Rigidbody _rb;
    [SerializeField]private bool _carInFront;

    Vector3 fwd;

    private RaycastHit objectHit;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }
    void Start()
    {
        //modifier = GetComponent<carModifier>();
        //wheels = modifier.colliders;
        _currentWaypoint = _wayPoints.transform;

    }
    private void Update()
    {


        fwd = _pointToCastForward.transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(_pointToCast.transform.position, fwd * 5, Color.green);
        Debug.DrawRay(_pointToCastForward.transform.position, fwd * 5, Color.green);
        if (Physics.Raycast(_pointToCastForward.transform.position, fwd, out objectHit, 5))
        {
            //do something if hit object ie
            if (objectHit.collider.tag == "AICar")
            {
                _carInFront = true;
            }
          
        }
        else
        {
            _carInFront = false;

        }

        speed =Mathf.RoundToInt( _rb.velocity.magnitude * 3.6f);
       
        Debug.Log(speed);
    }

    void FixedUpdate()
    {
        try
        {
            checkDistance();
            steerVehicle();
            AntiRoll();
            TractionControl();
            SpeedLimiter();
        }
        catch { }

    }


    void checkDistance()
    {         

        foreach (var item in wheels)
        {
           
            if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false)
            {
            item.motorTorque = totalPower / 2;
            item.brakeTorque = 1000;

            }
            else
            {
            item.motorTorque = totalPower;
            item.brakeTorque = 0;
            }
        }

    }

    //private void reachedDestination()
    //{

    //    int i =_wayPoints.NextWaypoint.Length;
    //    int newdetination = Random.Range(0, i);
    //    _currentWaypoint = _nextWaypoint;
    //}


    private void steerVehicle()
    {

        Vector3 relativeVector = transform.InverseTransformPoint(_currentWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 2;
        horizontal = newSteer;
        foreach (var item in wheels)
        {
            if(_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront==false)
            {
                item.motorTorque = totalPower;
                item.brakeTorque = 0;

            }
            else
            {
                item.brakeTorque = 500;
                item.motorTorque = 0 ;
                if(speed < 20)
                {
                    item.brakeTorque = 0;
                    item.motorTorque = totalPower;


                }
            }
        }

        if (horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        }
        else if (horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }

    }
    private void AntiRoll()
    {
        // Front axle
        ApplyAntiRoll(wheels[0], wheels[1]);
        // Back axle
        ApplyAntiRoll(wheels[2], wheels[3]);
    }

    private void ApplyAntiRoll(WheelCollider left, WheelCollider right)
    {
        WheelHit hit;
        float travelLeft = 1f;
        float travelRight = 1f;

        bool isGroundedLeft = left.GetGroundHit(out hit);
        if (isGroundedLeft)
        {
            travelLeft = (-left.transform.InverseTransformPoint(hit.point).y - left.radius) / left.suspensionDistance;
        }
        bool isGroundedRight = right.GetGroundHit(out hit);
        if (isGroundedRight)
        {
            travelRight = (-right.transform.InverseTransformPoint(hit.point).y - right.radius) / right.suspensionDistance;
        }

        float antirollForce = (travelLeft - travelRight) * _antiRoll;

        if (isGroundedLeft)
        {
            _rb.AddForceAtPosition(left.transform.up * -antirollForce, left.transform.position);
        }
        if (isGroundedRight)
        {
            _rb.AddForceAtPosition(right.transform.up * antirollForce, right.transform.position);
        }
    }
    private void TractionControl()
    {
        if (_tractionControl)
        {
            WheelHit hit;
            wheels[0].GetGroundHit(out hit);
            if (hit.forwardSlip >= 0.3f && wheels[0].motorTorque > 0)
            {
                wheels[0].motorTorque *= 0.9f;
            }
            wheels[1].GetGroundHit(out hit);
            if (hit.forwardSlip >= 0.3f && wheels[1].motorTorque > 0)
            {
                wheels[1].motorTorque *= 0.9f;
            }
        }
    }
    void SpeedLimiter()
    {
        if(speed >40)
        {
            foreach (var item in wheels)
            {
                if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront == false)
                {
                    item.motorTorque = 0;
                    

                }
                else
                {
                    item.motorTorque = totalPower;

                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoints")
        {
            if (other.GetComponent<WayPoints>().NextWaypoint.Length ==1)
            {
            _currentWaypoint = other.GetComponent<WayPoints>().NextWaypoint[0];

            }
            else
            {
                int routeToTake = Random.Range(0, other.GetComponent<WayPoints>().NextWaypoint.Length);
                _currentWaypoint = other.GetComponent<WayPoints>().NextWaypoint[routeToTake];

            }
           
        }
       
    }
   


}
