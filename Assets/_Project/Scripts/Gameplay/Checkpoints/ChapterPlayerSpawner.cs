using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterPlayerSpawner : MonoBehaviour
{
    [Header("Spawn References")]
    [SerializeField] private Transform _defaultSpawnPoint;
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

        if (_defaultSpawnPoint == null)
        {
            Debug.LogError("Cannot spawn player: default spawn point is missing.");
            return;
        }

        _spawnedPlayerInstance = Instantiate(
            _playerPrefab,
            _defaultSpawnPoint.position,
            Quaternion.identity);

        if (_cameraFollow != null)
        {
            _cameraFollow.SetTarget(_spawnedPlayerInstance.transform);
        }
    }
}
