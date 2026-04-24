using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterChoiceUI : MonoBehaviour
{
    [SerializeField] private GameObject _rootPanel;

    [Header("Choice A")]
    [SerializeField] private string _choiceAId;
    [SerializeField] private string _choiceARewardAbilityId;

    [Header("Choice B")]
    [SerializeField] private string _choiceBId;
    [SerializeField] private string _choiceBRewardAbilityId;

    private ChapterManager _currentChapterManager;

    private void Awake()
    {
        Hide();
    }

    public void Show(ChapterManager chapterManager)
    {
        _currentChapterManager = chapterManager;

        if (_rootPanel != null)
        {
            _rootPanel.SetActive(true);
        }
    }

    public void Hide()
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(false);
        }
    }

    public void OnChoiceAPressed()
    {
        SubmitChoice(_choiceAId, _choiceARewardAbilityId);
    }

    public void OnChoiceBPressed()
    {
        SubmitChoice(_choiceBId, _choiceBRewardAbilityId);
    }

    private void SubmitChoice(string choiceId, string rewardAbilityId)
    {
        if (_currentChapterManager == null)
        {
            Debug.LogError("ChapterChoiceUI: ChapterManager is missing.");
            return;
        }

        Hide();
        _currentChapterManager.CompleteChapter(choiceId, rewardAbilityId);
    }
}