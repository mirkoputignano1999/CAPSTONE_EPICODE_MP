using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private string _swordStartSceneName = "Chapter_01_Sword";
    [SerializeField] private string _mageStartSceneName = "Chapter_01_Mage";

    public void OnSelectSword()
    {
        StartGameWithCharacter(CharacterType.Sword, _swordStartSceneName);
    }

    public void OnSelectMage()
    {
        StartGameWithCharacter(CharacterType.Mage, _mageStartSceneName);
    }

    public void OnBackPressed()
    {
        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }

    private void StartGameWithCharacter(CharacterType characterType, string sceneName)
    {
        GameManager.Instance.GameStateManager.SetActiveCharacter(characterType);
        GameManager.Instance.SceneFlowManager.LoadChapterScene(sceneName);
    }
}