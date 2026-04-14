using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterProgress
{
    public CharacterType CharacterType;
    public string CurrentChapterId;
    public List<string> UnlockedAbilityIds = new();
    public List<string> ReachedCheckpointIds = new();
    public List<string> CompletedChapterIds = new();
    public bool HasReachedKeyChapter;

    public CharacterProgress(CharacterType characterType)
    {
        CharacterType = characterType;
        CurrentChapterId = string.Empty;
    }
}
