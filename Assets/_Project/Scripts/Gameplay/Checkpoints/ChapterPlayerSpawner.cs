using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterPlayerSpawner : MonoBehaviour
{
    [Header("Spawn References")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("Optional References")]
    [SerializeField] private CameraFollow _cameraFollow;

    private GameObject _spawnedPlayerInstance;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (_spawnedPlayerInstance != null)
        {
            return;
        }

        if (_playerPrefab == null)
        {
            Debug.LogError("Cannot spawn player: player prefab is missing.");
            return;
        }

        if (CheckpointManager.Instance == null || CheckpointManager.Instance.CurrentSpawnPoint == null)
        {
            Debug.LogError("Cannot spawn player: checkpoint manager or spawn point is missing.");
            return;
        }

        _spawnedPlayerInstance = Instantiate(
            _playerPrefab,
            CheckpointManager.Instance.CurrentSpawnPoint.position,
            Quaternion.identity);

        if (GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.RegisterPlayer(_spawnedPlayerInstance);
        }

        if (_cameraFollow != null)
        {
            _cameraFollow.SetTarget(_spawnedPlayerInstance.transform);
        }
    }

    public void RespawnPlayer()
    {
        if (_spawnedPlayerInstance != null)
        {
            Destroy(_spawnedPlayerInstance);
        }

        _spawnedPlayerInstance = null;
        SpawnPlayer();
    }
}