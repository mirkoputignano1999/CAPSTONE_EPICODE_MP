using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnNewGamePressed()
    {
        GameManager.Instance.StartNewGame();
        GameManager.Instance.SceneFlowManager.LoadCharacterSelect();
    }

    public void OnContinuePressed()
    {
        if (!GameManager.Instance.SaveManager.SaveExists())
        {
            Debug.LogWarning("Continue pressed, but no save exists.");
            return;
        }

        GameManager.Instance.ContinueGame();

        string resumeSceneName = GameManager.Instance.GetResumeSceneName();

        if (string.IsNullOrWhiteSpace(resumeSceneName))
        {
            GameManager.Instance.SceneFlowManager.LoadCharacterSelect();
            return;
        }

        GameManager.Instance.SceneFlowManager.LoadChapterScene(resumeSceneName);
    }

    public void OnDeleteSavePressed()
    {
        GameManager.Instance.DeleteCurrentSave();
    }

    public void OnExitPressed()
    {
        GameManager.Instance.SceneFlowManager.QuitGame();
    }
}