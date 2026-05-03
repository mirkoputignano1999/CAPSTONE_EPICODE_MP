using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string _continueCharacterSelectSceneName = "ContinueCharacterSelect";
    [SerializeField] private string _message = "Il salvataggio è stato cancellato.";

    public void OnNewGamePressed()
    {
        GameManager.Instance.StartNewGame();
        GameManager.Instance.SceneFlowManager.LoadCharacterSelect();
    }

    public void OnContinuePressed()
    {
        if (!GameManager.Instance.SaveManager.SaveExists())
        {
            {
                GameplayUIManager.Instance.ShowMessage("Nessun salvataggio disponibile.", 3f);
            }
            return;
        }

        GameManager.Instance.ContinueGame();
        GameManager.Instance.SceneFlowManager.LoadChapterScene(_continueCharacterSelectSceneName);
    }

    public void OnDeleteSavePressed()
    {
        if (GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage(_message, 3f);
        }
        else
        {
            Debug.Log(_message);
        }
        GameManager.Instance.DeleteCurrentSave();
    }

    public void OnExitPressed()
    {
        GameManager.Instance.SceneFlowManager.QuitGame();
    }
}

