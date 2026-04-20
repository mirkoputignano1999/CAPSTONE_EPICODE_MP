using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController2D : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int LastMoveXHash = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveYHash = Animator.StringToHash("LastMoveY");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HurtTriggerHash = Animator.StringToHash("Hurt");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    [SerializeField] private Animator _animator;

    private Vector2 _lastDirection = Vector2.down;

    private void Awake()
    {
        if (_animator == null)
        {
            Debug.LogError($"{name}: Animator reference is missing.");
        }

        SetFacingDirection(Vector2.down);
        UpdateMovement(Vector2.zero);
    }

    public void UpdateMovement(Vector2 velocity)
    {
        if (_animator == null)
        {
            return;
        }

        bool isMoving = velocity.sqrMagnitude > 0.001f;
        Vector2 cardinalDirection = isMoving ? GetCardinalDirection(velocity) : Vector2.zero;

        if (isMoving)
        {
            _lastDirection = cardinalDirection;
        }

        _animator.SetBool(IsMovingHash, isMoving);
        _animator.SetFloat(MoveXHash, cardinalDirection.x);
        _animator.SetFloat(MoveYHash, cardinalDirection.y);
        _animator.SetFloat(LastMoveXHash, _lastDirection.x);
        _animator.SetFloat(LastMoveYHash, _lastDirection.y);
    }

    public void SetFacingDirection(Vector2 direction)
    {
        if (_animator == null || direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        _lastDirection = GetCardinalDirection(direction);
        _animator.SetFloat(LastMoveXHash, _lastDirection.x);
        _animator.SetFloat(LastMoveYHash, _lastDirection.y);
    }

    public void TriggerAttack()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.ResetTrigger(HurtTriggerHash);
        _animator.SetTrigger(AttackTriggerHash);
    }

    public void TriggerHurt()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.ResetTrigger(AttackTriggerHash);
        _animator.SetTrigger(HurtTriggerHash);
    }

    public void SetDead()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetBool(IsDeadHash, true);
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