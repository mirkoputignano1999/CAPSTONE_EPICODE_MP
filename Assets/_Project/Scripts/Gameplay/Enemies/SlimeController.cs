using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private EnemyAnimatorController2D _animatorController;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;

    [Header("Combat")]
    [SerializeField] private int _contactDamage = 1;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackRange = 0.9f;
    [SerializeField] private LayerMask _playerLayerMask;

    private Transform _target;
    private float _lastAttackTime;
    private bool _isHurting;
    private bool _isDead;

    private void Awake()
    {
        ValidateReferences();
    }

    private void Update()
    {
        if (_isHurting)
        {
            _animatorController.UpdateMovement(Vector2.zero);
            return;
        }

        if (_target == null)
        {
            _animatorController.UpdateMovement(Vector2.zero);
            return;
        }

        Vector2 direction = ((Vector2)_target.position - _rigidbody2D.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        if (distanceToTarget <= _attackRange)
        {
            _animatorController.UpdateMovement(Vector2.zero);
            TryAttack();
            return;
        }

        _animatorController.UpdateMovement(direction);
    }

    private void FixedUpdate()
    {
        if (_isDead || _isHurting || _target == null)
        {
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        if (distanceToTarget <= _attackRange)
        {
            return;
        }

        Vector2 direction = ((Vector2)_target.position - _rigidbody2D.position).normalized;
        Vector2 newPosition = _rigidbody2D.position + direction * (_moveSpeed * Time.fixedDeltaTime);

        _rigidbody2D.MovePosition(newPosition);
    }

    private void TryAttack()
    {
        if (_target == null)
        {
            return;
        }

        if (Time.time < _lastAttackTime + _attackCooldown)
        {
            return;
        }

        Vector2 directionToTarget = ((Vector2)_target.position - _rigidbody2D.position).normalized;
        _animatorController.SetFacingDirection(directionToTarget);
        _animatorController.UpdateMovement(Vector2.zero);

        _lastAttackTime = Time.time;
        _animatorController.TriggerAttack();

        Collider2D hit = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayerMask);

        if (hit == null)
        {
            return;
        }

        PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(_contactDamage);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        _target = targetTransform;
    }

    public void ClearTarget(Transform targetTransform)
    {
        if (_target == targetTransform)
        {
            _target = null;
        }
    }

    public void OnHurt()
    {
        if (_isDead)
        {
            return;
        }

        _isHurting = true;
        _animatorController.TriggerHurt();
        Invoke(nameof(EndHurt), 0.2f);
    }

    public void OnDeath()
    {
        _isDead = true;
        _animatorController.UpdateMovement(Vector2.zero);
        _animatorController.SetDead();
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void EndHurt()
    {
        _isHurting = false;
    }

    private void ValidateReferences()
    {
        if (_health == null)
        {
            Debug.LogError($"{name}: EnemyHealth reference is missing.");
        }

        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (_animatorController == null)
        {
            Debug.LogError($"{name}: EnemyAnimatorController2D reference is missing.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}