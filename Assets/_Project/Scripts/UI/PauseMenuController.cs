using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PauseMenuUI _pauseMenuUI;

    [Header("Input")]
    [SerializeField] private InputActionReference _pauseAction;

    [Header("Menu Scenes")]
    [SerializeField]
    private string[] _menuSceneNames =
    {
        "Bootstrap",
        "MainMenu",
        "CharacterSelect",
        "ContinueCharacterSelect"
    };

    private void OnEnable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.action.Enable();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.action.Disable();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (IsMenuScene(SceneManager.GetActiveScene().name))
        {
            return;
        }

        if (_pauseAction == null)
        {
            return;
        }

        if (_pauseAction.action.WasPressedThisFrame())
        {
            if (_pauseMenuUI != null)
            {
                _pauseMenuUI.Toggle();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_pauseMenuUI != null && _pauseMenuUI.IsOpen)
        {
            _pauseMenuUI.ResumeGame();
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
}