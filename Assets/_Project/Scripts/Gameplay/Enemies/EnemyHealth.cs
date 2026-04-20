using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private SlimeController _slimeController;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
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
            return;
        }

        if (_slimeController != null)
        {
            _slimeController.OnHurt();
        }
    }

    private void Die()
    {
        if (_slimeController != null)
        {
            _slimeController.OnDeath();
        }

        Destroy(gameObject, 0.9f);
    }
}