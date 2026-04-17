using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    private static readonly int AttackXHash = Animator.StringToHash("AttackX");
    private static readonly int AttackYHash = Animator.StringToHash("AttackY");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Animator _animator;

    [Header("Hitboxes")]
    [SerializeField] private MeleeHitbox _upHitbox;
    [SerializeField] private MeleeHitbox _downHitbox;
    [SerializeField] private MeleeHitbox _leftHitbox;
    [SerializeField] private MeleeHitbox _rightHitbox;

    [Header("Attack Settings")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackDuration = 0.25f;
    [SerializeField] private float _hitboxStartupDelay = 0f;
    [SerializeField] private float _hitboxActiveTime = 0.10f;
    [SerializeField] private bool _lockMovementDuringAttack = true;

    private bool _isAttacking;

    public bool IsAttacking => _isAttacking;

    private void Awake()
    {
        ValidateReferences();
        ApplyDamageToHitboxes();
    }

    private void Update()
    {
        if (_inputHandler == null || _movement == null)
        {
            return;
        }

        if (_isAttacking)
        {
            return;
        }

        if (_inputHandler.AttackPressedThisFrame)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        DeactivateAllHitboxes();

        if (_lockMovementDuringAttack)
        {
            _movement.SetMovementLocked(true);
        }

        Vector2 attackDirection = _movement.LastNonZeroDirection;
        MeleeHitbox selectedHitbox = GetHitboxFromDirection(attackDirection);

        UpdateAnimatorDirection(attackDirection);
        _animator.SetTrigger(AttackTriggerHash);

        yield return new WaitForSeconds(_hitboxStartupDelay);

        if (selectedHitbox != null)
        {
            selectedHitbox.ActivateHitbox();
        }

        yield return new WaitForSeconds(_hitboxActiveTime);

        if (selectedHitbox != null)
        {
            selectedHitbox.DeactivateHitbox();
        }

        float remainingTime = _attackDuration - _hitboxStartupDelay - _hitboxActiveTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        if (_lockMovementDuringAttack)
        {
            _movement.SetMovementLocked(false);
        }

        _isAttacking = false;
    }

    private void ValidateReferences()
    {
        if (_inputHandler == null)
        {
            Debug.LogError($"{name}: PlayerInputHandler reference is missing.");
        }

        if (_movement == null)
        {
            Debug.LogError($"{name}: PlayerMovement reference is missing.");
        }

        if (_animator == null)
        {
            Debug.LogError($"{name}: Animator reference is missing.");
        }
    }

    private void DeactivateAllHitboxes()
    {
        if (_upHitbox != null) _upHitbox.DeactivateHitbox();
        if (_downHitbox != null) _downHitbox.DeactivateHitbox();
        if (_leftHitbox != null) _leftHitbox.DeactivateHitbox();
        if (_rightHitbox != null) _rightHitbox.DeactivateHitbox();
    }

    private void ApplyDamageToHitboxes()
    {
        if (_upHitbox != null) _upHitbox.SetDamage(_damage);
        if (_downHitbox != null) _downHitbox.SetDamage(_damage);
        if (_leftHitbox != null) _leftHitbox.SetDamage(_damage);
        if (_rightHitbox != null) _rightHitbox.SetDamage(_damage);
    }

    private MeleeHitbox GetHitboxFromDirection(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return _upHitbox;
        }

        if (direction == Vector2.down)
        {
            return _downHitbox;
        }

        if (direction == Vector2.left)
        {
            return _leftHitbox;
        }

        return _rightHitbox;
    }

    private void UpdateAnimatorDirection(Vector2 direction)
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetFloat(AttackXHash, direction.x);
        _animator.SetFloat(AttackYHash, direction.y);
    }
}