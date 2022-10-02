using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Pesquisar
        _currentBreakingForce = _isBraking ? _brakeForce : 0f;

        ApplyBraking();
    }

    private void ApplyBraking()
    {
        _frontLeftWheelCollider.brakeTorque = _currentBreakingForce;
        _frontRightWheelCollider.brakeTorque = _currentBreakingForce; _backLeftWheelCollider.brakeTorque = _currentBreakingForce;
        _backRightWheelCollider.brakeTorque = _currentBreakingForce; _backLeftWheelCollider.brakeTorque = _currentBreakingForce;
        _backLeftWheelCollider.brakeTorque = _currentBreakingForce;
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
