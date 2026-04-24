using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _maxHealth = 5;

    [Header("Invulnerability")]
    [SerializeField] private float _invulnerabilityDuration = 0.5f;

    [Header("Death")]
    [SerializeField] private float _deathReloadDelay = 3f;

    [Header("Flash Feedback")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _flashInterval = 0.08f;
    [SerializeField] private float _flashAlpha = 0.35f;

    [Header("References")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerAnimatorController _animatorController;

    private int _currentHealth;
    private bool _isInvulnerable;
    private bool _isDead;
    private Coroutine _flashCoroutine;
    private Color _originalColor = Color.white;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsDead => _isDead;
    public bool IsInvulnerable => _isInvulnerable;

    private void Awake()
    {
        _currentHealth = _maxHealth;

        if (_movement == null)
        {
            Debug.LogError($"{name}: PlayerMovement reference is missing.");
        }

        if (_animatorController == null)
        {
            Debug.LogError($"{name}: PlayerAnimatorController reference is missing.");
        }

        if (_spriteRenderer == null)
        {
            Debug.LogError($"{name}: SpriteRenderer reference is missing.");
        }
        else
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || _isDead || _isInvulnerable)
        {
            return;
        }

        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        if (_currentHealth == 0)
        {
            Die();
            return;
        }

        StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        _isInvulnerable = true;

        //if (_animatorController != null)
        //{
        //    _animatorController.TriggerHurt();
        //}

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(FlashRoutine());

        yield return new WaitForSeconds(_invulnerabilityDuration);

        _isInvulnerable = false;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        ResetSpriteVisual();
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            SetSpriteAlpha(_flashAlpha);
            yield return new WaitForSeconds(_flashInterval);

            SetSpriteAlpha(1f);
            yield return new WaitForSeconds(_flashInterval);
        }
    }

    private void Die()
    {
        _isDead = true;
        _isInvulnerable = true;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        ResetSpriteVisual();

        if (_movement != null)
        {
            _movement.SetMovementLocked(true);
        }

        if (_animatorController != null)
        {
            _animatorController.SetDead();
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(_deathReloadDelay);

        ChapterPlayerSpawner playerSpawner = FindFirstObjectByType<ChapterPlayerSpawner>();

        if (playerSpawner != null)
        {
            playerSpawner.RespawnPlayer();
        }
        else
        {
            Debug.LogError("ChapterPlayerSpawner not found. Cannot respawn player.");
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (_spriteRenderer == null)
        {
            return;
        }

        Color color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }

    private void ResetSpriteVisual()
    {
        if (_spriteRenderer == null)
        {
            return;
        }

        _spriteRenderer.color = _originalColor;
    }
}