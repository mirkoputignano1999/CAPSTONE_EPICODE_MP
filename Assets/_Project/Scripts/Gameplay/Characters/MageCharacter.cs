using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCharacter : CharacterBase
{
    [SerializeField] private MageCombat _mageCombat;

    protected override void ValidateReferences()
    {
        base.ValidateReferences();

        if (_mageCombat == null)
        {
            Debug.LogError($"{name}: MageCombat reference is missing.");
        }
    }
}
