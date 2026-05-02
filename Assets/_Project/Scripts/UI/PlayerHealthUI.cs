using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthText;

    private PlayerHealth _trackedHealth;

    public void SetTarget(PlayerHealth playerHealth)
    {
        UnbindCurrentTarget();

        _trackedHealth = playerHealth;

        if (_trackedHealth == null)
        {
            ClearUI();
            return;
        }

        _trackedHealth.OnHealthChanged += HandleHealthChanged;
        _trackedHealth.OnDied += HandlePlayerDied;

        Refresh();
    }

    private void OnDestroy()
    {
        UnbindCurrentTarget();
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        UpdateUI(currentHealth, maxHealth);
    }

    private void HandlePlayerDied()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (_trackedHealth == null)
        {
            ClearUI();
            return;
        }

        UpdateUI(_trackedHealth.CurrentHealth, _trackedHealth.MaxHealth);
    }

    private void UpdateUI(int currentHealth, int maxHealth)
    {
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHealth;
        }

        if (_healthText != null)
        {
            _healthText.text = $"HP: {currentHealth} / {maxHealth}";
        }
    }

    private void ClearUI()
    {
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = 1;
            _healthSlider.value = 0;
        }

        if (_healthText != null)
        {
            _healthText.text = "HP: -";
        }
    }

    private void UnbindCurrentTarget()
    {
        if (_trackedHealth == null)
        {
            return;
        }

        _trackedHealth.OnHealthChanged -= HandleHealthChanged;
        _trackedHealth.OnDied -= HandlePlayerDied;
        _trackedHealth = null;
    }
}