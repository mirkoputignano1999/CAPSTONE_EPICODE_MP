using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCombat : MonoBehaviour
{
    private static readonly int AttackXHash = Animator.StringToHash("AttackX");
    private static readonly int AttackYHash = Animator.StringToHash("AttackY");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int AttackModeHash = Animator.StringToHash("AttackMode");

    private const int BaseProjectileMode = 0;
    private const int BeamMode = 1;
    private const int LightBowMode = 2;

    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _castPoint;

    [Header("Base Projectile")]
    [SerializeField] private MagicProjectile _projectilePrefab;

    [Header("Beam Ability")]
    [SerializeField] private MageBeam _beamPrefab;

    [Header("Light Bow Ability")]
    [SerializeField] private LightBowProjectile _lightBowProjectilePrefab;

    [Header("Damage")]
    [SerializeField] private int _damage = 1;

    [Header("Cast Timing")]
    [SerializeField] private float _baseProjectileCastDuration = 0.35f;
    [SerializeField] private float _baseProjectileSpawnDelay = 0.12f;

    [SerializeField] private float _beamCastDuration = 0.35f;
    [SerializeField] private float _beamSpawnDelay = 0.12f;

    [SerializeField] private float _lightBowCastDuration = 0.6f;
    [SerializeField] private float _lightBowSpawnDelay = 0.18f;

    [SerializeField] private bool _lockMovementDuringCast = true;

    [Header("Projectile / Light Bow Offsets")]
    [SerializeField] private Vector2 _castOffsetUp = new(0f, 0.45f);
    [SerializeField] private Vector2 _castOffsetDown = new(0f, -0.45f);
    [SerializeField] private Vector2 _castOffsetLeft = new(-0.45f, 0f);
    [SerializeField] private Vector2 _castOffsetRight = new(0.45f, 0f);

    [Header("Beam Offsets")]
    [SerializeField] private Vector2 _beamOffsetUp = new(0f, 0.8f);
    [SerializeField] private Vector2 _beamOffsetDown = new(0f, -0.8f);
    [SerializeField] private Vector2 _beamOffsetLeft = new(-0.8f, 0f);
    [SerializeField] private Vector2 _beamOffsetRight = new(0.8f, 0f);

    private bool _isCasting;

    public bool IsCasting => _isCasting;

    private enum MageSpellType
    {
        BaseProjectile = 0,
        Beam = 1,
        LightBow = 2
    }

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
        MageSpellType selectedSpell = GetSelectedSpellType();

        float spawnDelay = GetSpawnDelay(selectedSpell);
        float castDuration = GetCastDuration(selectedSpell);

        UpdateAnimatorDirection(castDirection);
        SetAttackMode(selectedSpell);
        _animator.SetTrigger(AttackTriggerHash);

        yield return new WaitForSeconds(spawnDelay);

        SpawnSelectedSpell(selectedSpell, castDirection);

        float remainingTime = castDuration - spawnDelay;

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

    private MageSpellType GetSelectedSpellType()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return MageSpellType.BaseProjectile;
        }

        GameStateManager gameStateManager = GameManager.Instance.GameStateManager;

        if (gameStateManager.HasAbilityUnlocked(CharacterType.Mage, "Mage_Beam"))
        {
            return MageSpellType.Beam;
        }

        if (gameStateManager.HasAbilityUnlocked(CharacterType.Mage, "Mage_LightBow"))
        {
            return MageSpellType.LightBow;
        }

        return MageSpellType.BaseProjectile;
    }

    private float GetSpawnDelay(MageSpellType spellType)
    {
        return spellType switch
        {
            MageSpellType.Beam => _beamSpawnDelay,
            MageSpellType.LightBow => _lightBowSpawnDelay,
            _ => _baseProjectileSpawnDelay
        };
    }

    private float GetCastDuration(MageSpellType spellType)
    {
        return spellType switch
        {
            MageSpellType.Beam => _beamCastDuration,
            MageSpellType.LightBow => _lightBowCastDuration,
            _ => _baseProjectileCastDuration
        };
    }

    private void SetAttackMode(MageSpellType spellType)
    {
        int modeValue = spellType switch
        {
            MageSpellType.BaseProjectile => BaseProjectileMode,
            MageSpellType.Beam => BeamMode,
            MageSpellType.LightBow => LightBowMode,
            _ => BaseProjectileMode
        };

        _animator.SetInteger(AttackModeHash, modeValue);
    }

    private void SpawnSelectedSpell(MageSpellType spellType, Vector2 direction)
    {
        switch (spellType)
        {
            case MageSpellType.Beam:
                SpawnBeam(direction);
                break;

            case MageSpellType.LightBow:
                SpawnLightBowProjectile(direction);
                break;

            default:
                SpawnProjectile(direction);
                break;
        }
    }

    private void SpawnBeam(Vector2 direction)
    {
        if (_beamPrefab == null)
        {
            Debug.LogError($"{name}: Beam prefab is missing.");
            return;
        }

        Vector2 spawnPosition = (Vector2)transform.position + GetBeamOffset(direction);

        MageBeam beamInstance = Instantiate(_beamPrefab, spawnPosition, Quaternion.identity);
        beamInstance.Initialize(direction, _damage);
    }

    private void SpawnLightBowProjectile(Vector2 direction)
    {
        if (_lightBowProjectilePrefab == null || _castPoint == null)
        {
            Debug.LogError($"{name}: LightBow projectile prefab or cast point is missing.");
            return;
        }

        _castPoint.localPosition = GetCastOffset(direction);

        LightBowProjectile projectileInstance = Instantiate(
            _lightBowProjectilePrefab,
            _castPoint.position,
            Quaternion.identity);

        projectileInstance.Initialize(direction, _damage);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        if (_projectilePrefab == null || _castPoint == null)
        {
            Debug.LogError($"{name}: Base projectile prefab or cast point is missing.");
            return;
        }

        _castPoint.localPosition = GetCastOffset(direction);

        MagicProjectile projectileInstance = Instantiate(
            _projectilePrefab,
            _castPoint.position,
            Quaternion.identity);

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

    private Vector2 GetBeamOffset(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return _beamOffsetUp;
        }

        if (direction == Vector2.down)
        {
            return _beamOffsetDown;
        }

        if (direction == Vector2.left)
        {
            return _beamOffsetLeft;
        }

        return _beamOffsetRight;
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

        if (_health == null)
        {
            Debug.LogError($"{name}: PlayerHealth reference is missing.");
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