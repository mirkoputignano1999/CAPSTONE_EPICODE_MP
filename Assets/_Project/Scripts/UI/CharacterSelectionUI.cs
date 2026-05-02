using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private string _swordStartSceneName = "Chapter_01_Sword";
    [SerializeField] private string _mageStartSceneName = "Chapter_01_Mage";

    public void OnSelectSword()
    {
        StartCharacter(CharacterType.Sword, _swordStartSceneName);
    }

    public void OnSelectMage()
    {
        StartCharacter(CharacterType.Mage, _mageStartSceneName);
    }

    public void OnBackPressed()
    {
        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }

    private void StartCharacter(CharacterType characterType, string startSceneName)
    {
        if (GameManager.Instance == null || GameManager.Instance.SaveManager == null)
        {
            return;
        }

        if (!GameManager.Instance.SaveManager.SaveExists())
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
            GameManager.Instance.ContinueGame();
        }

        GameManager.Instance.StartCharacterStory(characterType, startSceneName);
    }
}