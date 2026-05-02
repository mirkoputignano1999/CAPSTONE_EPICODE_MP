using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string _checkpointId;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private bool _activateOnlyOnce = true;

    private bool _isActivated;

    public string CheckpointId => _checkpointId;
    public Transform RespawnPoint => _respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_activateOnlyOnce && _isActivated)
        {
            return;
        }

        CharacterBase character = other.GetComponentInParent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_checkpointId))
        {
            Debug.LogError($"{name}: Checkpoint ID is missing.");
            return;
        }

        if (_respawnPoint == null)
        {
            Debug.LogError($"{name}: Respawn point is missing.");
            return;
        }

        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("CheckpointManager instance is missing.");
            return;
        }

        CheckpointManager.Instance.SetCheckpoint(this);
        _isActivated = true;

        GameStateManager gameStateManager = GameManager.Instance.GameStateManager;
        CharacterType activeCharacter = gameStateManager.CurrentState.ActiveCharacterType;
        gameStateManager.SetLastCheckpointId(activeCharacter, _checkpointId);
        GameManager.Instance.SaveCurrentGame();

        if (GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage("Checkpoint raggiunto");
        }
    }
}