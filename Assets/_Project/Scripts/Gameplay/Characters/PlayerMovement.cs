using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private PlayerInputHandler _inputHandler;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementInput;
    private Vector2 _lastNonZeroDirection = Vector2.down;

    public Vector2 MovementInput => _movementInput;
    public Vector2 LastNonZeroDirection => _lastNonZeroDirection;
    public bool IsMoving => _movementInput.sqrMagnitude > 0.001f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_inputHandler == null)
        {
            Debug.LogError($"{name}: PlayerInputHandler reference is missing.");
        }
    }

    private void Update()
    {
        _movementInput = _inputHandler != null ? _inputHandler.MoveInput : Vector2.zero;

        if (_movementInput.sqrMagnitude > 0.001f)
        {
            _lastNonZeroDirection = GetCardinalDirection(_movementInput);
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta = _movementInput.normalized * (_moveSpeed * Time.fixedDeltaTime);
        _rigidbody2D.MovePosition(_rigidbody2D.position + delta);
    }

    private Vector2 GetCardinalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0f ? Vector2.right : Vector2.left;
        }

        return direction.y > 0f ? Vector2.up : Vector2.down;
    }
}