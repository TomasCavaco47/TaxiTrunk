using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelElements
{
    [SerializeField] WheelCollider _leftWheel;
    [SerializeField] WheelCollider _rightWheel;

    [SerializeField] bool _addWheelTorque;
    [SerializeField] bool _ShouldSteer;

    public WheelCollider LeftWheel { get => _leftWheel; set => _leftWheel = value; }
    public WheelCollider RightWheel { get => _rightWheel; set => _rightWheel = value; }
    public bool AddWheelTorque { get => _addWheelTorque; set => _addWheelTorque = value; }
    public bool ShouldSteer { get => _ShouldSteer; set => _ShouldSteer = value; }
    
}
enum SpeedType
{
    KPH,
    MPH
}
public class CarControllerTest : MonoBehaviour
{
    [SerializeField] List<WheelElements> _wheelData;

    [SerializeField] float _maxTorque;
    [SerializeField] float _brakeTorque;
    [SerializeField] float _maxSteerAngle =30;

    private Rigidbody _rigidBody;
    [SerializeField] private Transform _massCenter;

    [SerializeField] private SpeedType _speedType;
    private int _currentSpeed;
    [SerializeField]private bool _reachedMaxSpeed;
    [SerializeField] private int _maxSpeed;
    [SerializeField] AnimationCurve _drag;
    [SerializeField] WheelFrictionCurve _normal;
    [SerializeField] WheelFrictionCurve _new;
    bool _canMove =true;

    public int CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _normal = _wheelData[0].RightWheel.sidewaysFriction;
            _new = _normal;
        _new.extremumSlip = 0.1f;
        _new.stiffness = 1f;
    }
    void Start()
    {
        _rigidBody.centerOfMass = _massCenter.localPosition;
    }

    void Update()
    {
        Drag();
        LimitMaxSpeed();
        UpdateCurrentSpeed();
        HandBreake();
    }
    private void FixedUpdate()
    {
       


        HandleSteering();
        HandleDrive();
      
    }
    private void HandleSteering()
    {
        float steer = Input.GetAxis("Horizontal") * _maxSteerAngle;

        foreach (WheelElements element in _wheelData)
        {
            if (element.ShouldSteer)
            {
                if(Input.GetAxisRaw("Horizontal") == 0)
                {
                    element.LeftWheel.steerAngle = 0;
                    element.RightWheel.steerAngle = 0;
                }
                else
                {
                    element.LeftWheel.steerAngle = steer;
                    element.RightWheel.steerAngle = steer;

                }
            }     
            HandleWheelTransform(element.LeftWheel);
            HandleWheelTransform(element.RightWheel);
        }
    }
    private void HandleDrive()
    {
        float input = Input.GetAxis("Vertical") * _maxTorque;
        if(CanMove)
        {
            //Accelerate
            if (input > 0)
            {
                if (_reachedMaxSpeed == false)
                {

                    foreach (WheelElements element in _wheelData)
                    {
                        if (element.AddWheelTorque)
                        {
                            element.RightWheel.brakeTorque = 0;
                            element.LeftWheel.brakeTorque = 0;
                            element.LeftWheel.motorTorque = input;
                            element.RightWheel.motorTorque = input;
                        }
                        HandleWheelTransform(element.LeftWheel);
                        HandleWheelTransform(element.RightWheel);
                    }

                }
                else
                {

                    Debug.LogWarning("222");

                    foreach (WheelElements element in _wheelData)
                    {
                        if (element.AddWheelTorque)
                        {
                            element.RightWheel.brakeTorque = 0;
                            element.LeftWheel.brakeTorque = 0;
                            element.LeftWheel.motorTorque = 0;
                            element.RightWheel.motorTorque = 0;
                        }
                        HandleWheelTransform(element.LeftWheel);
                        HandleWheelTransform(element.RightWheel);
                    }


                }
            }
            //Brake
            if (input < 0 && _wheelData[0].LeftWheel.rpm > 0)
            {

                foreach (WheelElements element in _wheelData)
                {
                    if (element.AddWheelTorque)
                    {
                        element.LeftWheel.motorTorque = 0;
                        element.RightWheel.motorTorque = 0;

                        element.RightWheel.brakeTorque = _brakeTorque;
                        element.LeftWheel.brakeTorque = _brakeTorque;

                    }
                    HandleWheelTransform(element.LeftWheel);
                    HandleWheelTransform(element.RightWheel);
                }
            }
            //Reverse
            if (input < 0 && _wheelData[0].LeftWheel.rpm <= 0)
            {
                if (_reachedMaxSpeed == false)
                {
                    foreach (WheelElements element in _wheelData)
                    {
                        if (element.AddWheelTorque)
                        {
                            element.RightWheel.brakeTorque = 0;
                            element.LeftWheel.brakeTorque = 0;
                            element.LeftWheel.motorTorque = input;
                            element.RightWheel.motorTorque = input;
                        }
                        HandleWheelTransform(element.LeftWheel);
                        HandleWheelTransform(element.RightWheel);
                    }

                }
                else
                {
                    foreach (WheelElements element in _wheelData)
                    {
                        if (element.AddWheelTorque)
                        {
                            element.RightWheel.brakeTorque = 0;
                            element.LeftWheel.brakeTorque = 0;
                            element.LeftWheel.motorTorque = 0;
                            element.RightWheel.motorTorque = 0;
                        }
                        HandleWheelTransform(element.LeftWheel);
                        HandleWheelTransform(element.RightWheel);
                    }
                }
            }
        }
        else
        {
            foreach (WheelElements element in _wheelData)
            {
                if (element.AddWheelTorque)
                {
                    element.RightWheel.brakeTorque = 1000;
                    element.LeftWheel.brakeTorque = 1000;
                    element.LeftWheel.motorTorque = 0;
                    element.RightWheel.motorTorque = 0;
                }
                HandleWheelTransform(element.LeftWheel);
                HandleWheelTransform(element.RightWheel);
            }
        }
        
        
        
    }
    void HandleWheelTransform (WheelCollider collider)
    {
        if(collider.transform.childCount ==0)
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
    private void UpdateCurrentSpeed()
    {
        if (_speedType == SpeedType.KPH)
        {
            CurrentSpeed = Mathf.RoundToInt(_rigidBody.velocity.magnitude * 3.6f);
        }
        else
        {
            CurrentSpeed = Mathf.RoundToInt(_rigidBody.velocity.magnitude * 2.23693629f);
        }
        //Debug.Log(_currentSpeed);
    }
    void LimitMaxSpeed()
    {
        if(Input.GetAxisRaw("Vertical") ==1)
        {
            if (_currentSpeed >= _maxSpeed)
            {
                _reachedMaxSpeed = true;
            }
            else
            {
                _reachedMaxSpeed = false;
            }

        }
        else if(Input.GetAxisRaw("Vertical") == -1)
        {
            if (_currentSpeed >= _maxSpeed/3)
            {
                _reachedMaxSpeed = true;
            }
            else
            {
                _reachedMaxSpeed = false;
            }
        }
    }
    void Drag()
    {
        if (Input.GetAxis("Vertical") == 0)
        {
            _rigidBody.drag = _drag.Evaluate(_currentSpeed);
        }
        else
        {
            _rigidBody.drag = 0;

        }
    }
    private void HandBreake()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (WheelElements element in _wheelData)
            {
                WheelFrictionCurve teste = _normal;
                teste.stiffness = 0.4f;
                element.RightWheel.sidewaysFriction = teste;
                element.LeftWheel.sidewaysFriction = teste;
            }


        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            
            foreach (WheelElements element in _wheelData)
            {
               
                element.RightWheel.sidewaysFriction =  _normal;
                element.LeftWheel.sidewaysFriction =  _normal;
            }
        }

    }
    //private void GForce()
    //{
    
   


    //    float G;
    //    float steer = _wheelData[0].LeftWheel.steerAngle;
    //    float a = _currentSpeed * steer;
    //    G = a / 9.81f;
    //    Debug.Log( G);
    //}
}
