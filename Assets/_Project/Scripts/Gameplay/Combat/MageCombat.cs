using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCombat : MonoBehaviour
{
    private static readonly int AttackXHash = Animator.StringToHash("AttackX");
    private static readonly int AttackYHash = Animator.StringToHash("AttackY");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _castPoint;
    [SerializeField] private MagicProjectile _projectilePrefab;

    [Header("Cast Settings")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _castDuration = 0.35f;
    [SerializeField] private float _projectileSpawnDelay = 0.12f;
    [SerializeField] private bool _lockMovementDuringCast = true;

    [Header("Cast Offsets")]
    [SerializeField] private Vector2 _castOffsetUp = new(0f, 0.45f);
    [SerializeField] private Vector2 _castOffsetDown = new(0f, -0.45f);
    [SerializeField] private Vector2 _castOffsetLeft = new(-0.45f, 0f);
    [SerializeField] private Vector2 _castOffsetRight = new(0.45f, 0f);

    private bool _isCasting;

    public bool IsCasting => _isCasting;

    private void Awake()
    {
        ValidateReferences();
    }

    private void Update()
    {
        if (_inputHandler == null || _movement == null || _animator == null)
        {
            return;
        }

        if (_isCasting)
        {
            return;
        }

        if (_inputHandler.AttackPressedThisFrame)
        {
            StartCoroutine(CastRoutine());
        }
    }

    private IEnumerator CastRoutine()
    {
        _isCasting = true;

        if (_lockMovementDuringCast)
        {
            _movement.SetMovementLocked(true);
        }

        Vector2 castDirection = _movement.LastNonZeroDirection;

        UpdateAnimatorDirection(castDirection);
        _animator.SetTrigger(AttackTriggerHash);

        yield return new WaitForSeconds(_projectileSpawnDelay);

        SpawnProjectile(castDirection);

        float remainingTime = _castDuration - _projectileSpawnDelay;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        if (_lockMovementDuringCast)
        {
            _movement.SetMovementLocked(false);
        }

        _isCasting = false;
    }

    private void SpawnProjectile(Vector2 direction)
    {
        if (_projectilePrefab == null || _castPoint == null)
        {
            Debug.LogError($"{name}: Cannot spawn projectile. Missing prefab or cast point.");
            return;
        }

        _castPoint.localPosition = GetCastOffset(direction);

        MagicProjectile projectileInstance = Instantiate(_projectilePrefab, _castPoint.position, Quaternion.identity);
        projectileInstance.Initialize(direction, _damage);
    }

    private Vector2 GetCastOffset(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return _castOffsetUp;
        }

        if (direction == Vector2.down)
        {
            return _castOffsetDown;
        }

        if (direction == Vector2.left)
        {
            return _castOffsetLeft;
        }

        return _castOffsetRight;
    }

    private void UpdateAnimatorDirection(Vector2 direction)
    {
        _animator.SetFloat(AttackXHash, direction.x);
        _animator.SetFloat(AttackYHash, direction.y);
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

        if (_castPoint == null)
        {
            Debug.LogError($"{name}: CastPoint reference is missing.");
        }

        if (_projectilePrefab == null)
        {
            Debug.LogError($"{name}: Projectile prefab reference is missing.");
        }
    }
}
