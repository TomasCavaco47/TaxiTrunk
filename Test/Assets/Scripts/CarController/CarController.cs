using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public enum SpeedUnit
{
    Mph,
    kmh
}

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float _horizontalInput;
    private float _verticalInput;
    private float _currentStearingAngle;
    private float _currentBreakingForce;
    private bool _isBraking;

    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteerAngle;

    [SerializeField] private WheelCollider _frontLeftWheelCollider;
    [SerializeField] private WheelCollider _frontRightWheelCollider;
    [SerializeField] private WheelCollider _backLeftWheelCollider;
    [SerializeField] private WheelCollider _backRightWheelCollider;

    [SerializeField] private Transform _frontLeftWheelTransform;
    [SerializeField] private Transform _frontRightWheelTransform;
    [SerializeField] private Transform _backLeftWheelTransform;
    [SerializeField] private Transform _backRightWheelTransform;

    //TESTES

    private float _currentSpeed;
    [SerializeField] private Text _speedText;
    [SerializeField] private SpeedUnit _speedUnit;
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
 


     void Update()
    {
        
        if (_speedUnit == SpeedUnit.Mph)
        {
            // 2.23694 is the constant to convert a value from m/s to mph.
            
            _currentSpeed = Mathf.RoundToInt(_rigidbody.velocity.magnitude * 2.23694f);

            // _speedText.text = currentSpeed.ToString() + "MPH";
        }
        else
        {
            // 3.6 is the constant to convert a value from m/s to km/h.           
            _currentSpeed = Mathf.RoundToInt(_rigidbody.velocity.magnitude * 3.6f);


            //_speedText.text = currentSpeed.ToString() + " km/h";

        }
        Debug.Log(_currentSpeed);
    }
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _isBraking = Input.GetKey(KeyCode.S);
    }

    private void HandleMotor()
    {
        _backLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
        _backRightWheelCollider.motorTorque = _verticalInput * _motorForce;

        if (_verticalInput==0)
        {
            _rigidbody.drag = Mathf.Lerp(GetComponent<Rigidbody>().drag, 0.5f, Time.deltaTime * 4);

        }
        else
        {
            _rigidbody.drag = 0;
        }

        //Pesquisar
        _currentBreakingForce = _isBraking ? _brakeForce : 0f;
       

        ApplyBraking();
    }

    private void ApplyBraking()
    {
        if (_rigidbody.velocity.z > 0 && _isBraking)
        {
            _frontLeftWheelCollider.brakeTorque = _brakeForce;
            _frontRightWheelCollider.brakeTorque = _brakeForce;
            _backRightWheelCollider.brakeTorque = _brakeForce;
            _backLeftWheelCollider.brakeTorque = _brakeForce;
        }
        else if(_rigidbody.velocity.z < 0 && _isBraking==false)
        {
            _frontLeftWheelCollider.brakeTorque = _brakeForce;
            _frontRightWheelCollider.brakeTorque = _brakeForce;
            _backRightWheelCollider.brakeTorque = _brakeForce;
            _backLeftWheelCollider.brakeTorque = _brakeForce;
        }
        else
        {
            _frontLeftWheelCollider.brakeTorque = 0;
            _frontRightWheelCollider.brakeTorque = 0;
            _backRightWheelCollider.brakeTorque = 0;
            _backLeftWheelCollider.brakeTorque = 0;
        }


    }

    private void HandleSteering()
    {
        _currentStearingAngle = _maxSteerAngle * _horizontalInput;

        _frontLeftWheelCollider.steerAngle = _currentStearingAngle;
        _frontRightWheelCollider.steerAngle = _currentStearingAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
        UpdateSingleWheel(_frontRightWheelCollider, _frontRightWheelTransform);
        UpdateSingleWheel(_backLeftWheelCollider, _backLeftWheelTransform);
        UpdateSingleWheel(_backRightWheelCollider, _backRightWheelTransform);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
