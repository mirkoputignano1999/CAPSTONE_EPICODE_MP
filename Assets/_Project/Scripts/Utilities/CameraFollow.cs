using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private Vector3 _offset;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _offset;
        desiredPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _smoothSpeed * Time.deltaTime
        );
    }
    public void SetTarget(Transform targetTransform)
    {
        _target = targetTransform;
    }

    public void SnapToTarget()
    {
        if (_target == null)
        {
            return;
        }

        _velocity = Vector3.zero;

        transform.position = new Vector3(
            _target.position.x,
            _target.position.y,
            transform.position.z);
    }
}

