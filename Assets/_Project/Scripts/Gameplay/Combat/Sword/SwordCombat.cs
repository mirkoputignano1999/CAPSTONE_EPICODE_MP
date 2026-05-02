using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int AttackModeHash = Animator.StringToHash("AttackMode");
    private static readonly int AttackXHash = Animator.StringToHash("AttackX");
    private static readonly int AttackYHash = Animator.StringToHash("AttackY");

    private const int BaseAttackMode = 0;
    private const int SpinSlashMode = 1;
    private const int WideSpinMode = 2;

    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Animator _animator;

    [Header("Base Attack")]
    [SerializeField] private MeleeHitbox _upHitbox;
    [SerializeField] private MeleeHitbox _downHitbox;
    [SerializeField] private MeleeHitbox _leftHitbox;
    [SerializeField] private MeleeHitbox _rightHitbox;

    [Header("Spin Slash")]
    [SerializeField] private SpinSlashHitbox _spinSlashHitbox;

    [Header("Damage")]
    [SerializeField] private int _baseDamage = 1;
    [SerializeField] private int _spinSlashDamage = 2;
    [SerializeField] private int _wideSpinDamage = 2;

    [Header("Spin Radius")]
    [SerializeField] private float _spinSlashRadius = 0.9f;
    [SerializeField] private float _wideSpinRadius = 1.2f;

    [Header("Timing")]
    [SerializeField] private float _baseAttackDuration = 0.25f;
    [SerializeField] private float _baseHitboxStartupDelay = 0.04f;
    [SerializeField] private float _baseHitboxActiveTime = 0.12f;

    [SerializeField] private float _spinSlashDuration = 0.5f;
    [SerializeField] private float _spinSlashStartupDelay = 0.08f;
    [SerializeField] private float _spinSlashActiveTime = 0.18f;

    [SerializeField] private float _wideSpinDuration = 0.55f;
    [SerializeField] private float _wideSpinStartupDelay = 0.08f;
    [SerializeField] private float _wideSpinActiveTime = 0.22f;

    [Header("Behavior")]
    [SerializeField] private bool _lockMovementDuringAttack = true;

    private bool _isAttacking;

    private enum SwordAttackType
    {
        Base = 0,
        SpinSlash = 1,
        WideSpin = 2
    }

    private void Awake()
    {
        ValidateReferences();
        ApplyDamageToBaseHitboxes();
        DeactivateAllHitboxes();

        if (_spinSlashHitbox != null)
        {
            _spinSlashHitbox.SetDamage(_spinSlashDamage);
            _spinSlashHitbox.SetRadius(_spinSlashRadius);
            _spinSlashHitbox.DeactivateHitbox();
        }
    }

    private void Update()
    {
        if (_health != null && _health.IsDead)
        {
            return;
        }

        if (_inputHandler == null || _movement == null || _animator == null)
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

        if (_lockMovementDuringAttack)
        {
            _movement.SetMovementLocked(true);
        }

        SwordAttackType attackType = GetSelectedAttackType();
        Vector2 attackDirection = _movement.LastNonZeroDirection;

        SetAttackMode(attackType);
        UpdateAttackDirection(attackDirection);
        _animator.SetTrigger(AttackTriggerHash);

        switch (attackType)
        {
            case SwordAttackType.SpinSlash:
                yield return StartCoroutine(SpinSlashRoutine(_spinSlashDamage, _spinSlashRadius, _spinSlashDuration, _spinSlashStartupDelay, _spinSlashActiveTime));
                break;

            case SwordAttackType.WideSpin:
                yield return StartCoroutine(SpinSlashRoutine(_wideSpinDamage, _wideSpinRadius, _wideSpinDuration, _wideSpinStartupDelay, _wideSpinActiveTime));
                break;

            default:
                yield return StartCoroutine(BaseAttackRoutine(attackDirection));
                break;
        }

        if (_lockMovementDuringAttack)
        {
            _movement.SetMovementLocked(false);
        }

        _isAttacking = false;
    }

    private IEnumerator BaseAttackRoutine(Vector2 attackDirection)
    {
        MeleeHitbox selectedHitbox = GetHitboxFromDirection(attackDirection);

        DeactivateAllHitboxes();

        yield return new WaitForSeconds(_baseHitboxStartupDelay);

        if (selectedHitbox != null)
        {
            selectedHitbox.ActivateHitbox();
        }

        yield return new WaitForSeconds(_baseHitboxActiveTime);

        if (selectedHitbox != null)
        {
            selectedHitbox.DeactivateHitbox();
        }

        float remainingTime = _baseAttackDuration - _baseHitboxStartupDelay - _baseHitboxActiveTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }
    }

    private IEnumerator SpinSlashRoutine(int damage, float radius, float totalDuration, float startupDelay, float activeTime)
    {
        if (_spinSlashHitbox == null)
        {
            yield break;
        }

        _spinSlashHitbox.SetDamage(damage);
        _spinSlashHitbox.SetRadius(radius);
        _spinSlashHitbox.DeactivateHitbox();

        yield return new WaitForSeconds(startupDelay);

        _spinSlashHitbox.ActivateHitbox();

        yield return new WaitForSeconds(activeTime);

        _spinSlashHitbox.DeactivateHitbox();
        _spinSlashHitbox.ResetRadius();

        float remainingTime = totalDuration - startupDelay - activeTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }
    }

    private SwordAttackType GetSelectedAttackType()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return SwordAttackType.Base;
        }

        GameStateManager gameStateManager = GameManager.Instance.GameStateManager;

        if (gameStateManager.HasAbilityUnlocked(CharacterType.Sword, "Sword_WideSpin"))
        {
            return SwordAttackType.WideSpin;
        }

        if (gameStateManager.HasAbilityUnlocked(CharacterType.Sword, "Sword_SpinSlash"))
        {
            return SwordAttackType.SpinSlash;
        }

        return SwordAttackType.Base;
    }

    private void SetAttackMode(SwordAttackType attackType)
    {
        int modeValue = attackType switch
        {
            SwordAttackType.Base => BaseAttackMode,
            SwordAttackType.SpinSlash => SpinSlashMode,
            SwordAttackType.WideSpin => WideSpinMode,
            _ => BaseAttackMode
        };

        _animator.SetInteger(AttackModeHash, modeValue);
    }

    private void UpdateAttackDirection(Vector2 direction)
    {
        _animator.SetFloat(AttackXHash, direction.x);
        _animator.SetFloat(AttackYHash, direction.y);
    }

    private void ApplyDamageToBaseHitboxes()
    {
        if (_upHitbox != null) _upHitbox.SetDamage(_baseDamage);
        if (_downHitbox != null) _downHitbox.SetDamage(_baseDamage);
        if (_leftHitbox != null) _leftHitbox.SetDamage(_baseDamage);
        if (_rightHitbox != null) _rightHitbox.SetDamage(_baseDamage);
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

    private void DeactivateAllHitboxes()
    {
        if (_upHitbox != null) _upHitbox.DeactivateHitbox();
        if (_downHitbox != null) _downHitbox.DeactivateHitbox();
        if (_leftHitbox != null) _leftHitbox.DeactivateHitbox();
        if (_rightHitbox != null) _rightHitbox.DeactivateHitbox();
        if (_spinSlashHitbox != null) _spinSlashHitbox.DeactivateHitbox();
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

        if (_health == null)
        {
            Debug.LogError($"{name}: PlayerHealth reference is missing.");
        }

        if (_animator == null)
        {
            Debug.LogError($"{name}: Animator reference is missing.");
        }
    }
}