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
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField] private driveType drive;

    [SerializeField] private int _version;
    private float _horizontalInput;
    private float _verticalInput;
    private float _currentStearingAngle;
    private float _currentBreakingForce;
    private bool _isBraking;

    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteerAngle;

   
    [SerializeField] private WheelCollider[] _wheelColliders;

  
    [SerializeField] private Transform[] _wheelTransforms;

    private Vector3 _wheelPosition;
    private Quaternion _wheelRotation;

    //TESTES

    private float _currentSpeed;
    [SerializeField] private Text _speedText;
    [SerializeField] private SpeedUnit _speedUnit;
    Rigidbody _rigidbody;


    private float _vertical;
    private float _horizontal;
    private float _finalTurnAngle;
    private float _radius;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass= new Vector3(0, 0.5f, -0.2f);
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
        SteerVehicle();
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
        _wheelColliders[2].motorTorque = _verticalInput * _motorForce;
        _wheelColliders[3].motorTorque = _verticalInput * _motorForce;

        if (_verticalInput==0)
        {
            _rigidbody.drag = Mathf.Lerp(GetComponent<Rigidbody>().drag, 0.3f, Time.deltaTime * 2);

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
            _wheelColliders[0].brakeTorque = _brakeForce;
            _wheelColliders[1].brakeTorque = _brakeForce;
            _wheelColliders[2].brakeTorque = _brakeForce;
            _wheelColliders[3].brakeTorque = _brakeForce;
        }
        else if(_rigidbody.velocity.z < 0 && _isBraking==false && _verticalInput == 0)
        {
            _wheelColliders[0].brakeTorque = _brakeForce;
            _wheelColliders[1].brakeTorque = _brakeForce;
            _wheelColliders[2].brakeTorque = _brakeForce;
            _wheelColliders[3].brakeTorque = _brakeForce;
        }
        else 
        {
            _wheelColliders[0].brakeTorque = 0;
            _wheelColliders[1].brakeTorque = 0;
            _wheelColliders[2].brakeTorque = 0;
            _wheelColliders[3].brakeTorque = 0;
        }


    }

    //private void HandleSteering()
    //{
    //    _currentStearingAngle = _maxSteerAngle * _horizontalInput;

    //    _frontLeftWheelCollider.steerAngle = _currentStearingAngle;
    //    _frontRightWheelCollider.steerAngle = _currentStearingAngle;
    //}

    //private void UpdateWheels()
    //{
    //    UpdateSingleWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
    //    UpdateSingleWheel(_frontRightWheelCollider, _frontRightWheelTransform);
    //    UpdateSingleWheel(_backLeftWheelCollider, _backLeftWheelTransform);
    //    UpdateSingleWheel(_backRightWheelCollider, _backRightWheelTransform);

    //}

 
    void SteerVehicle()
    {
        if(_version == 1)
        {
        float _steeringAngle;
        _steeringAngle = _maxSteerAngle * _horizontalInput;
        _wheelColliders[0].steerAngle = _steeringAngle;
        _wheelColliders[1].steerAngle = _steeringAngle;

        }
        else
        {
            _vertical = _horizontalInput;
            _horizontal = Mathf.Lerp(_horizontal, _horizontalInput, (_horizontalInput != 0) ? 5 * Time.deltaTime : 5 * 2 * Time.deltaTime);

            _finalTurnAngle = (_radius > 5) ? _radius : 5;

            if (_horizontal > 0)
            {
                //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
                _wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(4f / (_finalTurnAngle - (1.5f / 2))) * _horizontal;
                _wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(4f / (_finalTurnAngle + (1.5f / 2))) * _horizontal;
            }
            else if (_horizontal < 0)
            {
                _wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(4f / (_finalTurnAngle + (1.5f / 2))) * _horizontal;
                _wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(4f / (_finalTurnAngle - (1.5f / 2))) * _horizontal;
                //transform.Rotate(Vector3.up * steerHelping);

            }
            else
            {
                _wheelColliders[0].steerAngle = 0;
                _wheelColliders[1].steerAngle = 0;
            }

        }


    }

    void UpdateWheels()
    {

        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].GetWorldPose(out _wheelPosition, out _wheelRotation);
            _wheelTransforms[i].transform.rotation = _wheelRotation;
            _wheelTransforms[i].transform.position = _wheelPosition;
        }
    }


 
}
