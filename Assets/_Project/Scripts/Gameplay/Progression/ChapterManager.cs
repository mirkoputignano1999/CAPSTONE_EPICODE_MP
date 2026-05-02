using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterManager : MonoBehaviour
{
    [Header("Chapter Info")]
    [SerializeField] private string _chapterId;
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private bool _isKeyChapter;
    [SerializeField] private string _nextSceneName;

    [Header("Gate Settings")]
    [SerializeField] private string _chapterThreeSceneName = "Chapter_03_Shared";
    [SerializeField] private string _continueCharacterSelectSceneName = "ContinueCharacterSelect";

    [Header("UI")]
    [SerializeField] private ChapterChoiceUI _chapterChoiceUI;

    private bool _isCompleting;

    public void BeginChapterCompletion()
    {
        if (_isCompleting)
        {
            return;
        }

        _isCompleting = true;
        PauseGameplay();

        if (_chapterChoiceUI != null)
        {
            _chapterChoiceUI.Show(this);
        }
        else
        {
            CompleteChapter(null, null);
        }
    }

    public void CompleteChapter(string choiceId, string rewardAbilityId)
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            Debug.LogError("GameManager or GameStateManager is missing.");
            ResumeGameplay();
            return;
        }

        GameStateManager gameStateManager = GameManager.Instance.GameStateManager;

        gameStateManager.CompleteChapter(_characterType, _chapterId, _isKeyChapter);

        if (!string.IsNullOrWhiteSpace(rewardAbilityId))
        {
            gameStateManager.UnlockAbility(_characterType, rewardAbilityId);
        }

        if (!string.IsNullOrWhiteSpace(choiceId))
        {
            gameStateManager.RegisterChoice(choiceId);
        }

        if (!string.IsNullOrWhiteSpace(_nextSceneName))
        {
            gameStateManager.SetCurrentChapter(_characterType, _nextSceneName);
            gameStateManager.SetLastCheckpointId(_characterType, string.Empty);
        }

        GameManager.Instance.SaveCurrentGame();

        ResumeGameplay();
        GoToNextStep();
    }

    private void PauseGameplay()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGameplay()
    {
        Time.timeScale = 1f;
    }

    private void GoToNextStep()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameStateManager == null)
        {
            return;
        }

        bool isSwordChapterTwo = _chapterId == "Chapter_02_Sword";
        bool isMageChapterTwo = _chapterId == "Chapter_02_Mage";

        if (isSwordChapterTwo || isMageChapterTwo)
        {
            if (GameManager.Instance.GameStateManager.AreBothChapterTwosCompleted())
            {
                SceneManager.LoadScene(_chapterThreeSceneName);
                return;
            }

            string blockedMessage = _characterType switch
            {
                CharacterType.Sword => "Il Mago deve ancora completare il Capitolo 2 prima di poter proseguire.",
                CharacterType.Mage => "Il Guerriero deve ancora completare il Capitolo 2 prima di poter proseguire.",
                _ => "L'altro personaggio deve ancora completare il Capitolo 2."
            };

            GameManager.Instance.SetPendingContinueMessage(blockedMessage);
            SceneManager.LoadScene(_continueCharacterSelectSceneName);
            return;
        }

        if (!string.IsNullOrWhiteSpace(_nextSceneName))
        {
            SceneManager.LoadScene(_nextSceneName);
            return;
        }

        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }
}