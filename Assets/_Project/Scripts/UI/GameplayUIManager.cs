using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject _healthPanel;
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    [SerializeField] private GameMessageUI _gameMessageUI;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    [Header("Menu Scenes")]
    [SerializeField]
    private string[] _menuSceneNames =
    {
        "Bootstrap",
        "MainMenu",
        "CharacterSelect",
        "ContinueCharacterSelect"
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        RefreshVisibility(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshVisibility(scene.name);
        HideInteractionPrompt();
    }

    private void RefreshVisibility(string sceneName)
    {
        bool isMenuScene = IsMenuScene(sceneName);

        if (_healthPanel != null)
        {
            _healthPanel.SetActive(!isMenuScene);
        }

        if (isMenuScene)
        {
            HideInteractionPrompt();
        }
    }

    private bool IsMenuScene(string sceneName)
    {
        foreach (string menuSceneName in _menuSceneNames)
        {
            if (sceneName == menuSceneName)
            {
                return true;
            }
        }

        return false;
    }

    public void RegisterPlayer(GameObject playerInstance)
    {
        if (playerInstance == null)
        {
            return;
        }

        PlayerHealth playerHealth = playerInstance.GetComponent<PlayerHealth>();

        if (_playerHealthUI != null)
        {
            _playerHealthUI.SetTarget(playerHealth);
        }
    }

    public void ShowMessage(string message)
    {
        if (_gameMessageUI == null || string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _gameMessageUI.ShowMessage(message);
    }

    public void ShowMessage(string message, float duration)
    {
        if (_gameMessageUI == null || string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _gameMessageUI.ShowMessage(message, duration);
    }

    public void ShowInteractionPrompt(string prompt)
    {
        if (_interactionPromptUI == null || string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }

        _interactionPromptUI.Show(prompt);
    }

    public void HideInteractionPrompt()
    {
        if (_interactionPromptUI == null)
        {
            return;
        }

        _interactionPromptUI.Hide();
    }
}