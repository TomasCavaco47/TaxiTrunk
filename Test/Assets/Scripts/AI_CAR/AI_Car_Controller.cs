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
    [SerializeField] private Transform _currentWaypoint;
    private Rigidbody _rb;
    [SerializeField]private bool _carInFront;
    [SerializeField]private bool _canTurn;
    private float _angleToTurn;
   [SerializeField] private Transform _checkPointToCheckCar;

    Vector3 fwd;
    Vector3 left;
    Vector3 right;

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
        left = _pointToCastLeft.transform.TransformDirection(Vector3.left);
        fwd = _pointToCastForward.transform.TransformDirection(Vector3.forward);



    }

    private void Update()
    {
        Debug.Log(_checkPointToCheckCar);
        _angleToTurn = Vector3.SignedAngle(_currentWaypoint.position - transform.position, transform.forward, Vector3.up);
       // _angleToTurn =Vector3.Angle(transform.forward,  _currentWaypoint.position -  transform.position);
        //Debug.Log(_angleToTurn);

        CheckCarInFront();
        CheckCanTurn();
       
        
        speed =Mathf.RoundToInt( _rb.velocity.magnitude * 3.6f);
     
        //Debug.Log(speed);
    }
    private void CheckCarInFront()
    {
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
    }
    private void CheckCanTurn()
    {
        if (_checkPointToCheckCar != null)
        {
            Debug.DrawRay(new Vector3(_checkPointToCheckCar.position.x, _checkPointToCheckCar.position.y, _checkPointToCheckCar.position.z), -_checkPointToCheckCar.right * 50, Color.green);
            if (Physics.Raycast(new Vector3(_checkPointToCheckCar.position.x, _checkPointToCheckCar.position.y, _checkPointToCheckCar.position.z), left, out objectHit, 50))
            {
                //do something if hit object ie
                if (objectHit.collider.tag == "AICar")
                {
                    _canTurn = false;

                }

            }
            else
            {
                _checkPointToCheckCar = null; ;

                _canTurn = true;

            }
        }

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
            MoveCar();
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

    private void MoveCar()
    {
        foreach (var item in wheels)
        {
            if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront == false && _canTurn == true)
            {
                item.motorTorque = totalPower;
                item.brakeTorque = 0;
                Debug.Log("1");

            }
            else if (_carInFront)
            {
                item.brakeTorque = 700;
                item.motorTorque = 0;
                Debug.Log("2");

            }
            else if (_currentWaypoint.GetComponent<WayPoints>().SlowDown && _currentWaypoint.GetComponent<WayPoints>().ItsATurn ==false)
            {
                if (speed < 20 && _carInFront == false)
                {
                    item.brakeTorque = 0;
                    item.motorTorque = 0;

                }
                else
                {
                   
                    item.brakeTorque = 900;
                    item.motorTorque = 0;
                }           
            }
            else if (_currentWaypoint.GetComponent<WayPoints>().SlowDown && _currentWaypoint.GetComponent<WayPoints>().ItsATurn)
            {

              
                    if (_checkPointToCheckCar == null)
                    {
                        _checkPointToCheckCar = _currentWaypoint;
                        int routeToTake = Random.Range(0, _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint.Length);
                        _currentWaypoint = _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint[routeToTake];
                    }
                    else
                    {

                    }
                        if (_canTurn == false)
                        {
                            item.brakeTorque = 700;
                            item.motorTorque = 0;
                            Debug.Log("1");
                        }
                        else
                        {
                            item.brakeTorque = 0;
                            item.motorTorque = totalPower;
                            Debug.Log("2");
                        }
                
            }
        }

        }
        private void steerVehicle()
    {

        Vector3 relativeVector = transform.InverseTransformPoint(_currentWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 2;
        horizontal = newSteer;
        foreach (var item in wheels)
        {
            //if(_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront==false )
            //{
            //    item.motorTorque = totalPower;
            //    item.brakeTorque = 0;

            //}
            //else
            //{
                //if(_currentWaypoint.GetComponent<WayPoints>().ItsATurn)
                //{
                //    if (_checkPointToCheckCar == null)
                //    {
                //        _checkPointToCheckCar = _currentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                //        int routeToTake = Random.Range(0, _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint.Length);
                //        _currentWaypoint = _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint[routeToTake];


                        
                //    }

                //if(speed < 20 && _carInFront==false )
                //{
                //    if (_angleToTurn >10 ||_angleToTurn< -10 )
                //    {

                //        if(_checkPointToCheckCar ==null)
                //        {
                //            if (Physics.Raycast(new Vector3(_pointToCastForward.transform.position.x, _pointToCastForward.transform.position.y, _pointToCastForward.transform.position.z + 8), fwd, out objectHit, 15))
                //            {
                //                Debug.DrawRay(new Vector3(_pointToCastForward.transform.position.x, _pointToCastForward.transform.position.y, _pointToCastForward.transform.position.z + 8), fwd * 15, Color.cyan);

                //                //do something if hit object ie
                //                if (objectHit.collider.tag == "WayPoints")
                //                {
                //                    _checkPointToCheckCar = objectHit.transform;

                //                }

                //            }

                //            //_checkPointToCheckCar = _currentWaypoint;
                //        }
                //        if (_canTurn ==false)
                //        {
                //            item.brakeTorque = 700;
                //            item.motorTorque = 0;
                //            Debug.Log("1");

                //        }
                //        else
                //        {

                //            item.brakeTorque = 0;
                //            item.motorTorque = totalPower;
                //            Debug.Log("2");
                //        }

                //    }



                //}
                //else
                //{
                //    Debug.Log("3");
                //    _checkPointToCheckCar = null;
                //    item.brakeTorque = 500;
                //    item.motorTorque = 0;
              //  }
            //}
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
        if(speed > 40)
        {
            foreach (var item in wheels)
            {
                if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront == false)
                {
                    item.motorTorque = 0;

                    item.brakeTorque = 400;

                }
                else
                {
                    item.brakeTorque = 0;

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
              

            }
           
        }
       
    }
   


}
