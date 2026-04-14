using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void LoadChapterScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Cannot load chapter scene: invalid scene name.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadCombatTest()
    {
        SceneManager.LoadScene("Combat_Test");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}