using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Car_Controller : MonoBehaviour
{
    public LayerMask rayMask;
    enum TurnDirection
    {
        Left,
        Center,
        Right
    }  
    enum TurnType
    {
        CrossRoad,
        NormalTurn
        
    }
    //private carModifier modifier;
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private TurnDirection _turnDirection;
    [SerializeField] private TurnType _turnType;
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
    [SerializeField] private Transform _turnWaypoint;
    private Rigidbody _rb;
    [SerializeField]private bool _carInFront;
    [SerializeField]private bool _canTurn;
    [SerializeField]private bool _hasCheckTurn = false;
    private float _angleToTurn;
   [SerializeField] private Transform _checkPointToCheckCar;
   [SerializeField] private Transform _checkPointToCheckCar2;

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
        _angleToTurn = Vector3.SignedAngle(_currentWaypoint.position - transform.position, transform.forward, Vector3.up);
       // _angleToTurn =Vector3.Angle(transform.forward,  _currentWaypoint.position -  transform.position);
        //Debug.Log(_angleToTurn);

        CheckCarInFront();
        CheckCanTurn();
       
        
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
            MoveCar();
        }
        catch { }

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
        if(_turnType == TurnType.CrossRoad)
            if (_checkPointToCheckCar != null)
            {
                if(_turnDirection == TurnDirection.Left)
                {               
                    Debug.DrawRay(new Vector3(_checkPointToCheckCar.position.x, _checkPointToCheckCar.position.y, _checkPointToCheckCar.position.z), -_checkPointToCheckCar.right * 50, Color.green);
                    Debug.DrawRay(new Vector3(_checkPointToCheckCar2.position.x, _checkPointToCheckCar2.position.y, _checkPointToCheckCar2.position.z), _checkPointToCheckCar2.right * 50, Color.magenta);
                    if (Physics.Raycast(new Vector3(_checkPointToCheckCar2.position.x, _checkPointToCheckCar2.position.y, _checkPointToCheckCar2.position.z), _checkPointToCheckCar2.right, out objectHit, 50, rayMask) || Physics.Raycast(new Vector3(_checkPointToCheckCar.position.x, _checkPointToCheckCar.position.y, _checkPointToCheckCar.position.z), -_checkPointToCheckCar.right, out objectHit, 50, rayMask))
                    {
                        Debug.Log(objectHit.collider.gameObject);
                        if (objectHit.collider.tag == "AICar")
                        {
                            Debug.Log("v");

                            _canTurn = false;
                        }
                    }
                    else
                    {
                        Debug.Log("c");

                        _canTurn = true;
                        _checkPointToCheckCar = null;
                        _checkPointToCheckCar2 = null;

                    }
                }
                if(_turnDirection == TurnDirection.Right)
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

                        _canTurn = true;
                        _checkPointToCheckCar = null;

                    }
                }
            }
        {

        }

    }

    


    void checkDistance()
    {         

        //foreach (var item in wheels)
        //{
           
        //    if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false)
        //    {
        //    item.motorTorque = totalPower / 2;
        //    item.brakeTorque = 1000;

        //    }
        //    else
        //    {
        //    item.motorTorque = totalPower;
        //    item.brakeTorque = 0;
        //    }
        //}

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
            if (_currentWaypoint.GetComponent<WayPoints>().SlowDown == false && _carInFront == false  && speed < _currentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
            {
                Debug.Log("two");

                item.motorTorque = totalPower;
                item.brakeTorque = 0;
                

            }
            else if (_carInFront)
            {
                item.brakeTorque = 700;
                item.motorTorque = 0;

            }
            else if (_currentWaypoint.GetComponent<WayPoints>().SlowDown && _currentWaypoint.GetComponent<WayPoints>().HasATurn ==false)
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
            else if (_currentWaypoint.GetComponent<WayPoints>().HasATurn)
            {


                if (_checkPointToCheckCar == null)
                {
                    Debug.Log("4");

                    _checkPointToCheckCar = _currentWaypoint;
                    if (Physics.Raycast(new Vector3(_checkPointToCheckCar.position.x, _checkPointToCheckCar.position.y, _checkPointToCheckCar.position.z), fwd, out objectHit, 15))
                    {
                        //do something if hit object ie
                        if (objectHit.collider.tag == "WayPoints")
                        {
                            _checkPointToCheckCar2 = objectHit.transform;
                        }
                    }
                    int routeToTake = Random.Range(0, _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint.Length);
                        _currentWaypoint = _checkPointToCheckCar.GetComponent<WayPoints>().NextWaypoint[routeToTake];
                    _turnWaypoint = _currentWaypoint;
                }


                        
            }
            if (_checkPointToCheckCar != null )
            {
                if(horizontal < -0.2 && _hasCheckTurn ==false)
                {
                    Debug.Log("1232321");
                    _turnDirection = TurnDirection.Left;
                    _currentWaypoint = _checkPointToCheckCar;
                    _hasCheckTurn = true;
                }
                else if(horizontal > 0.2 && _hasCheckTurn == false)
                {
                    _turnDirection = TurnDirection.Right;
                    _currentWaypoint = _checkPointToCheckCar;
                    _hasCheckTurn = true;
                }
                if (_canTurn == false)
                {
                    item.brakeTorque = 700;
                    item.motorTorque = 0;
                }
                else
                {
                    Debug.Log("one");

                    item.brakeTorque = 0;
                    item.motorTorque = totalPower;
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
        if(speed > _currentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
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
            if (other.GetComponent<WayPoints>().NextWaypoint.Length == 1)
            {
            _currentWaypoint = other.GetComponent<WayPoints>().NextWaypoint[0];
                _hasCheckTurn = false;
                _checkPointToCheckCar = null;

            }
            else
            {
                    _currentWaypoint = _turnWaypoint;
                
                

            }
           
        }
       
    }
   


}
