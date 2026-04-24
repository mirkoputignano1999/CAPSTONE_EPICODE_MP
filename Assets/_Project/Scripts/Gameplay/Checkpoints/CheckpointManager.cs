using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] private Transform _defaultSpawnPoint;
    [SerializeField] private List<Checkpoint> _sceneCheckpoints = new();

    private Checkpoint _currentCheckpoint;

    public Transform CurrentSpawnPoint => _currentCheckpoint != null ? _currentCheckpoint.RespawnPoint : _defaultSpawnPoint;
    public string CurrentCheckpointId => _currentCheckpoint != null ? _currentCheckpoint.CheckpointId : string.Empty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint == null)
        {
            return;
        }

        _currentCheckpoint = checkpoint;
        Debug.Log($"Checkpoint activated: {checkpoint.CheckpointId}");
    }

    public void ResetToDefault()
    {
        _currentCheckpoint = null;
    }

    public bool TryRestoreCheckpoint(string checkpointId)
    {
        if (string.IsNullOrWhiteSpace(checkpointId))
        {
            _currentCheckpoint = null;
            return false;
        }

        Debug.Log($"Trying to restore checkpoint ID: {checkpointId}");

        foreach (Checkpoint checkpoint in _sceneCheckpoints)
        {
            if (checkpoint == null)
            {
                Debug.LogWarning("Checkpoint list contains a null entry.");
                continue;
            }

            Debug.Log($"Scene checkpoint registered: {checkpoint.CheckpointId}");

            if (checkpoint.CheckpointId == checkpointId)
            {
                _currentCheckpoint = checkpoint;
                Debug.Log($"Checkpoint restored: {checkpoint.CheckpointId}");
                return true;
            }
        }

        Debug.LogWarning($"Checkpoint ID not found in scene: {checkpointId}");
        _currentCheckpoint = null;
        return false;
    }
}