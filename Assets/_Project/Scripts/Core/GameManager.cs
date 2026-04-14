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