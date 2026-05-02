using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string _continueCharacterSelectSceneName = "ContinueCharacterSelect";

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
        GameManager.Instance.SceneFlowManager.LoadChapterScene(_continueCharacterSelectSceneName);
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