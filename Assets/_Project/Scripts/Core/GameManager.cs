using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public GameStateManager GameStateManager { get; private set; }
    [field: SerializeField] public SaveManager SaveManager { get; private set; }
    [field: SerializeField] public SceneFlowManager SceneFlowManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewGame()
    {
        GameStateManager.InitializeNewState();
    }

    public void ContinueGame()
    {
        SaveData saveData = SaveManager.LoadGame();
        GameStateManager.LoadFromSaveData(saveData);
    }

    public string GetResumeSceneName(CharacterType characterType)
    {
        if (GameStateManager == null)
        {
            return string.Empty;
        }

        return GameStateManager.GetCurrentChapter(characterType);
    }

    public void ContinueGameAs(CharacterType characterType)
    {
        if (GameStateManager == null)
        {
            return;
        }

        GameStateManager.SetActiveCharacter(characterType);
    }

    public void StartCharacterStory(CharacterType characterType, string startSceneName)
    {
        if (GameStateManager == null)
        {
            return;
        }

        GameStateManager.SetActiveCharacter(characterType);

        if (string.IsNullOrWhiteSpace(GameStateManager.GetCurrentChapter(characterType)))
        {
            GameStateManager.SetCurrentChapter(characterType, startSceneName);
            GameStateManager.SetLastCheckpointId(characterType, string.Empty);
        }

        SaveCurrentGame();
        SceneFlowManager.LoadChapterScene(startSceneName);
    }

    public string PendingContinueMessage { get; private set; } = string.Empty;

    public void SetPendingContinueMessage(string message)
    {
        PendingContinueMessage = message ?? string.Empty;
    }

    public string ConsumePendingContinueMessage()
    {
        string message = PendingContinueMessage;
        PendingContinueMessage = string.Empty;
        return message;
    }

    public void SaveCurrentGame()
    {
        SaveManager.SaveGame(GameStateManager);
    }

    public void DeleteCurrentSave()
    {
        SaveManager.DeleteSave();
        GameStateManager.InitializeNewState();
    }
}