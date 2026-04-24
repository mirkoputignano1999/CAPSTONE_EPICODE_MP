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
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _castPoint;

    [Header("Projectile Attack")]
    [SerializeField] private MagicProjectile _projectilePrefab;

    [Header("Beam Attack")]
    [SerializeField] private MageBeam _beamPrefab;

    [Header("Cast Settings")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _castDuration = 0.35f;
    [SerializeField] private float _spellSpawnDelay = 0.12f;
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
        if (_health != null && _health.IsDead)
        {
            return;
        }

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

        yield return new WaitForSeconds(_spellSpawnDelay);

        SpawnSelectedSpell(castDirection);

        float remainingTime = _castDuration - _spellSpawnDelay;

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

    private void SpawnSelectedSpell(Vector2 direction)
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        if (GameManager.Instance.GameStateManager.HasAbilityUnlocked(CharacterType.Mage, "Mage_Beam"))
        {
            SpawnBeam(direction);
            return;
        }

        if (GameManager.Instance.GameStateManager.HasAbilityUnlocked(CharacterType.Mage, "Mage_LightBow"))
        {
            SpawnProjectile(direction);
            return;
        }

        SpawnProjectile(direction);
    }

    private void SpawnBeam(Vector2 direction)
    {
        if (_beamPrefab == null || _castPoint == null)
        {
            Debug.LogError($"{name}: Cannot spawn beam. Missing prefab or cast point.");
            return;
        }

        _castPoint.localPosition = GetCastOffset(direction);

        MageBeam beamInstance = Instantiate(_beamPrefab, _castPoint.position, Quaternion.identity);
        beamInstance.Initialize(direction, _damage);
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
    }
}