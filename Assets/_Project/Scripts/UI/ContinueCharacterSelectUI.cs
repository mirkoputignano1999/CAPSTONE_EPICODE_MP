using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinueCharacterSelectUI : MonoBehaviour
{
    [Header("Sword UI")]
    [SerializeField] private Button _swordButton;
    [SerializeField] private TMP_Text _swordButtonText;

    [Header("Mage UI")]
    [SerializeField] private Button _mageButton;
    [SerializeField] private TMP_Text _mageButtonText;

    [Header("Info Message")]
    [SerializeField] private TMP_Text _infoMessageText;

    [Header("Start Scenes")]
    [SerializeField] private string _swordStartSceneName = "Chapter_01_Sword";
    [SerializeField] private string _mageStartSceneName = "Chapter_01_Mage";

    private void Start()
    {
        RefreshButtons();
        RefreshPendingMessage();
    }

    private void RefreshButtons()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        bool swordStarted = GameManager.Instance.GameStateManager.HasStartedCharacter(CharacterType.Sword);
        bool mageStarted = GameManager.Instance.GameStateManager.HasStartedCharacter(CharacterType.Mage);

        if (_swordButton != null)
        {
            _swordButton.gameObject.SetActive(true);
        }

        if (_mageButton != null)
        {
            _mageButton.gameObject.SetActive(true);
        }

        if (_swordButtonText != null)
        {
            _swordButtonText.text = swordStarted ? "Continue Sword" : "Start Sword";
        }

        if (_mageButtonText != null)
        {
            _mageButtonText.text = mageStarted ? "Continue Mage" : "Start Mage";
        }
    }

    private void RefreshPendingMessage()
    {
        if (_infoMessageText == null || GameManager.Instance == null)
        {
            return;
        }

        string message = GameManager.Instance.ConsumePendingContinueMessage();
        _infoMessageText.text = message;
        _infoMessageText.gameObject.SetActive(!string.IsNullOrWhiteSpace(message));
    }

    public void OnSwordPressed()
    {
        HandleCharacter(CharacterType.Sword, _swordStartSceneName);
    }

    public void OnMagePressed()
    {
        HandleCharacter(CharacterType.Mage, _mageStartSceneName);
    }

    public void OnBackPressed()
    {
        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }

    private void HandleCharacter(CharacterType characterType, string startSceneName)
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        bool hasStarted = GameManager.Instance.GameStateManager.HasStartedCharacter(characterType);

        if (hasStarted)
        {
            string resumeScene = GameManager.Instance.GetResumeSceneName(characterType);

            if (string.IsNullOrWhiteSpace(resumeScene))
            {
                Debug.LogWarning($"No resume scene found for {characterType}.");
                return;
            }

            GameManager.Instance.ContinueGameAs(characterType);
            GameManager.Instance.SceneFlowManager.LoadChapterScene(resumeScene);
            return;
        }

        GameManager.Instance.StartCharacterStory(characterType, startSceneName);
    }
}