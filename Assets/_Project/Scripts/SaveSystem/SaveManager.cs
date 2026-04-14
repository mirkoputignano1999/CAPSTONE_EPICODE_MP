using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveManager : MonoBehaviour
{
    private const string SaveFileName = "save_slot_01.json";

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public bool SaveExists()
    {
        return File.Exists(SaveFilePath);
    }

    public void SaveGame(GameStateManager gameStateManager)
    {
        if (gameStateManager == null)
        {
            Debug.LogError("Save failed: GameStateManager is null.");
            return;
        }

        SaveData saveData = gameStateManager.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Game saved at: {SaveFilePath}");
    }

    public SaveData LoadGame()
    {
        if (!SaveExists())
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        string json = File.ReadAllText(SaveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        return saveData;
    }

    public void DeleteSave()
    {
        if (!SaveExists())
        {
            return;
        }

        File.Delete(SaveFilePath);
        Debug.Log("Save file deleted.");
    }
}