using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
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
        Destroy(gameObject);
    }
}