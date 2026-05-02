using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LightBowProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 9f;
    [SerializeField] private float _lifeTime = 1.5f;
    [SerializeField] private float _impactDestroyDelay = 0.1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider2D;

    private Vector2 _direction;
    private bool _isInitialized;
    private bool _hasImpacted;

    public void Initialize(Vector2 direction, int damage)
    {
        _direction = direction.normalized;
        _damage = damage;
        _isInitialized = true;

        RotateToDirection(_direction);
        Destroy(gameObject, _lifeTime);
    }

    private void Awake()
    {
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
        if (!_isInitialized || _hasImpacted)
        {
            return;
        }

        transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasImpacted)
        {
            return;
        }

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            Impact();
            return;
        }

        if (!other.isTrigger)
        {
            Impact();
        }
    }

    private void Impact()
    {
        _hasImpacted = true;

        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }

        StartCoroutine(DestroyAfterImpact());
    }

    private IEnumerator DestroyAfterImpact()
    {
        yield return new WaitForSeconds(_impactDestroyDelay);
        Destroy(gameObject);
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
