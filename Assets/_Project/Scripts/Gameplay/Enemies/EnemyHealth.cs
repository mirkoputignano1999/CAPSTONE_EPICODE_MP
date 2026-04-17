using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private bool _destroyOnDeath = true;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsDead => _currentHealth <= 0;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || IsDead)
        {
            return;
        }

        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        Debug.Log($"{name} took {damage} damage. Current HP: {_currentHealth}");

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} died.");

        if (_destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}