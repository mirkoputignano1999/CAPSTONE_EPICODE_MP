using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRange : MonoBehaviour
{
    [SerializeField] private SlimeController _slimeController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterBase character = other.GetComponentInParent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        _slimeController.SetTarget(character.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CharacterBase character = other.GetComponentInParent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        _slimeController.ClearTarget(character.transform);
    }
}
