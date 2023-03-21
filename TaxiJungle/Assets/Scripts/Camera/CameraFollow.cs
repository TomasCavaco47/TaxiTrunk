using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private Transform _target;
    [SerializeField] private float _translationSpeed;
    [SerializeField] private float _rotationSpeed;

    public Transform Target { get => _target; set => _target = value; }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleTranslation();
    }
    private void HandleTranslation()
    {
        Vector3 targetPosition = Target.TransformPoint(_offSet);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _translationSpeed * Time.deltaTime);

    }

    private void HandleRotation()
    {
        var direction = Target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
}
