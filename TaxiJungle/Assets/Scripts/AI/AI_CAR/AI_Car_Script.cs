using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AI_Car_Script : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private float _totalPower;
    enum TurnDirection
    {
        Left,
        Center,
        Right
    }
    [SerializeField] private TurnDirection _turnDirection;
    
    [SerializeField] private Transform _currentWaypoint;
    [SerializeField] private Transform _nextWaypoint;
    private int _speed;
    private float _antiRoll = 1000f;
    [SerializeField] private float _distanceToWaypoint;
    [SerializeField] private bool _canMove;
    Rigidbody _rb;
    [SerializeField] Transform _checkFront;
    [SerializeField] Transform _massCenter;
    [SerializeField] float _size;
    [SerializeField] LayerMask _aiCarLayer;
    private int _numberOfcarsPassing;
    [SerializeField] List<GameObject> _carsLeft;
    [SerializeField] List<GameObject> _carsRight;
    [SerializeField] List<GameObject> _carsCenter;
    private float _horizontal;
    private float _radius=8;
    private bool _tractionControl=true;
    [SerializeField] private LayerMask _playerCarLayer;
    [SerializeField] private List<GameObject> _tempcarsinfront;
    private bool _carInFront;
    [SerializeField] float collisionTimer;
    [SerializeField] float timer;

    public Transform CurrentWaypoint { get => _currentWaypoint; set => _currentWaypoint = value; }
    public int Speed { get => _speed; set => _speed = value; }
    public float DistanceToWaypoint { get => _distanceToWaypoint; set => _distanceToWaypoint = value; }
    private TurnDirection TurnDirection1 { get => _turnDirection; set => _turnDirection = value; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _massCenter.localPosition;
    }
    private void Update()
    {
        CheckSpeed();
        CheckTurningDirection();
        if(Speed<1)
        {
            timer += Time.deltaTime;
            if(timer>35)
            {
                GameManager.instance.RespawnCars(this.gameObject);
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void FixedUpdate()
    {
        try
        {
            Accelarate();
            AntiRoll();
            ChangeWaypoint();
            SteerVehicle();
            TractionControl();
        }
        catch
        {
           
        }
    }
    private void Accelarate()
    {
        if(_currentWaypoint==null)
        {
            GameManager.instance.RespawnCars(this.gameObject);
        }
        List<Collider> hitColliders = new List<Collider>();
        hitColliders = Physics.OverlapSphere(_checkFront.position, 5+(Speed/2), _aiCarLayer + _playerCarLayer).ToList();
        _tempcarsinfront = new List<GameObject>();
        for (var i = 0; i < hitColliders.Count; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            Vector3 targetPos = tempTarget.position - transform.position; // find target direction
            Vector3 targetDir = tempTarget.forward - transform.forward; // find target direction
            Vector3 myDir = _checkFront.forward;
            float visionRadious = Vector3.SignedAngle(myDir, targetPos, Vector3.up);
            float targetAngle = Vector3.SignedAngle(myDir, targetDir, Vector3.up);
            if (visionRadious > -20 && visionRadious < 20)
            {
                if (targetAngle <= 100 && targetAngle >= 90 || targetAngle <= -90 && targetAngle >= -100)
                { 
                    _tempcarsinfront.Add(tempTarget.gameObject);
                    _tempcarsinfront = _tempcarsinfront.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.black);
                }
                else
                {
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
                }
            }
        } 
        if(_tempcarsinfront.Count==0)
        {
            _carInFront=false;
        }
        else
        {
            _carInFront = true;

        }
        foreach (var item in wheels)
        {
            if (_canMove)
            {
                if (_carInFront == false)
                {
                    if (Speed < CurrentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
                    {
                        item.motorTorque = _totalPower;
                        item.brakeTorque = 0;
                    }
                    else if (Speed > CurrentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
                    {
                        item.motorTorque = 0;
                        item.brakeTorque = 1000;
                    }
                    else
                    {
                        item.motorTorque = 0;
                        item.brakeTorque = 0;
                    }
                }
                else
                {
                    if (_tempcarsinfront[0].tag=="AICar")
                    {
                        if (Speed < _tempcarsinfront[0].transform.GetComponent<AI_Car_Script>().Speed)
                        {
                            item.motorTorque = _totalPower;
                            item.brakeTorque = 0;
                        }
                        else if (Speed > _tempcarsinfront[0].transform.GetComponent<AI_Car_Script>().Speed)
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 1000;
                        }
                        else
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 0;
                        }
                    }
                    if(_tempcarsinfront[0].tag == "Player")
                    {
                        if (Speed < _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed)
                        {
                            Debug.Log("1");
                            item.motorTorque = _totalPower;
                            item.brakeTorque = 0;
                        }
                        else if (Speed > _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed)
                        {
                            Debug.Log("2");
                            item.motorTorque = 0;
                            item.brakeTorque = 1000;
                        }
                        else
                        {
                            Debug.Log("3");
                            item.motorTorque = 0;
                            item.brakeTorque = 0;
                        }
                    }
                    
                    
                }
            }
            else
            {
                item.motorTorque = 0;
                item.brakeTorque = 1000;
            }
        }
    }
    private void SteerVehicle()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(CurrentWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 4;
        _horizontal = newSteer;
        if (_horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius + (1.5f / 2))) * _horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius - (1.5f / 2))) * _horizontal;
        }
        else if (_horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius - (1.5f / 2))) * _horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius + (1.5f / 2))) * _horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
        foreach (WheelCollider element in wheels)
        {
            HandleWheelTransform(element);
        }
    }
    void HandleWheelTransform(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }
        Transform tyre = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        tyre.transform.position = position;
        tyre.transform.rotation = rotation;
    }
    private void ChangeWaypoint()
    {
        DistanceToWaypoint = Vector3.Distance(transform.position, CurrentWaypoint.position);
        if (DistanceToWaypoint <= 5)
        {
            if (CurrentWaypoint.GetComponent<WayPoints>().HasATurn == false)
            {
                CheckpointAfterATurn();
                switch (CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
                {
                    case true:
                        if (CurrentWaypoint.GetComponent<WayPoints>().TraficLight.CarCanGo)
                        {
                            _canMove = true;
                            CurrentWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                            _nextWaypoint = null;
                        }
                        else
                        {
                            _canMove = false;
                        }
                        break;
                    case false:
                        CurrentWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                        _nextWaypoint = null;
                        break;
                    default:
                }
                CheckpointAfterATurn();
            }
            else
            {
                CheckCars();
                CheckpointAfterATurn();
            }
        }
        if (DistanceToWaypoint <= 25 && DistanceToWaypoint > 5)
        {
            if (CurrentWaypoint.GetComponent<WayPoints>().HasATurn)
            {
                if (Speed > 20)
                {
                    _canMove = false;
                }
                else
                {
                    _canMove = true;
                }
            }
            else
            {
                switch (CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
                {
                    case true:
                        if (CurrentWaypoint.GetComponent<WayPoints>().TraficLight.CarCanGo)
                        {
                            _canMove = true;
                        }
                        else
                        {
                            if (Speed > 20)
                            {
                                _canMove = false;
                            }
                            else
                            {
                                _canMove = true;
                            }
                        }
                        break;
                    case false:
                        _canMove = true;

                        break;
                    default:
                }
            }
        }
    }
    private void CheckSpeed()
    {
        Speed = Mathf.RoundToInt(_rb.velocity.magnitude * 3.6f);     
    }
    private void CheckTurningDirection()
    {
        if (_nextWaypoint != null)
        {
            Vector3 localPos = transform.InverseTransformPoint(_nextWaypoint.transform.position);
            if (localPos.x < -2)
            {
                TurnDirection1 = TurnDirection.Left;
            }
            else if (localPos.x > 2)
            {
                TurnDirection1 = TurnDirection.Right;
            }
            else
            {
                TurnDirection1 = TurnDirection.Center;
            }
        }
    }
    private void CheckCars()
    {
        _carsLeft = new List<GameObject>();
        _carsCenter = new List<GameObject>();
        _carsRight = new List<GameObject>();
        List<Collider> hitColliders = new List<Collider>();
        hitColliders = Physics.OverlapSphere(_checkFront.position, _size, _aiCarLayer).ToList();
        for (var i = 0; i < hitColliders.Count; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            Vector3 targetPos = tempTarget.position - _checkFront.position; // find target direction
            Vector3 targetDir = tempTarget.forward - _checkFront.forward; // find target direction
            Vector3 myDir = _checkFront.forward;
            float visionRadious = Vector3.SignedAngle(myDir, targetPos, Vector3.up);
            float targetAngle = Vector3.SignedAngle(myDir, targetDir, Vector3.up);
            
            if (visionRadious >= -105 && visionRadious <= -55)
            {
                if (targetAngle <= 155 && targetAngle >= 105)
                {
                    _carsLeft.Add(tempTarget.gameObject);
                    _carsLeft = _carsLeft.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.magenta);
                }
                else
                {
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
                }
            }
            else if (visionRadious > -55 && visionRadious < 20)
            {
                if (targetAngle <= -110 && targetAngle >= -180 || targetAngle <= 180 && targetAngle >= 105)
                {
                    _carsCenter.Add(tempTarget.gameObject);
                    _carsCenter = _carsCenter.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.green);
                }
                else
                {
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
                }
            }
            else if (visionRadious >= 20 && visionRadious <= 90)
            {
                if (targetAngle <= -112 && targetAngle >= -157)
                {
                    
                        _carsRight.Add(tempTarget.gameObject);
                        _carsRight = _carsRight.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.yellow);
                    
                }
                else
                {
                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
                }
            }
            else
            {
                Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, UnityEngine.Color.red);
            }
        }
        if (CurrentWaypoint.GetComponent<WayPoints>().Stop)
        {
            switch (TurnDirection1)
            {
                case TurnDirection.Left:
                    if (_carsLeft.Count == 0 && _carsRight.Count == 0)
                    {
                        if (_carsCenter.Count > 0)
                        {
                            if (_carsCenter[0].GetComponent<AI_Car_Script>().CurrentWaypoint.GetComponent<WayPoints>().Stop)
                            {
                                switch (_carsCenter[0].GetComponent<AI_Car_Script>().TurnDirection1)
                                {

                                    case TurnDirection.Left:
                                        if (DistanceToWaypoint < _carsCenter[0].GetComponent<AI_Car_Script>().DistanceToWaypoint)
                                        {
                                            _currentWaypoint = _nextWaypoint;
                                            _canMove = true;
                                        }
                                        else
                                        {
                                            _canMove = false;
                                        }
                                        break;
                                    case TurnDirection.Center:
                                        _canMove = false;
                                        break;
                                    case TurnDirection.Right:
                                        _canMove = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                _canMove = false;
                            }
                        }
                        else
                        {
                            _currentWaypoint = _nextWaypoint;
                            _canMove = true;
                        }
                    }
                    else
                    {
                        _canMove = false;
                    }

                    break;
                case TurnDirection.Center:
                    if (_carsLeft.Count == 0 && _carsRight.Count == 0)
                    {
                        if (_carsCenter.Count > 0)
                        {
                            if (_carsCenter[0].GetComponent<AI_Car_Script>().CurrentWaypoint.GetComponent<WayPoints>().Stop)
                            {
                                switch (_carsCenter[0].GetComponent<AI_Car_Script>().TurnDirection1)
                                {

                                    case TurnDirection.Left:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    case TurnDirection.Center:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    case TurnDirection.Right:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                _canMove = false;
                            }
                        }
                        else
                        {
                            _currentWaypoint = _nextWaypoint;
                            _canMove = true;
                        }
                    }
                    else
                    {
                        _canMove = false;
                    }
                    break;
                case TurnDirection.Right:
                    if (_carsLeft.Count == 0 && _carsRight.Count == 0)
                    {
                        if (_carsCenter.Count > 0)
                        {
                            if (_carsCenter[0].GetComponent<AI_Car_Script>().CurrentWaypoint.GetComponent<WayPoints>().Stop)
                            {
                                switch (_carsCenter[0].GetComponent<AI_Car_Script>().TurnDirection1)
                                {
                                    case TurnDirection.Left:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    case TurnDirection.Center:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    case TurnDirection.Right:
                                        _currentWaypoint = _nextWaypoint;
                                        _canMove = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                _canMove = false;
                            }
                        }
                        else
                        {
                            _currentWaypoint = _nextWaypoint;
                            _canMove = true;
                        }
                    }
                    else
                    {
                        _canMove = false;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (TurnDirection1)
            {
                case TurnDirection.Left:
                    if (_carsCenter.Count == 0)
                    {
                        _currentWaypoint = _nextWaypoint;
                        _canMove = true;
                    }
                    else
                    {
                        _canMove = false;
                    }
                    break;
                case TurnDirection.Center:
                    _currentWaypoint = _nextWaypoint;
                    _canMove = true;

                    break;
                case TurnDirection.Right:
                    _currentWaypoint = _nextWaypoint;
                    _canMove = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void CheckpointAfterATurn()
    {
        int i = Random.Range(0, CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length);
        _nextWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[i];
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
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_checkFront.position, _size);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AICARCONTROLLE2>() != null || other.GetComponent<CarControllerTest>() != null)
        {
            if (other.gameObject != this.gameObject)
            {
                _canMove = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<AICARCONTROLLE2>() != null || other.GetComponent<CarControllerTest>() != null)
        {
            if (other.gameObject != this.gameObject)
            {
                _canMove = true;
            }
        }
    }
    // preciso de detetar colisoes com os predios e mapa exepto o chao, caso ele falhe
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag=="AICar")
        {
        collisionTimer = 0;

        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "AICar" )
        {
            collisionTimer += Time.deltaTime;

        }
        if(collisionTimer>=6)
        {
            GameManager.instance.RespawnCars(this.gameObject);
            collisionTimer = 0;
        }
        
    }
}

