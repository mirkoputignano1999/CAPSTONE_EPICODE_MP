using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MageBeam : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 0.2f;
    [SerializeField] private int _damage = 1;

    private readonly HashSet<IDamageable> _damagedTargets = new();

    public void Initialize(Vector2 direction, int damage)
    {
        _damage = damage;
        RotateToDirection(direction);
        Destroy(gameObject, _lifeTime);
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

    private void RotateToDirection(Vector2 direction)
    {
        float angle = direction switch
        {
            var d when d == Vector2.right => 0f,
            var d when d == Vector2.up => 90f,
            var d when d == Vector2.left => 180f,
            var d when d == Vector2.down => -90f,
            _ => 0f
        };

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}