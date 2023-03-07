using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_NavigatorController : MonoBehaviour
{
    [SerializeField] Vector3 _destination;
    Vector3 _lastPosition;
    [SerializeField] bool _reachedDestination;
    [SerializeField] float _stopDistance = 1;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _minSpeed, _maxSpeed;
    [SerializeField] float _movementSpeed;
    Vector3 _velocity;
    [SerializeField]Animator _animator;
    [SerializeField] bool _onBranch=false;


    public bool ReachedDestination { get => _reachedDestination; set => _reachedDestination = value; }
    public bool OnBranch { get => _onBranch; set => _onBranch = value; }
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();


    }
    private void Start()
    {
        MovementSpeed = Random.Range(_minSpeed, _maxSpeed);
        _animator.speed = MovementSpeed/2;
    }
    private void Update()
    {
        if (transform.position != _destination)
        {
            Vector3 destinationDirection = _destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= _stopDistance)
            {
                ReachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
            }
            else
            {
                ReachedDestination = true;
            }

            _velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _velocity.y = 0;
            var velocityMagnitude = _velocity.magnitude;
            _velocity = _velocity.normalized;
            var fwdDotProduct = Vector3.Dot(transform.forward, _velocity);
            var rightDotProduct = Vector3.Dot(transform.right, _velocity);
          //  _animator.SetFloat("horizontal", rightDotProduct);
            //_animator.SetFloat("Forward", fwdDotProduct);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this._destination = destination;
        ReachedDestination = false;
    }

}
