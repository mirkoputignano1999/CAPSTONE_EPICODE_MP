using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int LastMoveXHash = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveYHash = Animator.StringToHash("LastMoveY");

    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_movement == null)
        {
            Debug.LogError($"{name}: PlayerMovement reference is missing.");
        }

        if (_animator == null)
        {
            Debug.LogError($"{name}: Animator reference is missing.");
        }
    }

    private void Update()
    {
        if (_movement == null || _animator == null)
        {
            return;
        }

        Vector2 movementInput = _movement.MovementInput;
        Vector2 lastDirection = _movement.LastNonZeroDirection;
        bool isMoving = _movement.IsMoving;

        Vector2 cardinalMoveDirection = isMoving
            ? GetCardinalDirection(movementInput)
            : Vector2.zero;

        _animator.SetBool(IsMovingHash, isMoving);
        _animator.SetFloat(MoveXHash, cardinalMoveDirection.x);
        _animator.SetFloat(MoveYHash, cardinalMoveDirection.y);
        _animator.SetFloat(LastMoveXHash, lastDirection.x);
        _animator.SetFloat(LastMoveYHash, lastDirection.y);
    }

    private Vector2 GetCardinalDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude <= 0.001f)
        {
            return Vector2.zero;
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0f ? Vector2.right : Vector2.left;
        }

        return direction.y > 0f ? Vector2.up : Vector2.down;
    }
}
