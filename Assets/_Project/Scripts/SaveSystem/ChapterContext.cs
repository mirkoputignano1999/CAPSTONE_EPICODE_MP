using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterContext : MonoBehaviour
{
    [SerializeField] private CharacterType _characterType;

    private void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        GameManager.Instance.GameStateManager.SetCurrentChapter(_characterType, sceneName);
    }
}