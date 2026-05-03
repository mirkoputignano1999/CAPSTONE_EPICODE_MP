using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;

    public bool IsOpen => _root != null && _root.activeSelf;

    private void Awake()
    {
        HideImmediate();
    }

    public void Toggle()
    {
        if (IsOpen)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (_root != null)
        {
            _root.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (_root != null)
        {
            _root.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null && GameManager.Instance.SceneFlowManager != null)
        {
            GameManager.Instance.SceneFlowManager.LoadMainMenu();
            return;
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void HideImmediate()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }
    }
}