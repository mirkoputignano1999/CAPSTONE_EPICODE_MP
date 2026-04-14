using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        InitializeNewState();
    }

    public void InitializeNewState()
    {
        CurrentState = new GameState();
    }

    public void SetActiveCharacter(CharacterType characterType)
    {
        CurrentState.ActiveCharacterType = characterType;
    }

    public CharacterProgress GetCharacterProgress(CharacterType characterType)
    {
        return characterType switch
        {
            CharacterType.Sword => CurrentState.SwordProgress,
            CharacterType.Mage => CurrentState.MageProgress,
            _ => null
        };
    }

    public void RegisterChoice(string choiceId)
    {
        if (string.IsNullOrWhiteSpace(choiceId))
        {
            return;
        }

        if (!CurrentState.GlobalChoiceIds.Contains(choiceId))
        {
            CurrentState.GlobalChoiceIds.Add(choiceId);
        }
    }

    public void MarkCheckpointReached(CharacterType characterType, string checkpointId)
    {
        CharacterProgress progress = GetCharacterProgress(characterType);

        if (progress == null || string.IsNullOrWhiteSpace(checkpointId))
        {
            return;
        }

        if (!progress.ReachedCheckpointIds.Contains(checkpointId))
        {
            progress.ReachedCheckpointIds.Add(checkpointId);
        }
    }

    public void CompleteChapter(CharacterType characterType, string chapterId, bool isKeyChapter)
    {
        CharacterProgress progress = GetCharacterProgress(characterType);

        if (progress == null || string.IsNullOrWhiteSpace(chapterId))
        {
            return;
        }

        progress.CurrentChapterId = chapterId;

        if (!progress.CompletedChapterIds.Contains(chapterId))
        {
            progress.CompletedChapterIds.Add(chapterId);
        }

        if (isKeyChapter)
        {
            progress.HasReachedKeyChapter = true;
        }
    }

    public void UnlockAbility(CharacterType characterType, string abilityId)
    {
        CharacterProgress progress = GetCharacterProgress(characterType);

        if (progress == null || string.IsNullOrWhiteSpace(abilityId))
        {
            return;
        }

        if (!progress.UnlockedAbilityIds.Contains(abilityId))
        {
            progress.UnlockedAbilityIds.Add(abilityId);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (saveData == null)
        {
            InitializeNewState();
            return;
        }

        CurrentState = new GameState
        {
            ActiveCharacterType = saveData.ActiveCharacterType,
            SwordProgress = saveData.SwordProgress ?? new CharacterProgress(CharacterType.Sword),
            MageProgress = saveData.MageProgress ?? new CharacterProgress(CharacterType.Mage),
            GlobalChoiceIds = saveData.GlobalChoiceIds ?? new System.Collections.Generic.List<string>()
        };
    }

    public SaveData ToSaveData()
    {
        return new SaveData
        {
            ActiveCharacterType = CurrentState.ActiveCharacterType,
            SwordProgress = CurrentState.SwordProgress,
            MageProgress = CurrentState.MageProgress,
            GlobalChoiceIds = CurrentState.GlobalChoiceIds
        };
    }
}