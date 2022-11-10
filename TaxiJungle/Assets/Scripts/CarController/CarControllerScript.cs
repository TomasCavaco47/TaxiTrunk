using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SpeedType
{
    KPH,
    MPH
}
public class CarControllerScript : MonoBehaviour
{
  
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _topSpeed;
    [SerializeField] private SpeedType _speedType;
    [SerializeField] private float _antiRoll = 1000f;
    [SerializeField] private bool _tractionControl = true;
    [SerializeField] private float _slipLimit = 0.3f;
    [SerializeField] private bool _steeringAssist = true;
    [SerializeField] private float _steeringAssistRatio = 0.5f;
    [SerializeField] private WheelCollider[] _wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] _wheelMeshes = new Transform[4];

    private bool _canMove=false;
    private GameManager _gameManager;

    private Rigidbody _rb;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _isReversing = false;
    private float _rotationInPreviousFrame;
    private int _currentSpeed;

    public int CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }
    private void Start()
    {
        Debug.Log(_rb.centerOfMass);
        _rb.centerOfMass = new Vector3(0,0.25f,-0.02f);
        

        // Time.timeScale = 1.5f;
        //Time.fixedDeltaTime =0.03f;
         Time.timeScale = 1f;
        // Time.timeScale = 0.9f;
        //Time.fixedDeltaTime =Time.timeScale* 0.02f;
        Time.fixedDeltaTime =0.02f;

        _gameManager = GameManager.instance;
        

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.velocity *= 0.9f;
        }
        Debug.Log(_currentSpeed);
        _currentSpeed = Mathf.RoundToInt(_rb.velocity.magnitude * 3.6f);
    }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
            UpdateCurrentSpeed();
        HandleSteering();
        HandleDrive();
        HandleWheelTransform();

        //AntiRoll();
        DetectReverse();
        TractionControl();
        SteeringAssist();
      
    }
    // Verify current speed in KMH or MPH
    private void UpdateCurrentSpeed()
    {
        //if (_speedType == SpeedType.KPH)
        //{
        //    CurrentSpeed = _rb.velocity.magnitude * 3.6f;
        //}
        //else
        //{
        //    CurrentSpeed = _rb.velocity.magnitude * 2.23693629f;
        //}
    }
    //Steers the car
    private void HandleSteering()
    {
        _wheelColliders[0].steerAngle = _maxSteerAngle * _horizontalInput;
        _wheelColliders[1].steerAngle = _maxSteerAngle * _horizontalInput;
    }
    // Makes the car acelarate, brake ,reverse and adds  drag when there is no input 
    private void HandleDrive()
    {
        _wheelColliders[0].motorTorque = _motorForce * _verticalInput / 2;
        _wheelColliders[1].motorTorque = _motorForce * _verticalInput / 2;

        if (!_isReversing && _verticalInput < 0 && _rb.velocity.magnitude > 1)
        {
            
            ApplyBrakes();
        }
        else if(_isReversing && _verticalInput > 0 && _wheelColliders[3].rpm <0)
        {
            ApplyBrakes();
        }
        else
        {
            ResetBrakes();

        }
        if (_verticalInput == 0 || _isReversing)
        {
            _rb.drag = Mathf.Lerp(GetComponent<Rigidbody>().drag, 0.5f, Time.deltaTime * 2);

        }
        else
        {
            _rb.drag = 0;
        }
    }
    // Brakes the car while accerlarating or reversing
    private void ApplyBrakes()
    {
       
        if(_isReversing)
        {
            for (int i = 0; i < _wheelColliders.Length; i++)
            {
                _wheelColliders[i].brakeTorque = _brakeForce * _verticalInput;
            }
        }
        else
        {
            for (int i = 0; i < _wheelColliders.Length; i++)
            {
                _wheelColliders[i].brakeTorque = -_brakeForce * _verticalInput;
            }
        }
    }
    // Stop the brakes 
    private void ResetBrakes()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = 0f;
        }
    }
    // Makes The wheels visualy turning and rotating
    private void HandleWheelTransform()
    {
        for (int i = 0; i < _wheelMeshes.Length; i++)
        {
            Vector3 pos = _wheelMeshes[i].position;
            Quaternion quat = _wheelMeshes[i].rotation;

            _wheelColliders[i].GetWorldPose(out pos, out quat);

            _wheelMeshes[i].position = pos;
            _wheelMeshes[i].rotation = quat;
        }
    }
    // Helps the car to dont roll over
    private void AntiRoll()
    {
        // Front axle
        ApplyAntiRoll(_wheelColliders[0], _wheelColliders[1]);
        // Back axle
        ApplyAntiRoll(_wheelColliders[2], _wheelColliders[3]);
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

    // Detects if we are reversing
    private void DetectReverse()
    {
        float rpmSum = 0f;
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            rpmSum += _wheelColliders[i].rpm;
        }
        _isReversing = rpmSum / _wheelColliders.Length < 0;

    }

    // Helps the car to dont slip and makes it easy to control it
    private void TractionControl()
    {
        if (_tractionControl)
        {
            WheelHit hit;
            _wheelColliders[0].GetGroundHit(out hit);
            if (hit.forwardSlip >= _slipLimit && _wheelColliders[0].motorTorque > 0)
            {
                _wheelColliders[0].motorTorque *= 0.9f;
            }
            _wheelColliders[1].GetGroundHit(out hit);
            if (hit.forwardSlip >= _slipLimit && _wheelColliders[1].motorTorque > 0)
            {
                _wheelColliders[1].motorTorque *= 0.9f;
            }
        }
    }

    // Helps to turn the car
    private void SteeringAssist()
    {
        if (Mathf.Abs(_rotationInPreviousFrame - transform.eulerAngles.y) < 10f && _steeringAssist)
        {
            var turnadjust = (transform.eulerAngles.y - _rotationInPreviousFrame) * _steeringAssistRatio;
            Quaternion velocityRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            _rb.velocity = velocityRotation * _rb.velocity;
        }
        _rotationInPreviousFrame = transform.eulerAngles.y;
    }


}



