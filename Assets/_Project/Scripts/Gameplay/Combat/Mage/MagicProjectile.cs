using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MagicProjectile : MonoBehaviour
{
    private static readonly int ExplodeTriggerHash = Animator.StringToHash("Explode");

    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider2D;

    [Header("Settings")]
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private float _explodeDuration = 0.25f;
    [SerializeField] private int _damage = 1;

    private Vector2 _direction;
    private bool _isInitialized;
    private bool _isExploding;

    public void Initialize(Vector2 direction, int damage)
    {
        _direction = direction.normalized;
        _damage = damage;
        _isInitialized = true;

        RotateVisualToDirection(_direction);
        Destroy(gameObject, _lifeTime);
    }

    private void Awake()
    {
        if (_animator == null)
        {
            Debug.LogError($"{name}: Animator reference is missing.");
        }

        if (_collider2D == null)
        {
            _collider2D = GetComponent<Collider2D>();
        }

        if (_collider2D == null)
        {
            Debug.LogError($"{name}: Collider2D reference is missing.");
        }
    }

    private void Update()
    {
        if (!_isInitialized || _isExploding)
        {
            return;
        }

        transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isExploding)
        {
            return;
        }

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            StartExplosion();
            return;
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    private void StartExplosion()
    {
        _isExploding = true;

        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }

        if (_animator != null)
        {
            _animator.SetTrigger(ExplodeTriggerHash);
        }

        CancelInvoke();
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(_explodeDuration);
        Destroy(gameObject);
    }

    private void RotateVisualToDirection(Vector2 direction)
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