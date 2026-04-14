using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private string _swordChapterSceneName = "Chapter_01_Sword";
    [SerializeField] private string _mageChapterSceneName = "Chapter_01_Mage";

    public void OnSelectSword()
    {
        GameManager.Instance.GameStateManager.SetActiveCharacter(CharacterType.Sword);
        GameManager.Instance.SceneFlowManager.LoadChapterScene(_swordChapterSceneName);
    }

    public void OnSelectMage()
    {
        GameManager.Instance.GameStateManager.SetActiveCharacter(CharacterType.Mage);
        GameManager.Instance.SceneFlowManager.LoadChapterScene(_mageChapterSceneName);
    }

    public void OnBackPressed()
    {
        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }
}