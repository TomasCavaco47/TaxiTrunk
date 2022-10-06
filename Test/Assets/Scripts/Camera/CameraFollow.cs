using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private Transform _target;
    [SerializeField] private float _translationSpeed;
    [SerializeField] private float _rotationSpeed;


    private void FixedUpdate()
    {
        HandleRotation();
        HandleTranslation();
    }
    private void HandleTranslation()
    {
        Vector3 targetPosition = _target.TransformPoint(_offSet);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _translationSpeed * Time.deltaTime);

    }

    private void HandleRotation()
    {
        var direction = _target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
}
