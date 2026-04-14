using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionGate : MonoBehaviour
{
    public bool CanAdvanceToSharedProgression()
    {
        CharacterProgress swordProgress = GameManager.Instance.GameStateManager.GetCharacterProgress(CharacterType.Sword);
        CharacterProgress mageProgress = GameManager.Instance.GameStateManager.GetCharacterProgress(CharacterType.Mage);

        if (swordProgress == null || mageProgress == null)
        {
            return false;
        }

        return swordProgress.HasReachedKeyChapter && mageProgress.HasReachedKeyChapter;
    }
}