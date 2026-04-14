using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string _checkpointId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        GameManager.Instance.GameStateManager.MarkCheckpointReached(character.CharacterType, _checkpointId);
        GameManager.Instance.SaveCurrentGame();

        Debug.Log($"Checkpoint reached: {_checkpointId}");
    }
}
