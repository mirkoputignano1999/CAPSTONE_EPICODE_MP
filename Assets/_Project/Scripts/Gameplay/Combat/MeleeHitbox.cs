using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private readonly HashSet<IDamageable> _damagedTargets = new();

    private Collider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();

        if (_collider2D == null)
        {
            Debug.LogError($"{name}: Collider2D is missing.");
            return;
        }

        _collider2D.enabled = false;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void ActivateHitbox()
    {
        _damagedTargets.Clear();
        _collider2D.enabled = true;
    }

    public void DeactivateHitbox()
    {
        _collider2D.enabled = false;
        _damagedTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

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
