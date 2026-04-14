using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private PlayerInputHandler _inputHandler;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementInput;
    private Vector2 _lastNonZeroDirection = Vector2.down;

    public Vector2 LastNonZeroDirection => _lastNonZeroDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movementInput = _inputHandler != null ? _inputHandler.MoveInput : Vector2.zero;

        if (_movementInput.sqrMagnitude > 0.001f)
        {
            _lastNonZeroDirection = GetFacingDirection(_movementInput);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = _rigidbody2D.position + _movementInput.normalized * (_moveSpeed * Time.fixedDeltaTime);
        _rigidbody2D.MovePosition(targetPosition);
    }

    private Vector2 GetFacingDirection(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
        {
            return inputDirection.x > 0f ? Vector2.right : Vector2.left;
        }

        return inputDirection.y > 0f ? Vector2.up : Vector2.down;
    }
}
