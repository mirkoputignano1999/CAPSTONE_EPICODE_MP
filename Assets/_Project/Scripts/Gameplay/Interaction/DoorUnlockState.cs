using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockState : MonoBehaviour
{
    [SerializeField] private bool _isUnlocked;

    public bool IsUnlocked => _isUnlocked;

    public void Unlock()
    {
        _isUnlocked = true;
    }
}