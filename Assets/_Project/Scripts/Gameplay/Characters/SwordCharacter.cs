using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCharacter : CharacterBase
{
    [SerializeField] private SwordCombat _swordCombat;

    protected override void ValidateReferences()
    {
        base.ValidateReferences();

        if (_swordCombat == null)
        {
            Debug.LogError($"{name}: SwordCombat reference is missing.");
        }
    }
}