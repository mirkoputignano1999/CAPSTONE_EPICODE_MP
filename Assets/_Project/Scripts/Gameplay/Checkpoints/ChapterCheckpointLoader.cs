using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChapterCheckpointLoader : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        if (CheckpointManager.Instance == null)
        {
            return;
        }

        CharacterType activeCharacter = GameManager.Instance.GameStateManager.CurrentState.ActiveCharacterType;
        string checkpointId = GameManager.Instance.GameStateManager.GetLastCheckpointId(activeCharacter);

        CheckpointManager.Instance.TryRestoreCheckpoint(checkpointId);
    }
}