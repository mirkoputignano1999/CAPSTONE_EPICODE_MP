using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SpinSlashHitbox : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private readonly HashSet<IDamageable> _damagedTargets = new();

    private CircleCollider2D _collider2D;
    private float _defaultRadius;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_collider2D == null)
        {
            _collider2D = GetComponent<CircleCollider2D>();
        }

        if (_collider2D == null)
        {
            Debug.LogError($"{name}: CircleCollider2D is missing.");
            return;
        }

        _defaultRadius = _collider2D.radius;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetRadius(float radius)
    {
        Initialize();

        if (_collider2D == null)
        {
            return;
        }

        _collider2D.radius = radius;
    }

    public void ResetRadius()
    {
        Initialize();

        if (_collider2D == null)
        {
            return;
        }

        _collider2D.radius = _defaultRadius;
    }

    public void ActivateHitbox()
    {
        Initialize();

        if (_collider2D == null)
        {
            return;
        }

        _damagedTargets.Clear();
        _collider2D.enabled = true;
    }

    public void DeactivateHitbox()
    {
        Initialize();

        if (_collider2D == null)
        {
            return;
        }

        _collider2D.enabled = false;
        _damagedTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        if (_damagedTargets.Contains(damageable))
        {
            return;
        }

        _damagedTargets.Add(damageable);
        damageable.TakeDamage(_damage);
    }
}