using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected CharacterType _characterType;
    [SerializeField] protected PlayerMovement _movement;
    [SerializeField] protected PlayerHealth _health;
    [SerializeField] protected PlayerInputHandler _inputHandler;
    [SerializeField] protected PlayerAnimatorController _animatorController;

    public CharacterType CharacterType => _characterType;

    protected virtual void Awake()
    {
        ValidateReferences();
    }

    protected virtual void ValidateReferences()
    {
        if (_movement == null)
        {
            Debug.LogError($"{name}: PlayerMovement reference is missing.");
        }

        if (_health == null)
        {
            Debug.LogError($"{name}: PlayerHealth reference is missing.");
        }

        if (_inputHandler == null)
        {
            Debug.LogError($"{name}: PlayerInputHandler reference is missing.");
        }

        if (_animatorController == null)
        {
            Debug.LogError($"{name}: PlayerAnimatorController reference is missing.");
        }
    }
}