using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    [SerializeField] private string _chapterId;
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private bool _isKeyChapter;
    [SerializeField] private string _rewardAbilityId;

    public void CompleteChapter(string choiceId = null)
    {
        GameStateManager gameStateManager = GameManager.Instance.GameStateManager;

        gameStateManager.CompleteChapter(_characterType, _chapterId, _isKeyChapter);

        if (!string.IsNullOrWhiteSpace(_rewardAbilityId))
        {
            gameStateManager.UnlockAbility(_characterType, _rewardAbilityId);
        }

        if (!string.IsNullOrWhiteSpace(choiceId))
        {
            gameStateManager.RegisterChoice(choiceId);
        }

        GameManager.Instance.SaveCurrentGame();
    }
}